using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(PhysicsBasedCharacterController))]
public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 JoystickSize = new Vector2(300, 300);
    [SerializeField]
    private FloatingJoystick Joystick;
    private Finger MovementFinger;

    private PhysicsBasedCharacterController _physicsBasedCharacterController;

    private void Awake()
    {
        _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            float distance = Vector2.Distance(
                    currentTouch.screenPosition,
                    Joystick.RectTransform.anchoredPosition
                );

            if (distance > maxMovement)
            {
                knobPosition = (
                    currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition
                    ).normalized
                    * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            Vector2 movementAmount = knobPosition / maxMovement;
            _physicsBasedCharacterController.MoveInputTouchAction(movementAmount);
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            _physicsBasedCharacterController.MoveInputTouchAction(Vector2.zero);
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        // if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        if (MovementFinger == null && TouchedFinger.screenPosition.x <= Screen.width)
        {
            MovementFinger = TouchedFinger;
            _physicsBasedCharacterController.MoveInputTouchAction(Vector2.zero);
            Joystick.gameObject.SetActive(true);
            Joystick.RectTransform.sizeDelta = JoystickSize;
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x)
        {
            StartPosition.x = JoystickSize.x;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x)
        {
            StartPosition.x = Screen.width - JoystickSize.x;
        }

        if (StartPosition.y < JoystickSize.y)
        {
            StartPosition.y = JoystickSize.y;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y)
        {
            StartPosition.y = Screen.height - JoystickSize.y;
        }

        return StartPosition;
    }

    // private void Update()
    // {
    //     Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(
    //         MovementAmount.x,
    //         0,
    //         MovementAmount.y
    //     );

    //     Player.transform.LookAt(Player.transform.position + scaledMovement, Vector3.up);
    //     Player.Move(scaledMovement);
    // }

    private void OnGUI()
    {
        GUIStyle labelStyle = new GUIStyle()
        {
            fontSize = 24,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        if (MovementFinger != null)
        {
            GUI.Label(new Rect(10, 35, 500, 20), $"Finger Start Position: {MovementFinger.currentTouch.startScreenPosition}", labelStyle);
            GUI.Label(new Rect(10, 65, 500, 20), $"Finger Current Position: {MovementFinger.currentTouch.screenPosition}", labelStyle);
        }
        else
        {
            GUI.Label(new Rect(10, 35, 500, 20), "No Current Movement Touch", labelStyle);
        }

        GUI.Label(new Rect(10, 10, 500, 20), $"Screen Size ({Screen.width}, {Screen.height})", labelStyle);
    }
}

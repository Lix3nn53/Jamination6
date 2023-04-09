using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollowPath : MonoBehaviour
{
  [SerializeField] private Transform pointsParent;
  [SerializeField] private float duration = 2f;
  [SerializeField] private float waitAtEnd = 1f;
  [SerializeField] private float delay = 2f;

  private int currentPointIndex = 0;
  private Vector3 startPoint;
  private Vector3 endPoint;
  private float timeElapsed = 0f;
  private bool isWaiting = false;

  private void Start()
  {
    startPoint = pointsParent.GetChild(currentPointIndex).position;
    if (pointsParent.childCount > 1)
    {
      endPoint = pointsParent.GetChild(currentPointIndex + 1).position;
    }
    else
    {
      endPoint = startPoint;
    }
    currentPointIndex = (currentPointIndex + 1) % pointsParent.childCount;

    StartCoroutine(Delay());
  }

  private IEnumerator Delay()
  {
    isWaiting = true;
    yield return new WaitForSeconds(delay);
    isWaiting = false;
  }

  private void Update()
  {
    if (isWaiting)
    {
      return;
    }

    timeElapsed += Time.deltaTime;

    // Calculate the percentage of time elapsed as a value between 0 and 1
    float t = Mathf.Clamp01(timeElapsed / duration);

    // Move the platform between the start and end points using Lerp
    Vector3 pos = Vector3.Lerp(startPoint, endPoint, t);

    if (GetComponent<Oscillator>())
    {
      Oscillator oscillator = GetComponent<Oscillator>();
      oscillator.localEquilibriumPosition = pos;
      Vector3 tempPos = Vector3.zero;
      for (int i = 0; i < 3; i++)
      {
        if (oscillator.forceScale[i] == 0)
        {
          tempPos[i] = pos[i];
        }
        else
        {
          tempPos[i] = transform.position[i];
        }
      }
      transform.position = tempPos;
    }
    else
    {
      transform.position = pos;
    }

    // If we have reached the end point, switch the start and end points
    if (t == 1f)
    {
      SwitchPoints();
    }
  }

  private void SwitchPoints()
  {
    startPoint = endPoint;
    currentPointIndex = (currentPointIndex + 1) % pointsParent.childCount;
    endPoint = pointsParent.GetChild(currentPointIndex).position;
    timeElapsed = 0f;
    StartCoroutine(WaitAtEnd());
  }

  private IEnumerator WaitAtEnd()
  {
    isWaiting = true;
    yield return new WaitForSeconds(waitAtEnd);
    isWaiting = false;
  }
}

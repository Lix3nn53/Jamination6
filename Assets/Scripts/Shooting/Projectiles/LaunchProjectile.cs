using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchProjectile : MonoBehaviour
{
  [SerializeField] private Transform _launchPoint;
  [SerializeField] private GameObject _projectilePrefab;
  [SerializeField] private float _timeToArrive = 2f;
  [SerializeField] private float _reloadTime = 2f;
  public float ReloadTime => _reloadTime;

  // public void Launch(Vector3 playerVelocity)
  // {
  //   GameObject projectile = Instantiate(_projectilePrefab, _launchPoint.position, _launchPoint.rotation);
  //   Rigidbody rb = projectile.GetComponent<Rigidbody>();

  //   // rb.velocity = _launchPoint.forward * _launchForce;
  //   rb.AddForce(_launchPoint.forward * _launchForce, ForceMode.Impulse);
  //   rb.AddForce(playerVelocity, ForceMode.Impulse);
  // }

  public void LaunchToMouse()
  {
    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
      Vector3 velocity = CalculateVelocity(hit.point, _launchPoint.position, _timeToArrive);

      _launchPoint.rotation = Quaternion.LookRotation(velocity);

      GameObject projectile = Instantiate(_projectilePrefab, _launchPoint.position, _launchPoint.rotation);
      Rigidbody rb = projectile.GetComponent<Rigidbody>();

      rb.velocity = velocity;
    }
  }

  public Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
  {
    Vector3 distance = target - origin;
    Vector3 distanceXZ = distance;
    distanceXZ.y = 0f;

    float sY = distance.y;
    float sXZ = distanceXZ.magnitude;

    float vY = sY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;
    float vXZ = sXZ / time;

    Vector3 result = distanceXZ.normalized;
    result *= vXZ;
    result.y = vY;

    return result;
  }
}

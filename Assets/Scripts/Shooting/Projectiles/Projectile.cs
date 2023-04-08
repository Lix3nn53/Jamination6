using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] private float _life = 5;

  [SerializeField] private GameObject _spawnOnHitPrefab;

  void Start()
  {
    Destroy(gameObject, _life);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (_spawnOnHitPrefab != null)
    {
      Instantiate(_spawnOnHitPrefab, transform.position, transform.rotation);
    }
    Destroy(gameObject);
  }
}

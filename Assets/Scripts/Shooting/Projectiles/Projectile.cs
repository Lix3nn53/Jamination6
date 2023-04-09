using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] private float _life = 5;

  [SerializeField] private GameObject _spawnOnHitPrefab;

  void Start()
  {
    if (_life > 0)
    {
      Destroy(gameObject, _life);
    }

    StartCoroutine(ActivatePlayerCollisionAfterTime(0.4f));
  }

  private IEnumerator ActivatePlayerCollisionAfterTime(float time)
  {
    yield return new WaitForSeconds(time);
    gameObject.layer = LayerMask.NameToLayer("Enemy");
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (_spawnOnHitPrefab != null)
    {
      Instantiate(_spawnOnHitPrefab, transform.position, _spawnOnHitPrefab.transform.rotation);
      Destroy(gameObject);
    }
  }
}

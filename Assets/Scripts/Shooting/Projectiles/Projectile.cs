using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  [SerializeField] private float _life = 5;

  void Start()
  {
    Destroy(gameObject, _life);
  }
}

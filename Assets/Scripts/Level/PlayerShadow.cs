using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class PlayerShadow : MonoBehaviour
{
  [SerializeField] private LayerMask _groundMask;
  private Transform _playerTransform;
  private SpriteRenderer _spriteRenderer;
  // Start is called before the first frame update
  void Start()
  {
    _playerTransform = ServiceLocator.Get<Player>().transform;

    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    ShadowFollowPlayer();
  }

  public void ShadowFollowPlayer()
  {
    // Ray from player to ground
    Ray ray = new Ray(_playerTransform.position, Vector3.down);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 1000f, _groundMask))
    {
      transform.position = hit.point + Vector3.up * 0.1f;
      _spriteRenderer.enabled = true;
    }
    else
    {
      _spriteRenderer.enabled = false;
    }
  }
}

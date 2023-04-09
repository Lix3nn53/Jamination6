using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lix.Core;

public class FogTrigger : MonoBehaviour
{
  [SerializeField] private Cinemachine.CinemachineVirtualCamera vcam;

  private GameManager _gameManager;

  private void Start()
  {
    _gameManager = ServiceLocator.Get<GameManager>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      // Stop tracking the player
      vcam.Follow = null;
      vcam.LookAt = null;

      _gameManager.OnPlayerHealthChangeEvent?.Invoke(0);
      _gameManager.OnGameOverEvent?.Invoke(_gameManager.Score);

      StartCoroutine(StopPlayerPhysics(other));
    }
    else
    {
      // destroy other
      Destroy(other.gameObject, 2f);
    }
  }

  // Stop player physics after sometime
  private IEnumerator StopPlayerPhysics(Collider other)
  {
    yield return new WaitForSeconds(2f);
    // Stop player physics
    // player.GetComponent<Rigidbody>().isKinematic = true;
    other.gameObject.SetActive(false);
  }
}

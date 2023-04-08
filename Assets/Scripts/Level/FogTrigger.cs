using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : MonoBehaviour
{
  [SerializeField] private Cinemachine.CinemachineVirtualCamera vcam;

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      // Stop tracking the player
      vcam.Follow = null;
      vcam.LookAt = null;

      StartCoroutine(StopPlayerPhysics(other));
    }
  }

  // Stop player physics after sometime
  private IEnumerator StopPlayerPhysics(Collider other)
  {
    yield return new WaitForSeconds(1f);
    // Stop player physics
    // player.GetComponent<Rigidbody>().isKinematic = true;
    other.gameObject.SetActive(false);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lix.Core;

public class EnemyNavMesh : MonoBehaviour
{
  private Player _player;
  private NavMeshAgent _navMeshAgent;
  // Start is called before the first frame update
  void Start()
  {
    _player = ServiceLocator.Get<Player>();

    _navMeshAgent = GetComponent<NavMeshAgent>();
  }

  // Update is called once per frame
  void Update()
  {
    _navMeshAgent.SetDestination(_player.transform.position);
  }
}

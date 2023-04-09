using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaOfEffect : MonoBehaviour
{
  [SerializeField] private float _interval = 1f;

  private bool _cooldown = false;
  private SphereCollider _sphereCollider;
  public abstract void OnAreaOfEffect(CombatUnit combatUnit);

  private void Start()
  {
    _sphereCollider = GetComponent<SphereCollider>();
    StartCoroutine(RepeatingEffect());
  }

  private IEnumerator RepeatingEffect()
  {
    while (true)
    {
      yield return new WaitForSeconds(_interval);
      ApplyEffectToArea();
    }
  }

  private void ApplyEffectToArea()
  {
    Collider[] colliders = Physics.OverlapSphere(transform.position, _sphereCollider.radius * transform.localScale.x);

    foreach (Collider collider in colliders)
    {
      CombatUnit combatUnit = collider.GetComponent<CombatUnit>();
      if (combatUnit != null)
      {
        OnAreaOfEffect(combatUnit);
      }
    }
  }
}

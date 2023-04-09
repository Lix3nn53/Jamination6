using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
  private int _health;
  public int Health => _health;
  [SerializeField] private int _maxHealth;
  public int MaxHealth => _maxHealth;
  [SerializeField] private int _meleeDamage;
  public int MeleeDamage => _meleeDamage;
  [SerializeField] float _meleeCooldown = 0.1f;
  public float MeleeCooldown => _meleeCooldown;
  private bool _canMelee = true;

  // Start is called before the first frame update
  public virtual void Start()
  {
    _health = _maxHealth;

    if (_meleeCooldown <= 0 || _meleeDamage <= 0)
    {
      _canMelee = false;
    }
  }

  public void MeleeAttack(CombatUnit target)
  {
    target.TakeDamage(MeleeDamage);
    OnMelee();
    StartCoroutine(MeleeCooldownCoroutine());
  }

  private IEnumerator MeleeCooldownCoroutine()
  {
    _canMelee = false;
    yield return new WaitForSeconds(_meleeCooldown);
    _canMelee = true;
  }

  private void OnCollisionStay(Collision collision)
  {
    if (!_canMelee) return;

    // Check if the collision is a combat unit
    CombatUnit combatUnit = collision.gameObject.GetComponent<CombatUnit>();
    if (combatUnit != null)
    {
      MeleeAttack(combatUnit);
    }
  }

  public void TakeDamage(int damage)
  {
    _health -= damage;
    if (_health <= 0)
    {
      Die();
    }
    OnTakeDamage();
  }

  public void Die()
  {
    OnDeath();
  }

  public abstract void OnDeath();
  public abstract void OnMelee();
  public abstract void OnTakeDamage();
}

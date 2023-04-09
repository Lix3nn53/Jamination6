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
  [SerializeField] private float _pushForce = 10f;
  private bool _canMelee = true;

  // Damage Tint
  private Material[] _materials;
  private float _damageTakeInterval = 0.2f;
  private bool _isTakingDamage = false;

  // Start is called before the first frame update
  public virtual void Start()
  {
    _health = _maxHealth;

    if (_meleeCooldown <= 0 || _meleeDamage <= 0)
    {
      _canMelee = false;
    }

    // Damage Tint
    _materials = GetComponentInChildren<Renderer>().materials;
  }

  public void MeleeAttack(CombatUnit target)
  {
    target.TakeDamage(MeleeDamage);
    PushOnMelee(target);
    StartCoroutine(MeleeCooldownCoroutine());
    OnMelee();
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
      // check if combat unit is player type because we only want to melee attack the player
      if (combatUnit is Player)
      {
        MeleeAttack(combatUnit);
      }
    }
  }

  public void TakeDamage(int damage)
  {
    if (_isTakingDamage) return;

    _health -= damage;
    if (_health <= 0)
    {
      Die();
    }
    DamageTintColor();
    StartCoroutine(DamageTakeCooldown());
    OnTakeDamage();
  }

  public void Die()
  {
    OnDeath();
  }

  public abstract void OnDeath();
  public abstract void OnMelee();
  public abstract void OnTakeDamage();

  private void DamageTintColor()
  {
    foreach (Material material in _materials)
    {
      StartCoroutine(DamageTintColorForOne(material));
    }
  }

  private IEnumerator DamageTintColorForOne(Material material)
  {
    Color originalColor = material.color;
    material.color = Color.red;
    yield return new WaitForSeconds(_damageTakeInterval);
    material.color = originalColor;
  }

  private IEnumerator DamageTakeCooldown()
  {
    _isTakingDamage = true;
    yield return new WaitForSeconds(_damageTakeInterval);
    _isTakingDamage = false;
  }

  private void PushOnMelee(CombatUnit target)
  {
    Rigidbody rb = target.GetComponent<Rigidbody>();
    if (rb != null)
    {
      Vector3 direction = target.transform.position - transform.position;
      direction.Normalize();
      rb.AddForce(direction * _pushForce, ForceMode.Impulse);
    }
  }
}

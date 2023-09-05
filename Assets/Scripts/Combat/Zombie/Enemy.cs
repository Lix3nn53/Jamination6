using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using LlamAcademy.Sensors;
using UnityEngine.AI;
using Lix.Core;

public abstract class Enemy : MonoBehaviour
{
    public MeshRenderer MeshRenderer;

    [Header("Config")]
    public int MaxHealth = 100;
    public int Health = 100;
    private bool _isTakingDamage = false;
    private float _damageTakeInterval = 0.2f;

    public virtual void OnEnable()
    {
        Health = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isTakingDamage) return;

        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        else
        {
            DamageTintColor();
            StartCoroutine(DamageTakeCooldown());
        }
    }

    private void DamageTintColor()
    {
        foreach (Material material in MeshRenderer.materials)
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

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}

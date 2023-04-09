using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
  [SerializeField] private int _damage = 20;
  [SerializeField] private float _pushPower = 20f;
  [SerializeField] private float _harmfulStartAt = 1f;
  [SerializeField] private float _harmfulFor = 4f;
  [SerializeField] private Color _harmfulColor = Color.red;
  [SerializeField] Renderer _renderer;
  private Material _material;
  private bool _isHarmful = false;
  public bool IsHarmful => _isHarmful;
  private Color _originalColor;
  private List<CombatUnit> _alreadyHit = new List<CombatUnit>();
  void Start()
  {
    _renderer = GetComponentInChildren<MeshRenderer>();
    _material = _renderer.material;
    _isHarmful = false;
    _originalColor = _material.color;

    StartCoroutine(HarmfulForTime(_harmfulFor));
  }

  private IEnumerator HarmfulForTime(float time)
  {
    yield return new WaitForSeconds(_harmfulStartAt);
    _isHarmful = true;
    _material.color = _harmfulColor;
    yield return new WaitForSeconds(time);
    _isHarmful = false;
    _material.color = _originalColor;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (!_isHarmful) return;

    // Check if the collision is a combat unit
    CombatUnit combatUnit = collision.gameObject.GetComponent<CombatUnit>();
    if (combatUnit != null)
    {
      if (_alreadyHit.Contains(combatUnit))
      {
        return;
      }
      combatUnit.TakeDamage(_damage);
      _alreadyHit.Add(combatUnit);
      Rigidbody rb = combatUnit.GetComponent<Rigidbody>();
      if (rb != null)
      {

        // rb.AddForceAtPosition(collision.contacts[0].normal * _pushPower, collision.contacts[0].point, ForceMode.Impulse);
        // push combatUnit away from boulder
        Vector3 direction = combatUnit.transform.position - transform.position;
        direction.Normalize();
        rb.AddForce(direction * _pushPower, ForceMode.Impulse);
      }
    }
  }
}

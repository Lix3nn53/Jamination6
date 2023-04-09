using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
  [SerializeField] private float _harmfulFor = 2f;
  [SerializeField] private Color _harmfulColor = Color.red;
  [SerializeField] Renderer _renderer;
  private Material _material;

  private bool _isHarmful = false;
  public bool IsHarmful => _isHarmful;
  private Color _originalColor;
  void Start()
  {
    _renderer = GetComponentInChildren<MeshRenderer>();
    _material = _renderer.material;
    _isHarmful = false;
    _originalColor = _material.color;
    _material.color = _harmfulColor;

    StartCoroutine(HarmfulForTime(_harmfulFor));
  }

  private IEnumerator HarmfulForTime(float time)
  {
    yield return new WaitForSeconds(time);
    _isHarmful = false;
    _material.color = _originalColor;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenShaderPropertyAtStart : MonoBehaviour
{
  private Material _material;
  [SerializeField] private string _propertyName = "_Dissolve";
  [SerializeField] private float _startValue = 0;
  [SerializeField] private float _endValue = 1;
  [SerializeField] private float _duration = 1;


  // Start is called before the first frame update
  void Start()
  {
    _material = GetComponent<Renderer>().material;
    _material.SetFloat(_propertyName, _startValue);

    StartCoroutine(Tween());
  }

  public IEnumerator Tween()
  {
    float t = 0;
    while (t < _duration)
    {
      t += Time.deltaTime;
      _material.SetFloat(_propertyName, Mathf.Lerp(_startValue, _endValue, t / _duration));
      yield return null;
    }
  }
}

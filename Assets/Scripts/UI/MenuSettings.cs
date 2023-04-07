using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;

public class MenuSettings : MonoBehaviour
{
  private AudioManager audioManager;
  [SerializeField] private Slider slider;

  // Start is called before the first frame update
  void Start()
  {
    audioManager = ServiceLocator.Get<AudioManager>();
  }

  // Update is called once per frame
  public void OnSliderValueChange()
  {
    audioManager.SetMasterVolume(slider.value);
  }
}

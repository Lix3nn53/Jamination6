using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;

public class MenuSettings : MonoBehaviour
{
  private AudioManager audioManager;
  [SerializeField] private Slider sliderMaster;
  [SerializeField] private Slider sliderMusic;
  [SerializeField] private Slider sliderVFX;

  // Start is called before the first frame update
  void Start()
  {
    audioManager = ServiceLocator.Get<AudioManager>();
  }

  public void SetMasterVolume()
  {
    audioManager.SetMasterVolume(sliderMaster.value);
  }

  public void SetMusicVolume()
  {
    audioManager.SetMusicVolume(sliderMusic.value);
  }

  public void SetSFXVolume()
  {
    audioManager.SetSFXVolume(sliderVFX.value);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lix.Core;
using System;

public class ZombieTypeToggleSprites : MonoBehaviour
{
  [SerializeField] private Sprite passiveSprite;

  [SerializeField] private Sprite activeSprite;

  private Image background;

  private void Awake()
  {
    background = this.gameObject.transform.Find("Background").GetComponent<Image>();
  }


  public void ChangeSprite(Toggle toggle)
  {
    if(toggle.isOn){
    background.sprite = activeSprite;
    }
    else{
      background.sprite = passiveSprite;
    }
  }


}

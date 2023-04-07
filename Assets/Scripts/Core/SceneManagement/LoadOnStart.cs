using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class LoadOnStart : MonoBehaviour
  {
    [SerializeField] private SplashScreen splashScreen;
    [SerializeField] private int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine(splashScreen.FadeImageAndLoad(true, sceneIndex));
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Lix.Core
{
  public class SplashScreen : MonoBehaviour
  {
    [SerializeField] private SceneLoader sceneLoader;
    private Image img;

    private void Awake()
    {
      img = GetComponent<Image>();
    }

    public IEnumerator FadeImageAndLoad(bool fadeAway, int sceneIndex)
    {
      if (fadeAway)
      {
        float increment = 0.05f;
        // loop over 1 second
        for (float i = 0; i <= 1f; i += increment * Time.deltaTime)
        {
          // set color with i as alpha
          img.color = new Color(1, 1, 1, i);
          increment += 0.2f;
          yield return new WaitForSeconds(0.01f);
        }
      }

      sceneLoader.Load(sceneIndex);
    }
  }
}
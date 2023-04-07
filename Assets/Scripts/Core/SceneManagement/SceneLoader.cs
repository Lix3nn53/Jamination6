using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Lix.Core;

namespace Lix.Core
{
  public class SceneLoader : MonoBehaviour
  {
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text percentText;
    [SerializeField] private float extraWaitTime = 0.5f;

    private float extraWaitedFor = 0f;

    private bool isLoading = false;

    public void Load(int sceneIndex)
    {
      if (isLoading)
      {
        return;
      }
      isLoading = true;

      InternalDebug.Log("Loading scene: " + sceneIndex);
      slider.gameObject.SetActive(true);

      Time.timeScale = 1;
      StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
      if (loadingScreen != null)
      {
        loadingScreen.SetActive(true);
      }

      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

      //Don't let the Scene activate until you allow it to
      operation.allowSceneActivation = false;

      //When the load is still in progress, output the Text and progress bar
      while (!operation.isDone)
      {
        // Loading = 0 - 0.9
        // Activation = 0.9 - 1.0
        float progress = Mathf.Clamp01(operation.progress / 0.9f);

        if (slider != null)
        {
          slider.value = progress;
        }
        if (percentText != null)
        {
          percentText.text = progress * 100f + "%";
        }

        // Check if the load has finished
        if (operation.progress >= 0.9f)
        {
          //Change the Text to show the Scene is ready
          // percentText.text = "Press the space bar to continue";
          // Wait to you press the space key to activate the Scene
          // if (buttonPressed)
          // Activate the Scene
          if (extraWaitedFor >= extraWaitTime)
          {
            isLoading = false;
            operation.allowSceneActivation = true; // operation is not done until this line is executed
          }
          else
          {
            extraWaitedFor += Time.deltaTime;
          }
        }

        // InternalDebug.Log(progress);
        yield return null;
      }
    }
  }
}
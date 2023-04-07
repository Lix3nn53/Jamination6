using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  [System.Serializable]
  public class Sound
  {
    public string soundName;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.3f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
  }
}
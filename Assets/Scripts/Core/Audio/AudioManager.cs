using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance;

    public Sound[] soundList;
    private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();

    private float masterVolume = 1f;
    public float MasterVolume { get { return masterVolume; } private set { masterVolume = value; } }

    // Start is called before the first frame update
    protected void Awake()
    {
      foreach (Sound s in soundList)
      {
        if (sources.ContainsKey(s.soundName)) continue;

        AudioSource source = s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume * masterVolume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        sources.Add(s.soundName, source);
      }
    }

    private void Start()
    {
      Play("Background");
    }

    public void Play(string name)
    {
      sources[name].Play();
    }

    public void SetVolume(string name, float v)
    {
      sources[name].volume = v;
    }

    public void SetMasterVolume(float v)
    {
      masterVolume = v;

      foreach (KeyValuePair<string, AudioSource> entry in sources)
      {
        entry.Value.volume = entry.Value.volume * masterVolume;
      }

      foreach (Sound s in soundList)
      {
        if (sources.ContainsKey(s.soundName))
        {
          sources[s.soundName].volume = s.volume * masterVolume;
        }
      }
    }
  }
}
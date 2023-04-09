using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lix.Core
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance;

    public Sound[] soundList;
    private Dictionary<string, Sound> soundDic = new Dictionary<string, Sound>();

    private float masterVolume = 1f;
    public float MasterVolume { get { return masterVolume; } private set { masterVolume = value; } }

    private float musicVolume = 1f;
    public float MusicVolume { get { return musicVolume; } private set { musicVolume = value; } }
    private float sfxVolume = 1f;
    public float SFXVolume { get { return sfxVolume; } private set { sfxVolume = value; } }

    // Start is called before the first frame update
    protected void Awake()
    {
      foreach (Sound s in soundList)
      {
        if (soundDic.ContainsKey(s.soundName)) continue;

        AudioSource source = s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        float multiplier = 1f;
        if (s.soundType == SoundType.Background)
        {
          multiplier = musicVolume;
        }
        else if (s.soundType == SoundType.SFX)
        {
          multiplier = sfxVolume;
        }
        s.source.volume = s.volume * masterVolume * multiplier;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        soundDic.Add(s.soundName, s);
      }
    }

    private IEnumerator Start()
    {
      AudioSource source = Play("Background Start");

      yield return new WaitUntil(() => source.isPlaying == false);

      Play("Background Loop");
    }

    public AudioSource Play(string name)
    {
      if (!soundDic.ContainsKey(name))
      {
        Debug.LogWarning("Sound: " + name + " not found");
        return null;
      }

      soundDic[name].source.Play();
      return soundDic[name].source;
    }

    public void SetVolume(string name, float v)
    {
      soundDic[name].volume = v;
    }

    public void SetMasterVolume(float v)
    {
      masterVolume = v;
      UpdateVolumes();
    }

    public void SetMusicVolume(float v)
    {
      musicVolume = v;
      UpdateVolumes();
    }

    public void SetSFXVolume(float v)
    {
      sfxVolume = v;
      UpdateVolumes();
    }

    public void UpdateVolumes()
    {
      foreach (KeyValuePair<string, Sound> entry in soundDic)
      {
        float multiplier = 1f;
        if (entry.Value.soundType == SoundType.Background)
        {
          multiplier = musicVolume;
        }
        else if (entry.Value.soundType == SoundType.SFX)
        {
          multiplier = sfxVolume;
        }

        entry.Value.source.volume = entry.Value.volume * masterVolume * multiplier;
      }
    }

    /// <summary>
    /// Get whether a sound is playing or not.
    /// </summary>
    /// <param name="name">The name of the sound.</param>
    /// <returns>Whether the sound is playing or not.</returns>
    public bool IsPlaying(string name)
    {
      if (!soundDic.ContainsKey(name))
      {
        Debug.LogWarning("Sound: " + name + " not found");
        return false;
      }

      return soundDic[name].source.isPlaying;
    }

    /// <summary>
    /// Stop playing a sound.
    /// </summary>
    /// <param name="name">The name of the sound.</param>
    public void Stop(string name)
    {
      if (!soundDic.ContainsKey(name))
      {
        Debug.LogWarning("Sound: " + name + " not found");
        return;
      }

      soundDic[name].source.Stop();
    }
  }
}
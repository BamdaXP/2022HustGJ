using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Transform BGMSources;
    public Transform SESources;
    
    public void PlayBGM(string name,float volume = 1f, float pitch = 1f)
    {
        var clip = Resources.Load<AudioClip>("Audio/BGM/" + name);
        if (clip == null)
        {
            Debug.LogWarning($"BGM {name} at Resources/Audio/BGM is not found!");
            return;
        }
        var source = BGMSources.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }
    public void StopBGM(string name)
    {
        foreach (var source in BGMSources.GetComponents<AudioSource>())
        {
            if (source.clip.name == name)
            {
                Destroy(source);
            }
        }
    }


    public void PlaySE(string name, float volume = 1f)
    {
        var clip = Resources.Load<AudioClip>("Audio/SE/" + name);
        if (clip == null)
        {
            Debug.LogWarning($"SE {name} at Resources/Audio/SE is not found!");
            return;
        }
        var source = SESources.gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.volume = volume;
        source.Play();
    }

    private void Update()
    {
        foreach (var source in SESources.GetComponents<AudioSource>())
        {
            if (!source.isPlaying)
            {
                Destroy(source);
            }
        }
    }
}

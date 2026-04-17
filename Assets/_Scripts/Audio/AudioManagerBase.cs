using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerBase : MonoBehaviour
{
    [HideInInspector] public AudioSource defaultAudioSource;
    
    public void InitSource(ref AudioSource source, AudioMixerGroup mixerGroup = null)
    {
        if (!source)
        {
            GameObject go = new GameObject("AudioSource");
            go.transform.SetParent(transform); // optional
            source = go.AddComponent<AudioSource>();
        }

        source.outputAudioMixerGroup = mixerGroup 
            ? mixerGroup 
            : AudioMixerManager.GetMasterGroup();
    }
    
    /// <summary>
    /// Plays audio clip.
    /// </summary>
    /// <param name="clip">Clip</param>
    /// <param name="volumeIn">Clip's volume</param>
    /// <param name="parent">Parent, where clip's AudioSource would be instantiated. If == null: no instantiation
    /// (plays in manager)</param>
    public void PlayClipIndependently(AudioClip clip, float? volumeIn=null, Transform parent=null)
    {
        float volume = volumeIn ?? defaultAudioSource.volume;
        
        if (parent == null)
        {
            defaultAudioSource.volume = volume;
            defaultAudioSource.PlayOneShot(clip);
            return;
        }

        AudioSource audioSource = Instantiate(defaultAudioSource, parent.position, Quaternion.identity);
        
        audioSource.clip = clip;
        audioSource.volume = volume;
        
        audioSource.Play();
        
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
        
        h.Out("Played");

    }

    public void PlayClipIndependently(string path, float? volumeIn = null, Transform parent = null)
    {
        PlayClipIndependently(Resources.Load<AudioClip>(path), volumeIn, parent);
    }
    
    public void PlayRandomClipIndependently(List<AudioClip> clip, float? volumeIn = null, Transform parent = null)
    {
        PlayClipIndependently(h.RandChoice(clip), volumeIn, parent);
    }
    
    public void PlayRandomClipIndependently(List<string> paths, float? volumeIn = null, Transform parent = null)
    {
        List<AudioClip> clips = new List<AudioClip>();
        foreach (string path in paths)
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null) clips.Add(clip);
        }
        PlayRandomClipIndependently(clips, volumeIn, parent);
    }
    
    public void PlayAudioResource(AudioResource resource, float? volumeIn = null, Transform parent = null)
    {
        float volume = volumeIn ?? defaultAudioSource.volume;

        if (parent == null)
        {
            defaultAudioSource.volume = volume;
            defaultAudioSource.resource = resource;
            defaultAudioSource.Play();
            return;
        }

        AudioSource audioSource = Instantiate(defaultAudioSource, parent.position, Quaternion.identity);

        audioSource.resource = resource;
        audioSource.volume = volume;

        audioSource.Play();

        // ⚠️ We cannot reliably get length from AudioResource
        Destroy(audioSource.gameObject, 10f); // fallback lifetime
        
        h.Out("Played");
    }

    public void PlayAudioResource(string path, float? volumeIn = null, Transform parent = null)
    {
        AudioResource resource = Resources.Load<AudioResource>(path);
        if (resource != null)
        {
            PlayAudioResource(resource, volumeIn, parent);
        }
    }

    public void PlayClip(AudioClip clip)
    {
        defaultAudioSource.clip = clip;
        defaultAudioSource.Play();
    }
    
    public void PlayClip(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null)
        {
            PlayClip(clip);
        }
    }
    
    public void PlayRandomClip(List<AudioClip> clip)
    {
        PlayClip(h.RandChoice(clip));
    }
    
    public void PlayRandomClip(List<string> paths)
    {
        List<AudioClip> clips = new List<AudioClip>();
        foreach (string path in paths)
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null) clips.Add(clip);
        }
        PlayRandomClip(clips);
    }
}

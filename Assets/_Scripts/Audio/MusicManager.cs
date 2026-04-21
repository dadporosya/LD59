using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : AudioManagerBase
{
    public static MusicManager Instance; // Instance

    [SerializeField] private AudioClip deafultBGMusic;
    private string defaultBGMusicPath = "Audio/Music/bgMusic";
    
    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource current;
    private AudioSource next;
    private void Awake()
    {
        // h.Out(Instance);
        h.CreateStaticInstance(this, ref Instance);
        // h.Out(Instance);
        
        InitSource(ref musicSourceA, AudioMixerManager.GetMusicGroup());
        InitSource(ref musicSourceB, AudioMixerManager.GetMusicGroup());
        current = musicSourceA;
        next = musicSourceB;
        InitSource(ref defaultAudioSource, AudioMixerManager.GetMusicGroup());
    }

    private void Start()
    {
        var clip = Resources.Load<AudioClip>(defaultBGMusicPath);
        // var clip = Resources.Load<AudioClip>("Audio/Music/bgMusicTest");
        if (clip) deafultBGMusic = clip;
        if (deafultBGMusic) PlayMusic(deafultBGMusic);
        else h.Out("Default music is not found'");
    }
    
    public void PlayMusic(AudioClip newClip, float fadeTime = 1.5f)
    {
        if (current.clip == newClip) return;
        StopAllCoroutines();
        StartCoroutine(CrossFade(newClip, fadeTime));
    }
    
    private IEnumerator CrossFade(AudioClip newClip, float fadeTime, bool changeSource=true)
    {
        next.clip = newClip;
        next.volume = 0f;
        next.loop = true;
        next.Play();

        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalized = t / fadeTime;

            if (changeSource)
            {
                current.volume = Mathf.Lerp(1f, 0f, normalized);
            }
            
            next.volume = Mathf.Lerp(0f, 1f, normalized);

            yield return null;
        }
        if (changeSource)
        {
            current.Stop();
            current.volume = 1f;
            (current, next) = (next, current);
        }
    }
    
    public void ShutdownMusic(float fadeTime = 1.5f)
    {
        StopAllCoroutines();
        StartCoroutine(ShutdownMusicCoroutine(fadeTime));
    }
    
    private IEnumerator ShutdownMusicCoroutine(float fadeTime)
    {
        float t = 0f;
        
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalized = t / fadeTime;
            
            current.volume = Mathf.Lerp(1f, 0f, normalized);
            
            yield return null;
        }
        
        current.Stop();
        current.volume = 1f;
        current.clip = null;
        next.Stop();
        next.volume = 1f;
        next.clip = null;
    }
    
    public void SlowDownMusic(float targetPitch = 0.5f, float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(SlowDownMusicCoroutine(targetPitch, duration));
    }
    
    private IEnumerator SlowDownMusicCoroutine(float targetPitch, float duration)
    {
        float startPitch = current.pitch;
        float t = 0f;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            
            current.pitch = Mathf.Lerp(startPitch, targetPitch, normalized);
            
            yield return null;
        }
        
        current.pitch = targetPitch;
    }
    
    public void RestoreMusicSpeed(float duration = 2f)
    {
        StopAllCoroutines();
        StartCoroutine(RestoreMusicSpeedCoroutine(duration));
    }
    
    private IEnumerator RestoreMusicSpeedCoroutine(float duration)
    {
        float startPitch = current.pitch;
        float t = 0f;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            
            current.pitch = Mathf.Lerp(startPitch, 1f, normalized);
            
            yield return null;
        }
        
        current.pitch = 1f;
    }
    
    
    
}
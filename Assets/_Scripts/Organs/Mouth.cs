using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public List<Sprite> frames;
    public float gapBetweenFrames = 0.2f;
    [SerializeField] private List<AudioClip> voice = new List<AudioClip>();


    public void Talk()
    {
        StartCoroutine(TalkCoroutine());
    }

    public IEnumerator TalkCoroutine()
    {
        if (voice != null && voice.Count > 0) SFXManager.Instance.PlayRandomClip(voice);
        foreach (var frame in frames)
        {
            GetComponent<SpriteRenderer>().sprite = frame;
            yield return new WaitForSeconds(gapBetweenFrames);
        }
    }
}

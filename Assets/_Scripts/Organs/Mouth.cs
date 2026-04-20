using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public List<Sprite> frames;
    public float gapBetweenFrames = 0.2f;
    public AudioClip clip;

    public void Talk()
    {
        StartCoroutine(TalkCoroutine());
    }

    public IEnumerator TalkCoroutine()
    {
        SFXManager.Instance.PlayClip(clip);
        foreach (var frame in frames)
        {
            GetComponent<SpriteRenderer>().sprite = frame;
            yield return new WaitForSeconds(gapBetweenFrames);
        }
    }
}

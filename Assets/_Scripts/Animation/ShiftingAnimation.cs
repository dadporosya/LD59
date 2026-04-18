using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftingAnimation : MonoBehaviour
{
    public bool _animate = true;
    private Coroutine animationCoroutine;
    public bool animate
    {
        get { return _animate; }
        set
        {
            _animate = value;
            if (_animate) StartAnimating();
            else
            {
                if (animationCoroutine != null)  StopAnimating();
            }
            
        }
    }
    [SerializeField] private List<Sprite> frames;
    [SerializeField] private bool defaultPeriod=true;
    [SerializeField] public float period;
    private int currentFrameId = 0;
    private int frameCount;
    private Image image;

    void Start()
    {
        if (defaultPeriod) period = Preferences.shiftingAnimationPeriod;
        
        if (!image) image = GetComponent<Image>();
        frameCount = frames.Count;
        currentFrameId = h.Range(0, frameCount - 1);

        if (animate) StartAnimating();
    }

    public void StartAnimating()
    {
        animationCoroutine = StartCoroutine(ShiftingAnimationCoroutine());
    }
    
    public void StopAnimating()
    {
        StopCoroutine(animationCoroutine);
    }
    
    private IEnumerator ShiftingAnimationCoroutine()
    {
        yield return null;
        while (animate)
        {
            image.sprite = frames[currentFrameId];
            currentFrameId++;
            currentFrameId %= frameCount;
            yield return new WaitForSeconds(period);
        }
    }
}

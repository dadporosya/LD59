using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using M = System.Math;
public class Bar : MonoBehaviour
{
    [SerializeField] private bool initOnStart = false;
    
    public float currentValue;
    public float currentMaxValue;
    [SerializeField] private bool enableStartFill=false;
    [Range(0f,1f)]
    [SerializeField] private float startFill; 
    
    public enum FillOrigin { left, right }
    public FillOrigin fillOrigin;

    [SerializeField] private Image image;

    [SerializeField] private bool enableGradient = false;
    [SerializeField] private Gradient _gradient;

    [HideInInspector] public float _targetValue;

    [SerializeField] private GameObject indicator;
    [SerializeField] private float sizeIncreasementForFirstAndLast = 1.2f;
    [SerializeField] private Vector3 indicatorOffset;

    [SerializeField] private GameObject tracker; // SpaceShip included
    [SerializeField] private Vector3 trackersOffset;

    private List<GameObject> _spawnedIndicators = new List<GameObject>();
    private BarTracker _spawnedTracker;

    private float targetValue
    {
        get => _targetValue;
        set
        {
            _targetValue = M.Clamp(value, 0, currentMaxValue);
            UpdateBar();
        }
    }
    
    [SerializeField] private float _drainDuration = 0.25f;
    private Coroutine _drainCoroutine;

    public void Init(float targetValueIn, float maxValueIn, float startFillIn=-1, bool enableStartFillIn=false)
    {
        currentMaxValue = maxValueIn;

        if (enableStartFillIn || startFillIn >= 0)
        {
            targetValue = startFillIn * maxValueIn;
        } else targetValue = M.Clamp(targetValueIn, 0, maxValueIn);
        
        image.fillOrigin = fillOrigin == FillOrigin.left
            ? (int)Image.OriginHorizontal.Left
            : (int)Image.OriginHorizontal.Right;
        
        
        
        UpdateBar();
        UpdateTrackerPosition();
    }

    private void Awake()
    {
        if (!image) image = GetComponent<Image>();
    }

    private void Start()
    {
        if (initOnStart) Init(targetValue, currentMaxValue, startFill, enableStartFill);
    }

    public void SetValue(float value)
    {
        targetValue = M.Clamp(value, 0, currentMaxValue);
        UpdateBar();
    }

    public void ChangeValue(float dValue)
    {
        targetValue = M.Clamp(targetValue + dValue, 0, currentMaxValue);
        UpdateBar();
    }

    private void UpdateBar()
    {
        // h.Out(targetValue, currentMaxValue);
        SetValueSmooth(targetValue, _drainDuration);
    }

    public void SetValueSmooth(float target, float duration)
    {
        if (_drainCoroutine != null) StopCoroutine(_drainCoroutine);
        target = Mathf.Clamp(target, 0, currentMaxValue);
        _drainCoroutine = StartCoroutine(SmoothChange(target, duration));
    }

    private IEnumerator SmoothChange(float target, float duration)
    {
        if (_spawnedTracker) _spawnedTracker.StartMove();
        
        void Fill(float value=-1)
        {
            if (value >= 0) currentValue = value;
            
            float fillAmount = currentValue / currentMaxValue;
            image.fillAmount = fillAmount;
            if (enableGradient) image.color = _gradient.Evaluate(fillAmount);
        }
        
        
        
        float start = image.fillAmount * currentMaxValue;
        float time = 0f;
        float fillAmount;
        while (time < duration)
        {
            time += Time.deltaTime;
            currentValue = Mathf.Lerp(start, target, time / duration);

            Fill();
            UpdateTrackerPosition();
            
            yield return null;
        }
        Fill(target);
        UpdateTrackerPosition();
        if (_spawnedTracker) _spawnedTracker.EndMove();
    }

    /// <summary>
    /// Creates n indicators and spreads them evenly across the bar with tracker offset applied.
    /// First indicator at the beginning, last indicator at the end.
    /// </summary>
    /// <param name="count">Number of indicators to spawn</param>
    public void CreateSpreadIndicators(int count)
    {
        if (!indicator || !image)
        {
            Debug.LogError("Indicator prefab or Image component not set!");
            return;
        }

        // Clear existing indicators
        foreach (var ind in _spawnedIndicators)
        {
            Destroy(ind);
        }
        _spawnedIndicators.Clear();

        if (count <= 0) return;

        RectTransform barRect = image.rectTransform;
        float barWidth = barRect.rect.width;
        
        // Calculate spacing: distribute indicators evenly from beginning to end
        float spacing = count > 1 ? barWidth / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            // Spawn indicator as child of bar
            GameObject newIndicator = Instantiate(indicator, barRect);
            _spawnedIndicators.Add(newIndicator);

            // Get RectTransform to position it
            RectTransform indRect = newIndicator.GetComponent<RectTransform>();
            if (indRect == null)
            {
                indRect = newIndicator.AddComponent<RectTransform>();
            }

            // Calculate local position: from -barWidth/2 (beginning) to +barWidth/2 (end)
            float xPos = -barWidth * 0.5f + spacing * i;
            indRect.anchoredPosition = new Vector3(xPos, 0f, 0f) + (Vector3)new Vector2(indicatorOffset.x, indicatorOffset.y);

            // Scale first and last indicators 20% bigger
            if (i == 0 || i == count - 1)
            {
                indRect.localScale *= sizeIncreasementForFirstAndLast;
            }
            if (enableGradient) indRect.GetComponent<Image>().color = _gradient.Evaluate((float)i/count);

            // Disable indicator if it has a renderer to hide the prefab visually
            foreach (var renderer in newIndicator.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }
        
        // TEMP
        if (tracker  /* && !_spawnedTracker */)
        {
            _spawnedTracker = Instantiate(tracker, image.rectTransform).GetComponent<BarTracker>();
        }
    }
    
    private void UpdateTrackerPosition()
    {
        if (!_spawnedTracker) return;

        RectTransform barRect = image.rectTransform;
        float barWidth = barRect.rect.width;
        float fillAmount = image.fillAmount;
        float xPos;

        if (fillOrigin == FillOrigin.left)
        {
            xPos = -barWidth * 0.5f + fillAmount * barWidth;
        }
        else
        {
            xPos = barWidth * 0.5f - fillAmount * barWidth;
        }

        RectTransform trackerRect = _spawnedTracker.GetComponent<RectTransform>();
        trackerRect.anchoredPosition = new Vector3(xPos, 0f, 0f) + trackersOffset;
    }
    
}

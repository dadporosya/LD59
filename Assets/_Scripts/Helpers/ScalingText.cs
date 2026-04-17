using UnityEngine;
using TMPro;

public class ScalingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _textComponent;
    [SerializeField] private RectTransform _textRectTransform;
    [SerializeField] private float _minFontSize = 1f;
    [SerializeField] private float _maxFontSize = 50f;

    private void Awake()
    {
        if (!_textComponent) _textComponent = GetComponent<TMP_Text>();
        if (!_textRectTransform) _textRectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string value)
    {
        _textComponent.text = value;
        ScaleTextToFit();
    }

    protected void OnRectTransformDimensionsChange()
    {
        ScaleTextToFit();
    }

    private void ScaleTextToFit()
    {
        if (!_textComponent || !_textRectTransform || string.IsNullOrEmpty(_textComponent.text)) return;

        Vector2 availableSize = _textRectTransform.rect.size;

        // Binary search to find the optimal font size
        float minSize = _minFontSize;
        float maxSize = _maxFontSize;
        float optimalSize = _minFontSize;

        // First, try the maximum size to see if it fits
        _textComponent.fontSize = maxSize;
        _textComponent.ForceMeshUpdate();
        Vector2 maxSizePreferred = _textComponent.GetPreferredValues();

        if (maxSizePreferred.x <= availableSize.x && maxSizePreferred.y <= availableSize.y)
        {
            // Text fits at max size
            optimalSize = maxSize;
        }
        else
        {
            // Binary search for the right size
            for (int i = 0; i < 10; i++) // 10 iterations should be sufficient
            {
                float midSize = (minSize + maxSize) / 2f;
                _textComponent.fontSize = midSize;
                _textComponent.ForceMeshUpdate();
                Vector2 midSizePreferred = _textComponent.GetPreferredValues();

                if (midSizePreferred.x <= availableSize.x && midSizePreferred.y <= availableSize.y)
                {
                    // Text fits, try larger
                    minSize = midSize;
                    optimalSize = midSize;
                }
                else
                {
                    // Text doesn't fit, try smaller
                    maxSize = midSize;
                }
            }
        }

        _textComponent.fontSize = optimalSize;
        _textComponent.ForceMeshUpdate();
    }
}
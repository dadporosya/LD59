using UnityEngine;

public class Neuron : MonoBehaviour
{
    public LineRenderer lineRendererPrefab;
    [HideInInspector] public LineRenderer lineRenderer;
    
    [Header("Points")]
    public GameObject bottom;
    [HideInInspector]public Transform bottomPoint;
    public GameObject top;
    [HideInInspector] public Transform topPoint;

    [Header("Actions")]
    public Sprite actionIcon;
    // button, activate neuron
    // inst of IAction object

    private void Start()
    {
        if (!bottomPoint)
        {
            bottomPoint = h.GetFirstChildByTag(bottom.transform, "Point");
        }

        if (!topPoint)
        {
            topPoint = h.GetFirstChildByTag(top.transform, "Point");
        }
        
        if (!lineRenderer)
        {
            lineRenderer = Instantiate(lineRendererPrefab, parent:transform);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, bottomPoint.position);
            lineRenderer.SetPosition(1, topPoint.position);
            // TODO dynamic size and scale, so it always suit
        }
    }
    
    
}

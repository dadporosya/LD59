using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public LineRenderer lineRendererPrefab;
    [HideInInspector] public LineRenderer lineRenderer;

    public List<Transform> points;
    
    private void Start()
    {

        if (!lineRenderer)
        {
            lineRenderer = Instantiate(lineRendererPrefab, parent:transform);
            lineRenderer.positionCount = points.Count;
            UpdateLineRenderer();
        }
    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private InputActionReference actionTrigger;
    public SpriteRenderer actionIcon;
    public IAction actionPerformer;
    public Spark sparkPrefab;
    
    private void OnEnable()
    {
        if (actionTrigger != null)
            actionTrigger.action.Enable();
    }

    private void OnDisable()
    {
        if (actionTrigger != null)
            actionTrigger.action.Disable();
    }
    private void Start()
    {
        if (!bottomPoint) bottomPoint = h.GetFirstChildByTag(bottom.transform, "Point");
        if (!topPoint) topPoint = h.GetFirstChildByTag(top.transform, "Point");
        if (actionPerformer != null) actionIcon.sprite = actionPerformer.actionIcon;
        
        if (!lineRenderer)
        {
            lineRenderer = Instantiate(lineRendererPrefab, parent:transform);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, bottomPoint.position);
            lineRenderer.SetPosition(1, topPoint.position);
            // TODO dynamic size and scale, so it always suit
        }
    }

    private void Update()
    {
        if (actionTrigger != null && actionTrigger.action.triggered)
        {
            Triggered();
        }
    }

    public void Triggered()
    {
        SpawnSpark();
        if (actionPerformer != null) actionPerformer.Action();
    }

    private void SpawnSpark()
    {
        if (!sparkPrefab) return;
        Spark saprkGO = Instantiate(sparkPrefab, parent:transform);
        saprkGO.Init(bottomPoint, topPoint);

    }
}

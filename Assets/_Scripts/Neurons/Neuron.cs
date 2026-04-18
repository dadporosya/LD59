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
    public GameObject actionPerformerGO;
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
        if (actionPerformerGO) actionPerformer = actionPerformerGO.GetComponent<IAction>();
        if (actionPerformer != null) 
        {
            actionIcon.sprite = actionPerformer.actionIcon;
            if (actionPerformer.actionIcon != null)
            {
                Sprite sprite = actionPerformer.actionIcon;
                float width = sprite.bounds.size.x;
                float height = sprite.bounds.size.y;
                float maxDimension = Mathf.Max(width, height);
                actionIcon.transform.localScale = new Vector3(1f / maxDimension, 1f / maxDimension, 1f);
            }
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
        
    }

    private void SpawnSpark()
    {
        if (!sparkPrefab) return;
        Spark sparkGO = Instantiate(sparkPrefab, parent:transform);
        sparkGO.Init(bottomPoint, topPoint);
        if (actionPerformer != null)
        {
            sparkGO.onReachedTarget.AddListener(() => actionPerformer.Action());
        }
    }
}

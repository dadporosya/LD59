using System.Collections;
using System.Collections.Generic;
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
    public float signalSpeed = 3f;

    public float cooldown = 0.5f;
    public bool onCooldown = false;
    public float maxSparksCount = 2;
    
    private List<Spark> sparks = new List<Spark>();
    
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
        
        // Clean up destroyed sparks
        
    }

    public void Triggered()
    {
        sparks.RemoveAll(s => s == null);
        if (onCooldown || sparks.Count >= maxSparksCount) return;
        
        onCooldown = true;
        StartCoroutine(ResetCooldown());
        SpawnSpark();
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    private void SpawnSpark()
    {
        if (!sparkPrefab) return;
        Spark spark = Instantiate(sparkPrefab, parent:transform);
        spark.Init(bottomPoint, topPoint, signalSpeed);
        sparks.Add(spark);
        if (actionPerformer != null)
        {
            spark.onReachedTarget.AddListener(() => actionPerformer.Action());
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Eye : OrganBase
{
    public Transform scrollingParent;

    public float flashDuration=5f;
    public float flashVFXDuration = 0.5f;
    public float maxDistance;
    
    [SerializeField] private GameObject sparkPrefab;

    [SerializeField] private LineRenderer linePrefab;

    public override void Awake()
    {
        if (!scrollingParent) scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").GetComponent<Transform>();
        if (maxDistance <= 0) maxDistance = h.GetCameraWidth()*0.95f;
    }

    public override void Action()
    {
        base.Action();
        
        // Find all IBlindable objects in scrolling parent
        IBlindable[] blindableObjects = scrollingParent.GetComponentsInChildren<IBlindable>();
        h.Out(blindableObjects);
        
        if (blindableObjects.Length == 0)
        {
            h.Out("Eye no target found");
            return;
        }
        
        // Find the closest one to this transform
        IBlindable closestTarget = null;
        float closestDistance = float.MaxValue;
        Transform blindSpot=null;
        
        foreach (IBlindable blindable in blindableObjects)
        {
            if (blindable.blinded) continue;
            
            Transform blindableTransform = (blindable as MonoBehaviour).transform;
            float distance = Vector3.Distance(transform.position, blindableTransform.position);
            
            if (distance < closestDistance &&  distance <= maxDistance)
            {
                closestDistance = distance;
                closestTarget = blindable;
                blindSpot =  blindableTransform;
            }
        }
        h.Out(closestTarget);
        
        GameObject temp = h.FindChildrenWithTag(blindSpot, "BlindSpot");
        if (temp) blindSpot = temp.transform;

        if (linePrefab && blindSpot!=null)
        {
            LineRenderer laserLine = Instantiate(linePrefab, parent:transform);
            
            laserLine.positionCount = 2;
            laserLine.SetPosition(0, transform.position);
            laserLine.SetPosition(1, blindSpot.position);

            GameObject spark = Instantiate(sparkPrefab, blindSpot.position, Quaternion.identity, parent:scrollingParent);
            spark.transform.localScale *= 0.5f;
            h.InvokeAfterTime(this, flashVFXDuration, () => { Destroy(laserLine); });
            h.InvokeAfterTime(this, flashVFXDuration, () => { Destroy(spark); });
        }
        
        
        
        if (closestTarget != null) closestTarget.Blind(flashDuration);
    }
}



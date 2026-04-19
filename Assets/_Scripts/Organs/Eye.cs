using System.Collections.Generic;
using UnityEngine;

public class Eye : OrganBase
{
    public Transform scrollingParent;

    public float flashDuration;
    public float maxDistance;

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
        
        foreach (IBlindable blindable in blindableObjects)
        {
            Transform blindableTransform = (blindable as MonoBehaviour).transform;
            float distance = Vector3.Distance(transform.position, blindableTransform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = blindable;
            }
        }
        h.Out(closestTarget);
        // make trail
        if (closestTarget != null) closestTarget.Blind(2f);
    }
}



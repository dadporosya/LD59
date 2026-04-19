using System.Collections.Generic;
using UnityEngine;

public class Eye : OrganBase
{
    public Transform scrollingParent;
    public Transform closestTarget;

    public override void Awake()
    {
        if (!scrollingParent) scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").GetComponent<Transform>();
    }

    public override void Action()
    {
        base.Action();
        
        // Find all IBlindable objects in scrolling parent
        IBlindable[] blindableObjects = scrollingParent.GetComponentsInChildren<IBlindable>();
        
        if (blindableObjects.Length == 0)
        {
            h.Out("Eye no target found");
            return;
        }
        
        // Find the closest one to this transform
        closestTarget = null;
        float closestDistance = float.MaxValue;
        
        foreach (IBlindable blindable in blindableObjects)
        {
            Transform blindableTransform = (blindable as MonoBehaviour).transform;
            float distance = Vector3.Distance(transform.position, blindableTransform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = blindableTransform;
            }
        }
        
        // make trail
        if (closestTarget 
            && closestTarget.TryGetComponent<IOnDestroy>(out IOnDestroy onDestroyComponent))
        {
            onDestroyComponent.OnDestroy();
        }
    }
}



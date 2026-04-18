using System;
using System.Collections;
using UnityEngine;

public class Spark : MonoBehaviour
{
    public Transform target;
    public float speed=1f;
    [SerializeField] private bool scaling = true;
    [SerializeField] private float scalingTime = 0.1f;

    public void Init(Transform posIn, Transform targetIn)
    {
        transform.position = posIn.position;
        this.target = targetIn;
        StartMovement();
    }

    public float GetTimeToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance / speed;
    }

    // private void Start()
    // {
    //     if (target) Init(transform, target);
    //     
    // }

    public void StartMovement()
    {
        StartCoroutine(MoveCoroutine());
    }
    
    private IEnumerator MoveCoroutine()
    {
        float time = GetTimeToTarget();
        float currentTime = 0;
        Vector3 initialScale = transform.localScale;
        bool hasStartedScale = false;

        if (scaling)
        {
            transform.localScale = Vector3.zero;
            h.Out(initialScale);
            h.SmoothScaling(this, transform, initialScale,time*scalingTime);
        }

        
        
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            currentTime += Time.deltaTime;
            if (scaling && currentTime > time * (1-scalingTime) && !hasStartedScale)
            {
                h.SmoothScaling(this, transform, Vector3.zero,time*scalingTime);
                hasStartedScale = true;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
        
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spark : MonoBehaviour
{
    public Transform target;
    public float speed=1f;
    [SerializeField] private bool scaling = true;
    [SerializeField] private float scalingTime = 0.1f;
    
    [SerializeField] public UnityEvent onReachedTarget = new UnityEvent();

    public void Init(Transform posIn, Transform targetIn, float speedIn=-1)
    {
        transform.position = posIn.position;
        this.target = targetIn;
        if (speedIn > 0) speed = speedIn;
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
            h.SmoothScaling(this, transform, initialScale,time*scalingTime);
        }

        
        
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
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
        
        
        onReachedTarget?.Invoke();
        Destroy(gameObject);
    }
}

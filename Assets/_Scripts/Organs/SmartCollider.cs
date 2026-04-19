using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartCollider : MonoBehaviour
{
    public Collider2D collider;
    
    public UnityEvent<GameObject> onTriggerEnter;
    public List<string> targetTags;

    private void Awake()
    {
        if (!collider) collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        h.Out(other.tag);
        if (targetTags.Contains(other.tag))
        {
            onTriggerEnter.Invoke(other.gameObject);
        }
    }
}

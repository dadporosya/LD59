using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PunchCollider : MonoBehaviour
{
    public Collider2D collider;
    
    public UnityEvent<GameObject> onPunch;
    public List<string> targetTags;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        h.Out(other.tag);
        if (targetTags.Contains(other.tag))
        {
            onPunch.Invoke(other.gameObject);
        }
    }
}

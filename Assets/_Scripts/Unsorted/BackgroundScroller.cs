using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform backgroundParent;
    public Queue<Transform> backgroundObjects = new Queue<Transform>();
  public float borderX;
    
    [SerializeField] private bool scrolling = false;
    
    private void Start()
    {
        foreach (Transform backgroundObject in backgroundParent)
        {
            backgroundObjects.Enqueue(backgroundObject);
        }
        h.Out(backgroundObjects.Count);
        borderX = backgroundObjects.Peek().localPosition.x;
    }

    private void FixedUpdate()
    {
        if (scrolling)
        {
            foreach (Transform backgroundObject in backgroundObjects)
            {
                backgroundObject.Translate(new Vector3(-0.2f, 0f, 0f));
            }
        }

        if (backgroundObjects.Count > 0 && backgroundObjects.Peek().position.x < borderX)
        {
            Transform bg = backgroundObjects.Dequeue();
            bg.position = backgroundObjects.Peek().position + new Vector3(backgroundObjects.Peek().GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
            backgroundObjects.Enqueue(bg);
        }
    }
}
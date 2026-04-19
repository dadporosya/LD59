using System;
using UnityEngine;

public class PoliceEnemy : MonoBehaviour
{
    private Collider2D collider2D;
    
    
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }
}

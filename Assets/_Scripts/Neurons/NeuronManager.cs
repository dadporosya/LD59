using System;
using UnityEngine;
using UnityEngine.Events;

public class NeuronManager : MonoBehaviour
{
    public Transform scrollingParent;

    private void Start()
    {
        scrollingParent = GameObject.FindGameObjectWithTag("ScrollingParent").transform;
    }

    public void OnNeuronActivation(OrganBase organ)
    {
        if (!organ) return;
        
        // Find all BarrierEnemy in scrollingParent
        BarrierEnemy[] barriers = scrollingParent.GetComponentsInChildren<BarrierEnemy>();
        foreach (BarrierEnemy barrier in barriers)
        {
            
        }
    }
    
}

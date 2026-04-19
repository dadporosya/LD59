using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    // TODO потемнение экрана чем больше урона
    public float damageTaken=0;
    public float recoverySpeed=0.01f;
    private Coroutine BPMReductionCoroutineInst;
    [HideInInspector] public BPMManager bpmManager;

    public SmartCollider playerSmartCollider;
    
    public TextMeshProUGUI tempDamageLabel;
    
    private void Awake()
    {
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        if (!playerSmartCollider) FindPlayerCollider();
        playerSmartCollider.collider.enabled = false;
    }

    public void FindPlayerCollider()
    {
        playerSmartCollider = GameObject.Find("PlayerPunchCollider").GetComponent<SmartCollider>();
    }

    

    public void TakeDamage(float value)
    {
        damageTaken += value;
        if (BPMReductionCoroutineInst == null)
        {
            BPMReductionCoroutineInst = StartCoroutine(BPMReductionCoroutine());
        }
    }

    private IEnumerator BPMReductionCoroutine()
    {
        while (damageTaken > 0)
        {
            damageTaken -= recoverySpeed * Time.deltaTime;

            bpmManager.currentBPMReduction = h.Max(
                bpmManager.baseBPMReduction + damageTaken, bpmManager.baseBPMReduction
                );

            tempDamageLabel.text = bpmManager.currentBPMReduction.ToString();
            
            yield return null;
        }

        bpmManager.currentBPMReduction = bpmManager.baseBPMReduction;
        yield return null;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    // TODO потемнение экрана чем больше урона
    public float damageTaken=0;
    public float recoverySpeed=0.1f;
    private Coroutine BPMReductionCoroutineInst;
    [HideInInspector] public BPMManager bpmManager;

    public PunchCollider playerPunchCollider;

    private void Awake()
    {
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        if (!playerPunchCollider) playerPunchCollider = FindFirstObjectByType<PunchCollider>();
        playerPunchCollider.collider.enabled = false;
    }

    public void TakeDamage(int value)
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

            bpmManager.currentBPMReduction = h.Min(
                bpmManager.baseBPMReduction + damageTaken, bpmManager.baseBPMReduction
                );
            
            yield return null;
        }

        bpmManager.currentBPMReduction = bpmManager.baseBPMReduction;
        yield return null;
    }

}

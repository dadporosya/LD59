using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageManager : MonoBehaviour
{
    public float damageTaken=0;
    public float damageThreshold = 5f;
    public Image damageScreenImage;
    public float recoverySpeed=0.5f; // TODO
    private Coroutine BPMReductionCoroutineInst;
    [HideInInspector] public BPMManager bpmManager;

    public SmartCollider playerSmartCollider;
    
    public TextMeshProUGUI tempDamageLabel;
    
    private void Awake()
    {
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        if (!playerSmartCollider) FindPlayerCollider();
        playerSmartCollider.collider.enabled = false;
        if (!damageScreenImage) damageScreenImage = GameObject.Find("DamageScreen").GetComponent<Image>();
    }

    public void FindPlayerCollider()
    {
        playerSmartCollider = GameObject.Find("PlayerPunchCollider").GetComponent<SmartCollider>();
    }

    

    public void TakeDamage(float value)
    {
        damageTaken += value;
        UpdateDamageScreen();
        if (BPMReductionCoroutineInst == null)
        {
            BPMReductionCoroutineInst = StartCoroutine(BPMReductionCoroutine());
        }
    }

    public void UpdateDamageScreen()
    {
        if (!damageScreenImage) return;
        
        if (!damageScreenImage) return;
        
        // float alpha = Mathf.Clamp01(damageTaken / damageThreshold);
        float alpha = h.Min(damageTaken / damageThreshold, 0.97f);
        h.Out(damageTaken, damageThreshold, damageTaken/damageThreshold, alpha);

        Color damageColor = damageScreenImage.color;
        damageColor.a = alpha;
        damageScreenImage.color = damageColor;
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
            
            UpdateDamageScreen();
            
            yield return null;
        }

        bpmManager.currentBPMReduction = bpmManager.baseBPMReduction;
        yield return null;
    }

}

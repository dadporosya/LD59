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
    public int punchStack=0;
    
    public TextMeshProUGUI tempDamageLabel;

    public Transform aquariumParent;
    
    private GameFlowManager gameFlowManager;
    
    public void Init()
    {
        damageTaken = 0;
        punchStack = 0;



        if (BPMReductionCoroutineInst != null)
        {
            
            StopCoroutine(BPMReductionCoroutineInst);
        }
        BPMReductionCoroutineInst = null;
        h.Out(BPMReductionCoroutineInst, "RESSEEEEEEEEEEEEEEEEET", BPMReductionCoroutineInst);
        
        Awake();
    }
    
    private void Awake()
    {
        if (BPMReductionCoroutineInst != null)  StopCoroutine(BPMReductionCoroutineInst);
        BPMReductionCoroutineInst = null;
        
        if (!gameFlowManager) gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        
        if (!aquariumParent) aquariumParent = GameObject.FindGameObjectWithTag("AquariumParent").transform;
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        if (!playerSmartCollider) FindPlayerCollider();
        playerSmartCollider.collider.enabled = false;
        if (playerSmartCollider.targetTags.Contains("Enemy") == false) playerSmartCollider.targetTags.Add("Enemy");
        if (!damageScreenImage) damageScreenImage = GameObject.Find("DamageScreen").GetComponent<Image>();
    }

    public void FindPlayerCollider()
    {
        playerSmartCollider = GameObject.Find("PlayerPunchCollider").GetComponent<SmartCollider>();
    }

    

    public void TakeDamage(float value)
    {
        if (gameFlowManager.IsPaused()) return;
        
        h.Out("-----------------TAKEDAMAG-----------------");
        damageTaken += value;
        
        UpdateDamageScreen();
        h.Out(BPMReductionCoroutineInst);
        if (BPMReductionCoroutineInst == null || damageTaken == value)
        {
            BPMReductionCoroutineInst = StartCoroutine(BPMReductionCoroutine());
        }
    }

    public void UpdateDamageScreen()
    {
        if (!damageScreenImage) return;
        
        if (!damageScreenImage) return;
        
        // float alpha = Mathf.Clamp01(damageTaken / damageThreshold);
        h.Out("-----------------updateblood-----------------");
        float alpha = h.Min(damageTaken / damageThreshold, 0.97f);
        // h.Out(damageTaken, damageThreshold, damageTaken/damageThreshold, alpha);

        Color damageColor = damageScreenImage.color;
        damageColor.a = alpha;
        damageScreenImage.color = damageColor;
    }

    private IEnumerator BPMReductionCoroutine()
    {
        while (damageTaken > 0)
        {
            if (gameFlowManager.IsPaused())
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            
            damageTaken -= recoverySpeed * Time.deltaTime;

            bpmManager.currentBPMReduction = h.Max(
                bpmManager.baseBPMReduction + damageTaken, bpmManager.baseBPMReduction
                );

            tempDamageLabel.text = bpmManager.currentBPMReduction.ToString();
            
            UpdateDamageScreen();
            
            yield return null;
        }
        
        h.Out("VIJTI IZ CIKLA", BPMReductionCoroutineInst);
        // h.Out("");
        bpmManager.currentBPMReduction = bpmManager.baseBPMReduction;
        if (BPMReductionCoroutineInst!=null)
        {
            h.Out("s");
            StopCoroutine(BPMReductionCoroutineInst);
            h.Out("after stop", BPMReductionCoroutineInst);
        }
        BPMReductionCoroutineInst = null;
        h.Out("after stop", BPMReductionCoroutineInst);
        
        yield return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : OrganBase
{
    public float damage = 1;
    
    //dmg man
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerDamageManager playerDamageManager;
    [SerializeField] private SmartCollider smartCollider;
    
    public GameObject aquariumParent;

    public override void Awake()
    {
        if (!aquariumParent) aquariumParent = GameObject.FindWithTag("AquariumParent");
        
        base.Awake();
        
        playerDamageManager = FindFirstObjectByType<PlayerDamageManager>();
        if (playerDamageManager.playerSmartCollider == null)
        {
            playerDamageManager.FindPlayerCollider();
        }
        smartCollider = Instantiate(
            playerDamageManager.playerSmartCollider,
            playerDamageManager.playerSmartCollider.transform.position,
            Quaternion.identity,
            transform
            );
        
        smartCollider.collider.enabled = false;
        smartCollider.targetTags.Add("Enemy");
        smartCollider.onTriggerEnter.AddListener(Punch);
        
        locationInAquarium = LocationInAquarium.OutIn; // ?
        
        if (!animator)
            animator = GetComponent<Animator>();
        
        
    }

    public override void Action()
    {
        base.Action();
        
        if (!animator) return;

        // Start animation only
        animator.Play("ArmPunchStart");
    }

    // This will be called from Animation Event
    public void StartPunch()
    {
        StartCoroutine(StartPunchCoroutine());

    }

    public IEnumerator StartPunchCoroutine()
    {
        // smartCollider.collider.enabled = true;
        
        playerDamageManager.playerSmartCollider.onTriggerEnter.AddListener(Punch);
        playerDamageManager.playerSmartCollider.collider.enabled = true;
        playerDamageManager.punchStack++;
        h.Out(playerDamageManager.playerSmartCollider.collider.enabled);
        yield return new WaitForSeconds(0.01f);
        EndPunch();
        yield return null;
    }

    public void EndPunch()
    {
        // smartCollider.collider.enabled = false;
        playerDamageManager.playerSmartCollider.onTriggerEnter.RemoveListener(Punch);
        playerDamageManager.playerSmartCollider.collider.enabled = false;
        
        playerDamageManager.punchStack--;
        if (playerDamageManager.punchStack > 0)
        {
            playerDamageManager.playerSmartCollider.collider.enabled = true;
        }
    }

    public void Punch(GameObject target)
    {
        SFXManager.Instance.PlayRandomClip(new List<string>()
        {
            "Audio/SFX/punch1", 
            "Audio/SFX/punch2", 
            "Audio/SFX/punch3", 
        });
        
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy == null) return;

        h.ShakeOnce(1, 10, 0, 0.2f);
        enemy.TakeDamage((int)damage);
    }

    public void OnIdle()
    {
        if (!aquariumParent || !animator) return;
        
        Animator otherAnimator = aquariumParent.GetComponent<Animator>();

        float normalizedTime =
            otherAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

        animator.CrossFade(
            "ArmIdle",
            0f,
            0,
            normalizedTime
        );
    }
}
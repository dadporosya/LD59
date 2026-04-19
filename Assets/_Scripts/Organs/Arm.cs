using UnityEngine;

public class Arm : OrganBase
{
    public float damage = 1;
    
    //dmg man
    private Animator animator;
    private PlayerDamageManager playerDamageManager;
    private PunchCollider punchCollider;
    
    public GameObject aquariumParent;

    public override void Awake()
    {
        base.Awake();
        
        playerDamageManager = FindFirstObjectByType<PlayerDamageManager>();
        punchCollider = Instantiate(
            playerDamageManager.playerPunchCollider,
            playerDamageManager.playerPunchCollider.transform.position,
            Quaternion.identity,
            transform
            );
        punchCollider.collider.enabled = false;
        punchCollider.targetTags.Add("Enemy");
        punchCollider.onPunch.AddListener(Punch);
        
        locationInAquarium = LocationInAquarium.OutIn; // ?
        
        if (!animator)
            animator = GetComponent<Animator>();
        
        if (!aquariumParent) aquariumParent = GameObject.FindWithTag("AquariumParent");
    }

    public override void Action()
    {
        base.Action();

        // Start animation only
        animator.Play("ArmPunchStart");
    }

    // This will be called from Animation Event
    public void StartPunch()
    {
        punchCollider.collider.enabled = true;
    }

    public void EndPunch()
    {
        punchCollider.collider.enabled = false;
    }

    public void Punch(GameObject target)
    {
        EnemyBase enemy = target.GetComponent<EnemyBase>();
        if (enemy == null) return;
        
        enemy.TakeDamage((int)damage);
    }

    public void OnIdle()
    {
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
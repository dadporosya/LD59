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
        smartCollider.collider.enabled = true;
    }

    public void EndPunch()
    {
        smartCollider.collider.enabled = false;
    }

    public void Punch(GameObject target)
    {
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
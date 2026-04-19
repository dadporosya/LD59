using UnityEngine;

public class Arm : OrganBase
{
    public float damage = 5;
    
    //dmg man
    private Animator animator;
    
    public GameObject aquariumParent;

    public override void Awake()
    {
        base.Awake();
        
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
    public void Punch()
    {
        // damage

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
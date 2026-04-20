using UnityEngine;

public class Leg : OrganBase
{
    public float XPPerStep = 5;
    
    private EscapeProgressManager escapeProgressManager;
    private ScrollManager scrollManager;
    private Animator animator;
    
    public GameObject aquariumParent;

    

    public override void Awake()
    {
        base.Awake();

        if (XPPerStep <= 0) XPPerStep = Preferences.defaultLegSpeed;
        
        locationInAquarium = LocationInAquarium.Out;
        
        if (!escapeProgressManager)
            escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();

        if (!animator)
            animator = GetComponent<Animator>();
        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (!scrollManager)
            scrollManager = FindFirstObjectByType<ScrollManager>();
        
        if (!aquariumParent) aquariumParent = GameObject.FindWithTag("AquariumParent");
    }

    public override void Action()
    {
        base.Action();

        

        // Start animation only
        animator.Play("LegStep1");
    }

    // This will be called from Animation Event
    public void OnLegStep2()
    {
        float distance = XPPerStep * Preferences.distancePerXP;
        float currentAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = distance / currentAnimationLength;
        
        // h.Out(speed, "speed");
        escapeProgressManager.initialTempXp = h.Min(escapeProgressManager.initialTempXp, escapeProgressManager.currentXP);
        escapeProgressManager.onChangeTempXP += XPPerStep;
        
        escapeProgressManager.ChangeXP(XPPerStep, currentAnimationLength);
        
        h.Out(escapeProgressManager.initialTempXp, escapeProgressManager.currentXP);
        
        scrollManager.Scroll(distance, speed);
        
        h.Out(scrollManager.currentSpeed);
    }

    public void OnIdle()
    {
        Animator otherAnimator = aquariumParent.GetComponent<Animator>();

        float normalizedTime =
            otherAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

        animator.CrossFade(
            "LegIdle",
            0f,
            0,
            normalizedTime
        );
    }
}
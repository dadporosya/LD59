using UnityEngine;

public class Leg : OrganBase
{
    public float XPPerStep = 5;
    
    
    
    private EscapeProgressManager escapeProgressManager;
    private ScrollManager scrollManager;
    private Animator animator;

    public override void Awake()
    {
        base.Awake();

        if (!escapeProgressManager)
            escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();

        if (!animator)
            animator = GetComponent<Animator>();

        if (!scrollManager)
            scrollManager = FindFirstObjectByType<ScrollManager>();
    }

    public override void Action()
    {
        base.Action();

        escapeProgressManager.ChangeXP(XPPerStep);

        // Start animation only
        animator.Play("LegStep1");
    }

    // This will be called from Animation Event
    public void OnLegStep2()
    {
        // h.Out(XPPerStep, Preferences.distancePerXP);
        float distance = XPPerStep * Preferences.distancePerXP;
        float currentAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float speed = distance / currentAnimationLength;
        
        h.Out(distance, currentAnimationLength, speed);
        
        scrollManager.Scroll(distance, speed);
    }
}
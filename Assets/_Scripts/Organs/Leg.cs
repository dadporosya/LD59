using UnityEngine;

public class Leg : OrganBase
{
    public int XPPerStep = 5;
    private EscapeProgressManager escapeProgressManager;
    
    private ScrollManager scrollManager;
    
    private Animator animator;
    
    public override void Awake()
    {
        base.Awake();
        if (!escapeProgressManager) escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
        if (!animator) animator = GetComponent<Animator>();
        if (!scrollManager) scrollManager = FindFirstObjectByType<ScrollManager>();
    }

    public override void Action()
    {
        base.Action();
        escapeProgressManager.ChangeXP(XPPerStep);
        animator.Play("LegStep");
        
        scrollManager.Scroll(2, 2);
    }
    
}

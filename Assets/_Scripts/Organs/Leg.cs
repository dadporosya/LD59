using UnityEngine;

public class Leg : OrganBase
{
    public int XPPerStep = 5;
    private EscapeProgressManager escapeProgressManager;
    private Animator animator;
    
    public override void Awake()
    {
        base.Awake();
        if (!escapeProgressManager) escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>();
        if (!animator) animator = GetComponent<Animator>();
    }

    public override void Action()
    {
        base.Action();
        escapeProgressManager.ChangeXP(XPPerStep);
        animator.Play("LegStep");
    }
    
}

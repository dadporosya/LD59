using UnityEngine;

public class Leg : OrganBase
{
    public int XPPerStep = 5;
    private EscapeProgressManager escapeProgressManager;
    
    public override void Awake()
    {
        base.Awake();
        if (!escapeProgressManager) escapeProgressManager = FindFirstObjectByType<EscapeProgressManager>()
    }

    public override void Action()
    {
        escapeProgressManager.ChangeXP(XPPerStep);
    }
    
}

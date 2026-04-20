using UnityEngine;

public class BarTracker : MonoBehaviour, IOnMove
{
    public Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        EndMove();
    }
    
    public void StartMove()
    {
        animator.Play("LegTrackerOnMove");
    }

    public void OnMove(){}

    public void EndMove()
    {
        CrossFade("LegTrackerIdle", 0.1f);
    }
    
    public void CrossFade(string animationStateName, float duration)
    {
        animator.CrossFade(animationStateName, duration, 0);
    }
    
    
}

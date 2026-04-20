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
        // start jump
    }

    public void OnMove(){}

    public void EndMove()
    {
        //etner idle
    }
}

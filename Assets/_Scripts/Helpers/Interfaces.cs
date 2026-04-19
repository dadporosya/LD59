using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using M=System.MathF;

public interface IInteractable
{
    public void OnInteract() {  }
    public void StartInteraction(GameObject target=null) {  }
    public void EndInteraction(GameObject target=null) {  }
    public void ContinuousInteraction(GameObject target=null) {  }
}

public interface IAction
{
    public Sprite actionIcon { get; set; }
    public void Action(){}
}

public interface IOnMove
{
    public void StartMove();
    public void OnMove();
    public void EndMove();
}

public interface IOnDestroy
{
    public void OnDestroy();
    public IEnumerator BeforeDestroyCoroutine()
    {
        yield return null;
    }
}

public interface IBlindable
{
    public void Blind()
    {
        
    }
}
using UnityEngine;
using UnityEngine.Events;

public class FunctionWrapper : MonoBehaviour
{
    public UnityEvent action1;
    public UnityEvent action2;

    public void Invoke1()
    {
        action1?.Invoke();
    }
    
    public void Invoke2()
    {
        action2?.Invoke();
    }
}

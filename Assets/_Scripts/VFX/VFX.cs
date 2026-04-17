using UnityEngine;

public class VFX : MonoBehaviour
{
    public GameObject parent;

    protected virtual void Start()
    {
        Init(parent.transform);
    }

    public void Init(Transform newParent)
    {
        transform.SetParent(newParent);
        parent = newParent != null ? newParent.gameObject : null;
    }
}

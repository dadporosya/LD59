using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/NewScriptableObjectScript")]
public class PurchaseItemBase : ScriptableObject
{
    public Sprite sprite;
    public string itemName;
    public string itemDescription;

    public virtual void Assign()
    {
        
    }

    public virtual void Revert()
    {
        
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Scriptable Objects/PurchaseItemBase")]
public class PurchaseItemBase : ScriptableObject
{
    public Sprite sprite;
    public string itemName;
    [TextArea(3, 7)]
    public string itemDescription;

    public virtual void Apply()
    {
        
    }

    public virtual void Revert()
    {
        
    }
}

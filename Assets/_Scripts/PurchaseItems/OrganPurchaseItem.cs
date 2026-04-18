using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OrganItem", menuName = "Scriptable Objects/OrganItem")]
public class OrganPurchaseItem : PurchaseItemBase
{
    public OrganBase content;

    private void OnEnable()
    {
        if (!sprite && content)
        {
            sprite = content.GetComponent<SpriteRenderer>().sprite;
        }
    }
}

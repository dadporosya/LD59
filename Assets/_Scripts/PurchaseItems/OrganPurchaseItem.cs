using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OrganItem", menuName = "Scriptable Objects/OrganPurchaseItem",  order = 1)]
public class OrganPurchaseItem : PurchaseItemBase
{
    public OrganBase content;

    private void OnEnable()
    {
        if (!sprite && content)
        {
            sprite = content.GetComponent<SpriteRenderer>().sprite;
        }

        if (itemName == default)
        {
            itemName = content.name;
        }
    }
}

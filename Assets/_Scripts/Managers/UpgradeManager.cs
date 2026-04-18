using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<OrganUpgrade> organUpgrades;
    public List<OrganPurchaseItem> organPurchaseItems;
    public List<OrganBase> organs;

    public Transform organsOutParent;
    public Transform organsInParent;

    public void AddOrgan(OrganBase organPrefab)
    {
        OrganBase organ = Instantiate(organPrefab);
        organs.Add(organ);
        if (organ.IsIn())
        {
            organ.transform.SetParent(organsInParent);
        }
        else
        {
            organ.transform.SetParent(organsOutParent);
        }
    }
}

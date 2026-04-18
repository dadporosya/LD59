using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public GameObject shopWindow;
    [SerializeField] private GameObject purchaseItemWindowPrefab;
    private List<GameObject> currentPurchaseItems = new List<GameObject>();

    public ObjectsKindsContainer<OrganPurchaseItem> organsContainer;
    public ObjectsKindsContainer<UpgradePurchaseItem> upgradesContainer;
    
    private UpgradeManager upgradeManager;

    private void Start()
    {
        if (!upgradeManager) upgradeManager = FindFirstObjectByType<UpgradeManager>();
    }

    public PurchaseItemBase GetRandomItem()
    {
        List<PurchaseItemBase> choices = new List<PurchaseItemBase>();

        if (upgradeManager.organs.Count < Preferences.maxOrganCount)
        {
            choices.AddRange(organsContainer.objects.Values);
        }
        
        // Add upgrades where organ prefab is present in upgradeManager.organs
        foreach (UpgradePurchaseItem upgrade in upgradesContainer.objects.Values)
        {
            if (upgrade.content.organPrefab != null && upgradeManager.organs.Contains(upgrade.content.organPrefab))
            {
                choices.Add(upgrade);
            }
        }
        
        return h.RandChoice(choices);
    }

    public void ApplyItem(PurchaseItemBase item)
    {
        
    }
}

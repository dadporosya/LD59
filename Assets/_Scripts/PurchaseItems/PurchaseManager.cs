using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public GameObject shopWindow;
    [SerializeField] private PurchaseItemUIWindow purchaseItemWindowPrefab;
    private List<PurchaseItemUIWindow> currentPurchaseItems = new List<PurchaseItemUIWindow>();

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

    public void OpenShopWindow(int itemCount=3)
    {
        shopWindow.SetActive(true);
        for (int i = 0; i < itemCount; i++)
        {
            PurchaseItemUIWindow window = Instantiate(purchaseItemWindowPrefab, parent:shopWindow.transform);
            if (!window.content) window.Init();
            currentPurchaseItems.Add(window);
        }

    }

    public void CloseShopWindow()
    {
        shopWindow.SetActive(false);
        currentPurchaseItems.Clear();
    }
    
    public void BuyItem(PurchaseItemBase item)
    {
        
    }
    
    public void BuyItem(OrganPurchaseItem item)
    {
        upgradeManager
    }
}

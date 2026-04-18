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
    
    private List<PurchaseItemBase> choicesForRandomItem = new List<PurchaseItemBase>();

    private void Start()
    {
        if (!upgradeManager) upgradeManager = FindFirstObjectByType<UpgradeManager>();
    }

    public void generateChoices()
    {
        h.Out(upgradeManager.organs.Count);
        if (upgradeManager.organs.Count < Preferences.maxOrganCount)
        {
            choicesForRandomItem.AddRange(organsContainer.objects.Values);
        }
        
        // Add upgrades where organ prefab is present in upgradeManager.organs
        foreach (UpgradePurchaseItem upgrade in upgradesContainer.objects.Values)
        {
            if (upgrade.content.organPrefab != null && upgradeManager.organs.Contains(upgrade.content.organPrefab))
            {
                choicesForRandomItem.Add(upgrade);
            }
        }
    }
    public PurchaseItemBase GetRandomItem()
    {
        if (choicesForRandomItem == null || choicesForRandomItem.Count == 0) generateChoices();
        h.Out(choicesForRandomItem);
        
        return h.RandChoice(choicesForRandomItem);
    }

    public void OpenShopWindow(int itemCount=3)
    {
        shopWindow.SetActive(true);
        generateChoices();
        h.Out(choicesForRandomItem);
        
        // TODO spacing and gap and accelerate + optimize
        RectTransform shopWindowRect = shopWindow.GetComponent<RectTransform>();
        RectTransform itemRect = purchaseItemWindowPrefab.GetComponent<RectTransform>();
        
        float gap = 0.8f * (shopWindowRect.rect.width - itemRect.rect.width * itemCount) / itemCount;
        
        for (int i = 0; i < itemCount; i++)
        {
            PurchaseItemUIWindow window = Instantiate(purchaseItemWindowPrefab, parent:shopWindow.transform);
            if (!window.content) window.Init(this);
            
            RectTransform windowRect = window.GetComponent<RectTransform>();
            windowRect.anchoredPosition = new Vector2(gap / 2 + i * (itemRect.rect.width + gap), 0);
            
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
        if (item is OrganPurchaseItem organItem)
        {
            BuyItem(organItem);
        }
        else if (item is UpgradePurchaseItem upgradeItem)
        {
            upgradeManager.AddOrgan(upgradeItem.content.organPrefab);
        }
        
        CloseShopWindow();
    }
    
    public void BuyItem(OrganPurchaseItem item)
    {
        upgradeManager.AddOrgan(item.content);
    }
}

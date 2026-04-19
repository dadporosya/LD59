using System;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public GameObject shopWindow;
    [SerializeField] private PurchaseItemUIWindow purchaseItemWindowPrefab;
    private List<PurchaseItemUIWindow> currentPurchaseItems = new List<PurchaseItemUIWindow>();

    public ObjectsKindsContainer organsContainer;
    public ObjectsKindsContainer upgradesContainer;
    
    private UpgradeManager upgradeManager;
    
    private List<PurchaseItemBase> choicesForRandomItem = new List<PurchaseItemBase>();

    private void Start()
    {
        if (!upgradeManager) upgradeManager = FindFirstObjectByType<UpgradeManager>();
    }

    public void GenerateChoices()
    {
        choicesForRandomItem.Clear();

        // Add organs only if under the cap and player doesn't already own them
        if (upgradeManager.organs.Count < Preferences.maxOrganCount)
        {
            foreach (PurchaseItemBase organ in organsContainer.objects.Values)
            {
                if (organ is OrganPurchaseItem organItem && 
                    !upgradeManager.organs.Contains(organItem.content))
                {
                    choicesForRandomItem.Add(organItem);
                }
            }
        }

        // Add upgrades only for organs the player currently owns
        foreach (PurchaseItemBase upgrade in upgradesContainer.objects.Values)
        {
            if (upgrade is UpgradePurchaseItem upgradeItem &&
                upgradeItem.content.organPrefab != null &&
                upgradeManager.organs.Contains(upgradeItem.content.organPrefab))
            {
                choicesForRandomItem.Add(upgradeItem);
            }
        }
    }
    public PurchaseItemBase GetRandomItem()
    {
        if (choicesForRandomItem == null || choicesForRandomItem.Count == 0) GenerateChoices();
        h.Out(choicesForRandomItem);
        
        return h.RandChoice(choicesForRandomItem);
    }

    public void OpenShopWindow(int itemCount=3)
    {
        shopWindow.SetActive(true);
        GenerateChoices();
        h.Out(choicesForRandomItem);
        
        // TODO spacing and gap and accelerate + optimize
        RectTransform shopWindowRect = shopWindow.GetComponent<RectTransform>();
        RectTransform itemRect = purchaseItemWindowPrefab.GetComponent<RectTransform>();
        
        float gap = 0.8f * (shopWindowRect.rect.width - itemRect.rect.width * itemCount) / (itemCount+2);
        
        float itemWidth = itemRect.rect.width;
        float startX = -((itemCount - 1) * (itemWidth + gap)) / 2;
        
        for (int i = 0; i < itemCount; i++)
        {
            PurchaseItemUIWindow window = Instantiate(purchaseItemWindowPrefab, parent:shopWindow.transform);
            if (!window.content) window.Init(this);
            
            RectTransform windowRect = window.GetComponent<RectTransform>();
            windowRect.anchoredPosition = new Vector2(startX + i * (itemWidth + gap), 0);
            
            
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

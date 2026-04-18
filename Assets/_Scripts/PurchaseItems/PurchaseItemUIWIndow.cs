using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseItemUIWindow : MonoBehaviour
{
    [SerializeField] private Image purchaseItemIcon;
    [SerializeField] private ScalingText titleText;
    [SerializeField] private ScalingText descriptionText;
    
    private Button purchaseButton;

    private PurchaseManager purchaseManager;
    
    public PurchaseItemBase content;

    private void Start()
    {
        if (!purchaseManager) purchaseManager = FindFirstObjectByType<PurchaseManager>();
        if (!content)
        {
            content = purchaseManager.GetRandomItem();
        }
        
        if (!content) return;
        purchaseItemIcon.sprite = content.sprite;
        titleText.SetText(content.name);
        descriptionText.SetText(content.itemDescription);
        
        
    }
    
}

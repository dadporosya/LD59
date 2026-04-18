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

    public void Init(PurchaseItemBase contentIn=null)
    {
        if (!contentIn)
        {
            h.Out(purchaseManager);
            content = purchaseManager.GetRandomItem();
        }
        else content = contentIn;

        // content = contentIn;
        
        if (!content) return;
        purchaseItemIcon.sprite = content.sprite;
        titleText.SetText(content.itemName);
        descriptionText.SetText(content.itemDescription);
    }
    private void Start()
    {
        if (!purchaseManager) purchaseManager = FindFirstObjectByType<PurchaseManager>();
        
        Init(content);
        
        if (!purchaseButton) purchaseButton = GetComponent<Button>();
        
        if (!content) return;
        if (purchaseButton) purchaseButton.onClick.AddListener(() =>
        {
            purchaseManager.BuyItem(content);
        });
    }
    
    
    
}

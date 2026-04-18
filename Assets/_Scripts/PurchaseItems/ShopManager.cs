using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopWindow;
    [SerializeField] private GameObject purchaseItemPrefab;
    private List<GameObject> currentPurchaseItems = new List<GameObject>();
}


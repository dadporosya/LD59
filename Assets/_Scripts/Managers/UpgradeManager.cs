using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<OrganUpgrade> organUpgrades=new List<OrganUpgrade>();
    public List<OrganPurchaseItem> organPurchaseItems=new List<OrganPurchaseItem>();
    public List<OrganBase> organs=new List<OrganBase>();

    public Transform organsOutParent;
    public Transform organsInParent;

    public Transform neuronParent;
    public List<Neuron> neurons=new List<Neuron>();
    public Neuron neuronPrefab;

    private void Start()
    {
        if (neuronParent)
        {
            foreach (Transform neuron in neuronParent)
            {
                if (neuron.TryGetComponent<Neuron>(out Neuron neuronComponent))
                {
                    neurons.Add(neuronComponent);
                }
            }
        }
    }

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
        
        Neuron neuron = Instantiate(neuronPrefab, neuronParent.position, Quaternion.identity, neuronParent);
        neuron.Init(organ.gameObject);
        neurons.Add(neuron);
        //TODO : randomize pos of neuron
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<OrganBase> startOrgans  = new List<OrganBase>();
    
    public List<OrganUpgrade> organUpgrades=new List<OrganUpgrade>();
    public List<OrganPurchaseItem> organPurchaseItems=new List<OrganPurchaseItem>();
    public List<OrganBase> organs=new List<OrganBase>();

    public Transform organsOutParent;
    public Transform organsInParent;

    public Transform neuronParent;
    public List<Neuron> neurons=new List<Neuron>();
    public Neuron neuronPrefab;
    public Transform organInSpawnPoint;
    
    [SerializeField] private OrganBase armPrefab;
    [SerializeField] private OrganBase legPrefab;

    public GameObject neuronBG;
    public int maxNeuronCount = 10;
    public Vector3 neuronOffset = new Vector3(0,0,0);
    
    // public 
    public List<Transform> armSpawnPoints = new List<Transform>();
    public List<Transform> legSpawnPoints = new List<Transform>();


    public void Init()
    {
        armPrefab = null;
        legPrefab = null;

        ClearNeuronsAndOrgans();
        
        neurons.Clear();
        organs.Clear();
        
        armSpawnPoints.Clear();
        legSpawnPoints.Clear();

        armSpawnPoints.AddRange(h.FindAllTransformsWithTag("ArmSpawnPointIn"));
        armSpawnPoints.AddRange(h.FindAllTransformsWithTag("ArmSpawnPointOut"));
        legSpawnPoints.AddRange(h.FindAllTransformsWithTag("LegSpawnPointIn"));
        legSpawnPoints.AddRange(h.FindAllTransformsWithTag("LegSpawnPointOut"));

    }
    
    private void Start()
    {


        ClearNeuronsAndOrgans();
        
        neurons.Clear();
        organs.Clear();
        
        armSpawnPoints.Clear();
        legSpawnPoints.Clear();
        
        if (!neuronBG) neuronBG = GameObject.Find("NeuronBG");
        if (neuronParent)
        {
            foreach (Transform neuron in neuronParent)
            {
                if (neuron.TryGetComponent<Neuron>(out Neuron neuronComponent))
                {
                    h.Out(neuron, "neuron");
                    neurons.Add(neuronComponent);
                }
            }
        }
        
        // Clear first to avoid duplicates if Start is called again
        armSpawnPoints.Clear();
        legSpawnPoints.Clear();

        armSpawnPoints.AddRange(h.FindAllTransformsWithTag("ArmSpawnPointIn"));
        armSpawnPoints.AddRange(h.FindAllTransformsWithTag("ArmSpawnPointOut"));
        legSpawnPoints.AddRange(h.FindAllTransformsWithTag("LegSpawnPointIn"));
        legSpawnPoints.AddRange(h.FindAllTransformsWithTag("LegSpawnPointOut"));
        
        if (armPrefab)
        {
            for (int i = 0; i < 4+1; i++)
            {
                AddOrgan(armPrefab);
            }
        }
        
        if (legPrefab)
        {
            for (int i = 0; i < 4+1; i++)
            {
                AddOrgan(legPrefab);
            }
        }
        
        foreach (var organ in startOrgans)
        {
            AddOrgan(organ);
        }
        
    }

    public void AddOrgan(OrganBase organPrefab)
    {
        OrganBase organ = Instantiate(organPrefab);
        organs.Add(organ);
        if (organ.IsIn())
        {
            organ.transform.SetParent(organsInParent);
            organ.transform.position = organInSpawnPoint.position;
        }
        else
        {
            organ.transform.SetParent(organsOutParent);
        }

        void SetSpawnPoint(OrganBase organ, ref List<Transform> spawnPoints)
        {
            Transform spawnPoint;
            if (spawnPoints.Count == 0) return;
            
            spawnPoint = h.RandChoice(spawnPoints);
            organ.transform.position = spawnPoint.position;
            organ.transform.rotation = spawnPoint.rotation;
            
            if (spawnPoint.tag.Contains("Out"))
            {
                organ.UpdateOverlap(true);
            }
            else
            {
                organ.transform.localScale *= 0.9f;
            }
            
            spawnPoints.Remove(spawnPoint);
        }
        
        if (organ is Leg leg)
        {
            SetSpawnPoint(leg, ref legSpawnPoints);
        }
        else if (organ is Arm arm)
        {
            SetSpawnPoint(arm, ref armSpawnPoints);
        }
        
        
        
        // Calculate neuron position with gap
        float neuronBGWidth = 1f;
        if (neuronBG && neuronBG.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            neuronBGWidth = spriteRenderer.sprite.bounds.size.x * neuronBG.transform.localScale.x;
        }
        h.Out(neuronBGWidth);
        float gap = (neuronBGWidth * 0.9f) / (maxNeuronCount+2f) + 0.3f;
        Vector3 neuronPosition = neuronParent.position + neuronOffset;
        neuronPosition.x = neuronParent.position.x + ((neurons.Count+1) * gap) - neuronBGWidth/2;

        neuronPosition.y += h.Range(-0.3f, 0f);
        neuronPosition.x +=  h.Range(-0.25f, 0.2f);
        
        
        Neuron neuron = Instantiate(neuronPrefab, neuronPosition, Quaternion.identity, neuronParent);
        string actionKey;
        if (neurons.Count+1 == 10)
        {
            actionKey = "0";
        }
        else
        {
            actionKey = h.Str(neurons.Count+1);
        }
        neuron.Init(organ.gameObject, actionKey);
        neurons.Add(neuron);
    }
    
    public void ClearNeuronsAndOrgans()
    {
        // Destroy all organs
        foreach (OrganBase organ in organs)
        {
            if (organ)
            {
                Destroy(organ.gameObject);
            }
        }
        organs.Clear();
        
        // Destroy all neurons
        foreach (Neuron neuron in neurons)
        {
            if (neuron)
            {
                Destroy(neuron.gameObject);
            }
        }
        neurons.Clear();
    }
    
    
    
}

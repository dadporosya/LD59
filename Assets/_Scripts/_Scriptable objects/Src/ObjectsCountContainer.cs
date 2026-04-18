using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "ObjectsCountContainer", menuName = "Scriptable Objects/ObjectsCountContainer")]
public class ObjectsCountContainer : ScriptableObject
{
    public List<string> rawKeys;
    public List<int> rawValues;
    public int defaultValue = 0;

    [SerializeField] private bool initFromKindsContainer=true;
    public ObjectsKindsContainer<ScriptableObject> data;
    
    [SerializeField] private bool initFromKeyContainer = false;
    public KeyContainer keyData;

    
    [SerializeField] private bool initFromDefaultValues = false;
    public ObjectsCountContainer defaultData;
    
    //todo: add key list, which might be influenced by rand init
    public int maxValue=0;
    public int minSum=1;
    public int maxSum=1;
    public bool randValues=false;

    [HideInInspector] public Dictionary<string, int> values = new Dictionary<string, int>();
    [HideInInspector] public List<string> keys = new List<string>();

    public void Init(ObjectsCountContainer newData)
    {
        Init(newData.values.Keys.ToList(), newData.values.Values.ToList());
    }

    public void Init(List<string> keysIn, List<int> valuesIn, bool rand=false)
    {
        if (keysIn == null) return;
        InitValues(keysIn, valuesIn);
        // //h.Out("Initititit");
        // //h.Out(values);
        // //h.Out(keys);
        if (rand)
        {
            SetValuesZero();
            GenerateRandomValues();
        }
    }

    void OnEnable()
    {
        values.Clear();
        keys.Clear();

        var k = rawKeys;
        
        if (initFromKindsContainer && data)
        {
            k = data.keys;
        } else if (initFromDefaultValues && defaultData)
        {
            k = defaultData.keys;
        } else if (keyData && initFromKeyContainer)
        {
            k = keyData.keys;
        }
        // Create all necessary keys from kinds container with default value 0
        Init(k, rawValues);
        
        if (initFromDefaultValues && defaultData)
        {
            // Assign default values
            h.AssignValuesToDict(ref values, defaultData.values);
        }
        
        // Assign specific values, provided in the inspector
        h.AssignValuesToDict(ref values, rawKeys, rawValues);
    }

    void OnDisable()
    {
        // rawKeys.Clear();
        // rawValues.Clear();
        keys.Clear();
        values.Clear();
    }

    void GenerateRandomValues(int quota=0, int max=0)
    {
        if (keys.Count == 0 || values.Count == 0) InitValues(rawKeys, rawValues);
        ////h.Out("GENERATING");
        ////h.Out(values);
        // EditorApplication.isPaused = true;   // pause

        if (quota == 0) quota = UnityEngine.Random.Range(minSum, maxSum);
        if (max == 0) max = Math.Min(maxValue, quota);

        ////h.Out($"max: {max}, quota: {quota}");

        int n = keys.Count;

        List<int> remaining = new List<int>(); // reaminig indexes
        for (int i = 0; i < n; i++) remaining.Add(i);

        int currentV, currentI;

        int min = 1;
        
        for (int i = 0; i < n-1 && quota > 0; i++)
        {
            currentV = UnityEngine.Random.Range(min, Math.Min(max, quota)+1);
            
            min = 0;
            currentI = h.RandChoice(remaining);
            remaining.Remove(currentI);
            values[keys[currentI]] = currentV;

            ////h.Out($"{keys[currentI]}: {values[keys[currentI]]} = {currentV}");

            quota -= currentV;
        }

        if (quota > 0)
        {
            currentI = h.RandChoice(remaining);
            remaining.Remove(currentI);
            // //h.Out($"{currentI},");
            // //h.Out(keys);
            // //h.Out($" {keys[currentI]}");
            // //h.Out(values) ;
            values[keys[currentI]] = quota;

            ////h.Out($"{keys[currentI]}: {values[keys[currentI]]} = {quota}");

            quota = 0;
        }
    }

    public void SetValue(string key, int newValue)
    {
        values[key] = newValue;
    }

    public void ChangeValue(string key, int delta)
    {
        values[key] += delta;
    }

    public void Show()
    {
        ////h.Out(values);
    }

    public void SetValuesZero()
    {
        ////h.Out("clear");
        foreach(KeyValuePair<string, int> kv in values)
        {
            values[kv.Key] = defaultValue;
        }
        ////h.Out(values);
    }

    public void InitValues(List<string> keysIn, List<int> valuesIn=null)
    {
        if (valuesIn==null
            || valuesIn.Count != keysIn.Count)
        {
            valuesIn = h.CreateList(keysIn.Count, defaultValue);
        }
        
        values.Clear();
        keys.Clear();

        for (int i = 0; i < keysIn.Count; i++)
        {
            values[keysIn[i]] = valuesIn[i];
            keys.Add(keysIn[i]);
        }
    }
}

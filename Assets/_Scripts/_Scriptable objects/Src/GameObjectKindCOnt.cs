using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameObjectObjectsKindsContainer", menuName = "Scriptable Objects/Game Object ObjectsKindsContainer")]
public class GameObjectObjectsKindsContainer : ScriptableObject
{
    [SerializeField] public List<GameObject> rawObject; // mb swtich
    [SerializeField] public List<string> rawKeys;
    [HideInInspector] public Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();
    [HideInInspector] public List<string> keys;

    void OnEnable()
    {
        objects.Clear();
        keys.Clear();

        // if (rawKeys.Count != rawKeys.Count) throw new System.Exception($"Missing values or keys for {name}");
        for (int i = 0; i < h.Min(rawKeys.Count, rawObject.Count); i++)
        {
            objects.Add(rawKeys[i], rawObject[i]);
            keys.Add(rawKeys[i]);
            Debug.Log(rawKeys[i]);
        }
        h.Out(objects);
    }

    void OnDisable()
    {
        objects.Clear();
        keys.Clear();

    }
}
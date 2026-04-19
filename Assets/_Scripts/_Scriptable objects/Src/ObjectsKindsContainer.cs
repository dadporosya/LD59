// using UnityEngine;
// using System.Collections.Generic;
// using UnityEngine.InputSystem;
//
//
// public class ObjectsKindsContainer<T> : ScriptableObject where T : ScriptableObject
// {
//     [SerializeField] public List<T> rawObject; // mb swtich
//     [SerializeField] public List<string> rawKeys;
//     [HideInInspector] public Dictionary<string, T> objects = new Dictionary<string, T>();
//     [HideInInspector] public List<string> keys;
//
//     void OnEnable()
//     {
//         objects.Clear();
//         keys.Clear();
//
//         if (rawKeys.Count != rawKeys.Count) throw new System.Exception($"Missing values or keys for {name}");
//         for (int i = 0; i < rawKeys.Count; i++)
//         {
//             objects.Add(rawKeys[i], rawObject[i]);
//             keys.Add(rawKeys[i]);
//             Debug.Log(rawKeys[i]);
//         }
//     }
//
//     void OnDisable()
//     {
//         objects.Clear();
//         keys.Clear();
//
//     }
// }
//
// [CreateAssetMenu(fileName = "ObjectsKindsContainer", menuName = "Scriptable Objects/ObjectsKindsContainer")]
// public class ObjectScriptableKindsContainer : ObjectsKindsContainer<ScriptableObject>
// {
//     
// }
//
// [CreateAssetMenu(fileName = "OrganPurchaseItemContainer", menuName = "Scriptable Objects/Organ Purchase Item Container")]
// public class OrganPurchaseItemContainer : ObjectsKindsContainer<OrganPurchaseItem>
// {
//     
// }
//
// [CreateAssetMenu(fileName = "UpgradePurchaseItemContainer", menuName = "Scriptable Objects/Upgrade Purchase Item Container")]
// public class UpgradePurchaseItemContainer : ObjectsKindsContainer<UpgradePurchaseItem>
// {
//     
// }


using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ObjectsKindsContainer", menuName = "Scriptable Objects/ObjectsKindsContainer")]
public class ObjectsKindsContainer : ScriptableObject
{
    [SerializeField] public List<ScriptableObject> rawObject; // mb swtich
    [SerializeField] public List<string> rawKeys;
    [HideInInspector] public Dictionary<string, ScriptableObject> objects = new Dictionary<string, ScriptableObject>();
    [HideInInspector] public List<string> keys;

    void OnEnable()
    {
        objects.Clear();
        keys.Clear();

        if (rawKeys.Count != rawKeys.Count) throw new System.Exception($"Missing values or keys for {name}");
        for (int i = 0; i < rawKeys.Count; i++)
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
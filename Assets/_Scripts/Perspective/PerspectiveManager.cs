using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static PerspectiveManager Instance; // Instance
    public Quaternion perspectiveRotation;
    
    private void Awake()
    {
        h.CreateStaticInstance(this, ref Instance);
        perspectiveRotation = Camera.main.transform.rotation;
    }
}

using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static PerspectiveManager Instance; // Instance
    public Quaternion perspectiveRotation;
    public Vector3 perspectivePosition;
    [SerializeField] private bool assignByCamera = false;
    public bool ENABLE_PRESPECTIVE = true;
    
    private void Awake()
    {
        h.CreateStaticInstance(this, ref Instance);
        if (!ENABLE_PRESPECTIVE) return;
        if (assignByCamera)
        {
            perspectiveRotation = Camera.main.transform.rotation;
            perspectivePosition = Camera.main.transform.position;
        }
        else
        {
            Camera.main.transform.rotation = perspectiveRotation;
            Camera.main.transform.position = perspectivePosition;
        }
        
        
        
    }
}

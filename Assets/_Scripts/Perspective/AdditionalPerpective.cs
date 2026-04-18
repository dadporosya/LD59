using UnityEngine;

public class AdditionalPerspective : MonoBehaviour
{
    private void Start()
    {
        if (!PerspectiveManager.Instance.ENABLE_PRESPECTIVE) return;
        // transform.rotation = Quaternion.Inverse(
        //     PerspectiveManager.Instance.perspectiveRotation
        // );
        
        transform.rotation = Quaternion.identity;
    }
}
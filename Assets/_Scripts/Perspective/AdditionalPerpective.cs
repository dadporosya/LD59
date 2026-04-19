using UnityEngine;

public class Perspective : MonoBehaviour
{
    private void Start()
    {
        // if (!PerspectiveManager.Instance.ENABLE_PRESPECTIVE) return;
        // // transform.rotation = Quaternion.Inverse(
        // //     PerspectiveManager.Instance.perspectiveRotation
        // // );
        //
        // transform.rotation = Quaternion.identity;
        
        transform.rotation = Quaternion.Euler(Preferences.perspectiveRotation);
    }
}
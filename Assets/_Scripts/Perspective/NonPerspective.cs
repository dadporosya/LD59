using UnityEngine;

public class NonPerspective : MonoBehaviour
{
    private void Start()
    {
        if (!PerspectiveManager.Instance.ENABLE_PRESPECTIVE) return;

        transform.rotation = PerspectiveManager.Instance.perspectiveRotation;
    }
}

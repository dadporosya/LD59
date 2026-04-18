using UnityEngine;

public class NonPerspective : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = PerspectiveManager.Instance.perspectiveRotation;
    }
}

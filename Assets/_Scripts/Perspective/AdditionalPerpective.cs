using UnityEngine;

public class AddiciotnalPerpective : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = -PerspectiveManager.Instance.perspectiveRotation;
    }
}

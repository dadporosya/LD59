using UnityEngine;

public class AlignByBorder : MonoBehaviour
{
    [SerializeField] private bool alignByCameraTopBound;
    [SerializeField] private Vector3 offset =  new Vector3(0f, 0.3f, 0f);

    private void Awake()
    {
        if (alignByCameraTopBound)
        {
            AlignByCameraTopBound();
        }
    }

    public void AlignByCameraTopBound()
    {
        // Get the camera's top bound Y coordinate
        float cameraTopY = h.GetCameraTopLeftCorner().y;
        
        // Get all sprite renderers from this object and children
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 0) return;
        
        // Find the topmost point among all child sprites
        float objectTop = float.MinValue;
        foreach (SpriteRenderer sr in sprites)
        {
            float spriteTop = sr.transform.position.y + (sr.bounds.size.y / 2f);
            if (spriteTop > objectTop)
            {
                objectTop = spriteTop;
            }
        }
        
        // Calculate the offset needed to align with camera top
        float offsetY = cameraTopY - objectTop;
        
        // Move the object
        transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z) + offset;
    }
}



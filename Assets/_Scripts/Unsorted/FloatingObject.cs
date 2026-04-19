using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private Collider2D movementArea; // Green collider (allowed area)
    [SerializeField] private bool defaultSpeed = true;
    [SerializeField] private float baseFloatingSpeed = 2f;
    private float floatingSpeed = 2f;
    [SerializeField] private float ignoreDuration = 0.1f;

    private Collider2D objectCollider;
    private Vector2 direction;
    private float ignoreTimer = 0f;

    public List<Transform> vesselPoints;
    public List<Connection> vesselPrefabs;

    private void Start()
    {
        if (!movementArea)
        {
            movementArea = GameObject.FindGameObjectWithTag("OrganBound").GetComponent<Collider2D>();
        }
        if (defaultSpeed) baseFloatingSpeed = Preferences.floatingSpeed;
        floatingSpeed = baseFloatingSpeed;
        objectCollider = GetComponent<Collider2D>();

        direction = Random.insideUnitCircle.normalized;
        
        // Find all BloodVesselPoint children in self and add them to vesselPoints
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BloodVesselPoint"))
            {
                if (!vesselPoints.Contains(child))
                {
                    vesselPoints.Add(child);
                }
            }
        }
        
        GenerateVessels();
        
        
    }

    private void GenerateVessels()
    {
        // Find all objects with tag BloodVesselPoint
        GameObject[] allVesselPointObjects = GameObject.FindGameObjectsWithTag("BloodVesselPoint");
        List<Transform> availablePoints = new List<Transform>();
        
        // Add points that are not already in vesselPoints
        foreach (GameObject obj in allVesselPointObjects)
        {
            if (!vesselPoints.Contains(obj.transform))
            {
                availablePoints.Add(obj.transform);
            }
        }
        
        if (availablePoints.Count == 0 || vesselPoints.Count == 0)
        {
            h.Out("No available vessel points");
            return;
        }
        
        List<Transform> availableThisVessels = new List<Transform>();
        availableThisVessels.AddRange(vesselPoints);
        
        int vesselCount = h.Range(1, vesselPoints.Count);

        for (int i = 0; i < vesselCount; i++)
        {
            Transform point1 = h.RandChoice(availablePoints);
            if (!point1) return;
            availablePoints.Remove(point1);
            
            Transform point2 = h.RandChoice(availableThisVessels);
            availableThisVessels.Remove(point2);
            
            Connection vessel = Instantiate(h.RandChoice(vesselPrefabs), point2.position, Quaternion.identity, transform);
            vessel.points.Add(point1);
            vessel.points.Add(point2);
            
            
        }
    }

    private void Update()
    {
        // Move object
        transform.Translate(direction * floatingSpeed * Time.deltaTime);

        // If object's collider is no longer fully inside green area -> bounce
        if (!IsFullyInside() && ignoreTimer <= 0)
        {
            Bounce();
        }

        // Handle ignore timer
        if (ignoreTimer > 0)
        {
            ignoreTimer -= Time.deltaTime;
        }
    }

    private bool IsFullyInside()
    {
        Bounds areaBounds = movementArea.bounds;
        Bounds objBounds = objectCollider.bounds;

        // Check if all sides of black collider are inside green collider bounds
        return areaBounds.Contains(objBounds.min) &&
               areaBounds.Contains(objBounds.max);
    }

    private void Bounce()
    {
        // Reverse direction
        direction = -direction;

        // Small randomness for more natural movement
        direction += Random.insideUnitCircle * 0.25f;
        direction.Normalize();

        // Push object slightly back inside to prevent sticking
        transform.Translate(direction * floatingSpeed * Time.deltaTime * 2f);

        // Reset ignore timer
        ignoreTimer = ignoreDuration;
        
        floatingSpeed = h.RangeWithCoof(baseFloatingSpeed, 0.05f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO fix collision with others floating objects
        if (collision.CompareTag("FloatingObject"))
        {
            Bounce();
        }
        // Ignore collisions with the edges immediately after bouncing
        if (ignoreTimer > 0)
        {
            Physics2D.IgnoreCollision(objectCollider, collision, true);
            return;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Restore collision detection when exiting the ignore period
        if (collision == movementArea && ignoreTimer <= 0)
        {
            Physics2D.IgnoreCollision(objectCollider, collision, false);
        }
    }
}
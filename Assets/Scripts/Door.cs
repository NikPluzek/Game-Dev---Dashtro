using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{

    [Header("Connection")]
    public Room connectedRoom;
    public Transform exitPoint;

    private bool isLocked = false;
    private bool playerInRange = false;
    private RoomManager roomManager;

    // References to block/unblock the doorway
    private Renderer[] doorRenderers;
    private Collider solidCollider;
    private NavMeshObstacle navMeshObstacle;

    void Start()
    {
        roomManager = FindFirstObjectByType<RoomManager>();

        // Search children too in case mesh is on a child object
        doorRenderers = GetComponentsInChildren<Renderer>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        // Find the non-trigger collider specifically
        Collider[] allColliders = GetComponents<Collider>();
        foreach (Collider col in allColliders)
        {
            if (!col.isTrigger)
            {
                solidCollider = col;
                break;
            }
        }
    }

    void Update()
    {

    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (isLocked)
        {
            foreach (Renderer r in doorRenderers) r.enabled = true;
            if (solidCollider != null) solidCollider.enabled = true;
            if (navMeshObstacle != null) navMeshObstacle.enabled = true;
        }
        else
        {
            foreach (Renderer r in doorRenderers) r.enabled = false;
            if (solidCollider != null) solidCollider.enabled = false;
            if (navMeshObstacle != null) navMeshObstacle.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!isLocked)
                Debug.Log("Press E to enter " + connectedRoom?.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    
}
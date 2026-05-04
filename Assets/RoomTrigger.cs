using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Room room;

    private RoomManager roomManager;

    void Start()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Room trigger hit!");
            roomManager.TransitionToRoom(room, null);
        }
    }
}
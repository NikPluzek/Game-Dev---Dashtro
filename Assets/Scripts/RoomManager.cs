using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Rooms")]
    public Room startingRoom;

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 15, -5); 

    private Room currentRoom;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        // Start in the starting room
        TransitionToRoom(startingRoom, null);
    }

    public void TransitionToRoom(Room newRoom, Door enteredThrough)
    {
        currentRoom = newRoom;

        Vector3 targetPos = newRoom.cameraTarget != null ? newRoom.cameraTarget.position : newRoom.transform.position;
        cameraTransform.position = targetPos + cameraOffset;

        newRoom.OnRoomEntered();

        Debug.Log("Transitioning to: " + newRoom.gameObject.name);
    }
}
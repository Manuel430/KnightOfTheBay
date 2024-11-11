using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerLoc;

    [Header("Rooms")]
    [SerializeField] List<RoomLayout> rooms;
    [SerializeField] RoomLayout currentRoom;

    [Header("Transition")]
    [SerializeField] float transitionSpeed;
    [SerializeField] bool isTransitioning;

    private void Awake()
    {
        rooms = new List<RoomLayout>(FindObjectsOfType<RoomLayout>());
        currentRoom = GetRoomForPlayer(playerLoc.position);
        if(currentRoom != null)
        {
            MoveCameraInRoom(currentRoom);
        }
    }

    private void Update()
    {
        RoomLayout newRoom = GetRoomForPlayer(playerLoc.position);

        if(newRoom != null && newRoom != currentRoom && !isTransitioning)
        {
            StartCoroutine(CameraTransition(newRoom));
        }

        if(isTransitioning && currentRoom != null)
        {
            if(currentRoom.followPlayer)
            {
                if(currentRoom.verticalRoom)
                {
                    MoveCameraVertically(currentRoom);
                }
                else
                {
                    MoveCameraInRoom(currentRoom);
                }
            }
            else
            {
                MoveCameraToCenter(currentRoom);
            }
        }
    }

    RoomLayout GetRoomForPlayer(Vector3 playerPos)
    {
        foreach(RoomLayout room in rooms)
        {
            if(IsPlayerInRoom(playerPos, room))
            {
                return room;
            }
        }

        return null;
    }

    bool IsPlayerInRoom(Vector3 playerPos, RoomLayout room)
    {
        Vector2 min = room.roomCenter - room.roomSize / 2;
        Vector2 max = room.roomCenter + room.roomSize / 2;

        return playerPos.x > min.x && playerPos.x < max.x && playerPos.y > min.y && playerPos.y < max.y;
    }

    void MoveCameraInRoom(RoomLayout room)
    {
        Vector2 minBounds = room.roomCenter - room.roomSize / 2;
        Vector3 maxBounds = room.roomCenter + room.roomSize / 2;

        Vector3 followPlayer = new Vector3(playerLoc.position.x, playerLoc.position.y, transform.position.z);

        float clampX = Mathf.Clamp(followPlayer.x, minBounds.x + CameraHalfWidth(), maxBounds.x - CameraHalfWidth());
        float clampY = Mathf.Clamp(followPlayer.y, minBounds.y + CameraHalfHeight(), maxBounds.y - CameraHalfHeight());

        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }

    void MoveCameraToCenter(RoomLayout room)
    {
        transform.position = new Vector3(room.roomCenter.x, room.roomCenter.y, transform.position.z);
    }

    void MoveCameraVertically(RoomLayout room)
    {
        Vector2 minBounds = room.roomCenter - room.roomSize / 2;
        Vector3 maxBounds = room.roomCenter + room.roomSize / 2;

        Vector3 followPlayer = new Vector3(playerLoc.position.x, playerLoc.position.y, transform.position.z);

        float clampY = Mathf.Clamp(followPlayer.y, minBounds.y + CameraHalfHeight(), maxBounds.y - CameraHalfHeight());

        transform.position = new Vector3(room.roomCenter.x, clampY, transform.position.z);
    }

    float CameraHalfWidth()
    {
        return Camera.main.orthographicSize * Camera.main.aspect;
    }

    float CameraHalfHeight()
    {
        return Camera.main.orthographicSize;
    }

    IEnumerator CameraTransition(RoomLayout newRoom)
    {
        isTransitioning = true;

        Vector3 playerPos = new Vector3(playerLoc.position.x, playerLoc.position.y, transform.position.z);

        Vector2 minBounds = newRoom.roomCenter - newRoom.roomSize / 2;
        Vector3 maxBounds = newRoom.roomCenter + newRoom.roomSize / 2;

        float clampX = Mathf.Clamp(playerPos.x, minBounds.x + CameraHalfWidth(), maxBounds.x - CameraHalfWidth());
        float clampY = Mathf.Clamp(playerPos.y, minBounds.y + CameraHalfHeight(), maxBounds.y - CameraHalfHeight());

        Vector3 targetPos = new Vector3(clampX, clampY, transform.position.z);
        Vector3 startPos = transform.position;

        float elapsedTime = 0;
        float duration = Vector3.Distance(targetPos, startPos) / transitionSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        currentRoom = newRoom;
        isTransitioning = false;
    }
}

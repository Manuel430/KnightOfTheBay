using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayout : MonoBehaviour
{
    [Header("Room Size")]
    public Vector2 roomSize;
    public Vector2 roomCenter;

    [Header("Camera Movement")]
    public bool followPlayer;
    public bool verticalRoom;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(roomCenter, roomSize);
    }
}

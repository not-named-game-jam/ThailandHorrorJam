using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room6ToEndtrue : MonoBehaviour
{
    [SerializeField] RoomEndHover enterEnd;
    void Start()
    {
        enterEnd.inRoomEnd = true;
    }

}

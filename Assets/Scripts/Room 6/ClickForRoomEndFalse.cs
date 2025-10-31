using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickForRoomEndFalse : MonoBehaviour
{
    [SerializeField] RoomEndHover isRoomEnd;
    void Update()
    {

    }
    
    public void ClicktoFalseEnd()
    {
        isRoomEnd.inRoomEnd = false;
    }
}

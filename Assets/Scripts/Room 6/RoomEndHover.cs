using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomEndHover : MonoBehaviour
{
    [SerializeField] GameObject door1close;
    [SerializeField] GameObject door1open;
    [SerializeField] GameObject door1light;
    [SerializeField] GameObject door2close;
    [SerializeField] GameObject door2open;
    [SerializeField] GameObject door2light;
    [SerializeField] GameObject door3close;
    [SerializeField] GameObject door3open;
    [SerializeField] GameObject door3light;
    [SerializeField] GameObject door4close;
    [SerializeField] GameObject door4open;
    [SerializeField] GameObject door4light;
    

    bool door1hovering;
    bool door2hovering;
    bool door3hovering;
    bool door4hovering;
    public bool inRoomEnd;


    void Start() 
    {

    }

    void Update()
    {
        if (inRoomEnd && door1hovering)
        {
            door1open.SetActive(true);
            door1light.SetActive(true);
            door1close.SetActive(false);
        }

        if (inRoomEnd && door2hovering)
        {
            door2open.SetActive(true);
            door2light.SetActive(true);
            door2close.SetActive(false);
        }

        if (inRoomEnd && door3hovering)
        {
            door3open.SetActive(true);
            door3light.SetActive(true);
            door3close.SetActive(false);
        }

        if (inRoomEnd && door4hovering)
        {
            door4open.SetActive(true);
            door4light.SetActive(true);
            door4close.SetActive(false);
        }

        if (inRoomEnd && !door1hovering)
        {
            door1close.SetActive(true);
            door1open.SetActive(false);
            door1light.SetActive(false);
        }

        if (inRoomEnd && !door2hovering)
        {
            door2close.SetActive(true);
            door2open.SetActive(false);
            door2light.SetActive(false);
        }

        if (inRoomEnd && !door3hovering)
        {
            door3close.SetActive(true);
            door3open.SetActive(false);
            door3light.SetActive(false);
        }

        if (inRoomEnd && !door4hovering)
        {
            door4close.SetActive(true);
            door4open.SetActive(false);
            door4light.SetActive(false);
        }
        
    }

    public void OnPointerEnterDoor1()
    {
        door1hovering = true;
    }

    public void OnPointerExitDoor1()
    {
        door1hovering = false;
    }
    
    public void OnPointerEnterDoor2()
    {
        door2hovering = true;
    }

    public void OnPointerExitDoor2()
    {
        door2hovering = false;
    }
    
    public void OnPointerEnterDoor3()
    {
        door3hovering = true;
    }

    public void OnPointerExitDoor3()
    {
        door3hovering = false;
    }
    
    public void OnPointerEnterDoor4()
    {
        door4hovering = true;
    }

    public void OnPointerExitDoor4()
    {
        door4hovering = false;
    }
    
}

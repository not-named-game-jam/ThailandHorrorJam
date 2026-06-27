using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPanning : MonoBehaviour
{

    [SerializeField] public Transform centerObject;
    [SerializeField] private Transform Object;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] public float cameraSpeed = 100f;
    [SerializeField] public DialogueSystem isDialogueActive;
    //[SerializeField] private InEndingtrue inEnding;
    public bool stopCamWhenDialogue;
    private float cameraDeadzone = 5.055f;
    [SerializeField] private float borderThickness = 20f;
    void Start()
    {
        KeepingSettingValues.savedcameraSpeed = cameraSpeed;
    }

    // void Update()
    // {
    //     if (!Input.GetMouseButton(1))
    //     {
    //         cameraSpeed = 0;
    //         return;
    //     }

    //     if (CheckOverlayActive.IsOverlayActive)
    //     {
    //         cameraSpeed = 0;
    //         return;
    //     }
        
    //     if (isDialogueActive != null && isDialogueActive.IsActive && stopCamWhenDialogue || InEndingtrue.InEnding)
    //     {
    //         cameraSpeed = 0;
    //         return;
    //     }

    //     else
    //     {
    //         cameraSpeed = KeepingSettingValues.savedcameraSpeed;
    //     }

        
    //     Vector3 mousePosi = Input.mousePosition;
    //     mousePosi.z = cameraDeadzone - 0.055f;
    //     Vector3 toWorldmousePosi = cam.ScreenToWorldPoint(mousePosi);
    //     Vector3 newPosi = Vector3.MoveTowards(rb.position, toWorldmousePosi, Time.deltaTime * cameraSpeed);
    //     float deadZone = Vector3.Distance(rb.position, toWorldmousePosi);
    //     if (deadZone > cameraDeadzone)
    //     {
    //         rb.MovePosition(newPosi);
    //     }

    // }
    void Update()
    {

        Vector3 mousePosi = Input.mousePosition;

        bool hittingBorder = mousePosi.x <= borderThickness || 
                             mousePosi.x >= Screen.width - borderThickness || 
                             mousePosi.y <= borderThickness || 
                             mousePosi.y >= Screen.height - borderThickness;

        if (!hittingBorder)
        {
            cameraSpeed = 0;
            return;
        }


        if (CheckOverlayActive.IsOverlayActive)
        {
            cameraSpeed = 0;
            return;
        }
        
        if (isDialogueActive != null && isDialogueActive.IsActive && stopCamWhenDialogue || InEndingtrue.InEnding)
        {
            cameraSpeed = 0;
        }
        else
        {
            cameraSpeed = KeepingSettingValues.savedcameraSpeed;
        }

    
        mousePosi.z = cameraDeadzone - 0.055f;
        Vector3 toWorldmousePosi = cam.ScreenToWorldPoint(mousePosi);
        Vector3 newPosi = Vector3.MoveTowards(rb.position, toWorldmousePosi, Time.deltaTime * cameraSpeed);
        float deadZone = Vector3.Distance(rb.position, toWorldmousePosi);
        
        if (deadZone > cameraDeadzone)
        {
            rb.MovePosition(newPosi);
        }
    }
}





using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [SerializeField] public Transform centerObject;
    [SerializeField] private Transform Object;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] public float cameraSpeed = 100f;
    [SerializeField] public DialogueSystem isDialogueActive;
    [SerializeField] private InEndingtrue inEnding;
    public bool stopCamWhenDialogue;
    private float savedcameraSpeed;
    private float cameraDeadzone = 5.055f;
    void Start()
    {
        savedcameraSpeed = cameraSpeed;
    }

    void Update()
    {
        if (CheckOverlayActive.IsOverlayActive)
        {
            cameraSpeed = 0;
            return;
        }
        
        if (isDialogueActive != null && isDialogueActive.IsActive && stopCamWhenDialogue && !inEnding.InEnding)
        {
            cameraSpeed = 0;
        }

        else
        {
            cameraSpeed = savedcameraSpeed;
        }
        
        Vector3 mousePosi = Input.mousePosition;
        mousePosi.z = cameraDeadzone - 0.055f;
        Vector3 toWorldmousePosi = cam.ScreenToWorldPoint(mousePosi);
        Vector3 newPosi = Vector3.MoveTowards(rb.position, toWorldmousePosi, Time.deltaTime * cameraSpeed);
        float deadZone = Vector3.Distance(rb.position, toWorldmousePosi);
        if (deadZone > cameraDeadzone)
        {
            rb.MovePosition(newPosi);
        }

        if (inEnding.InEnding)
        {
            cameraSpeed = 0;
            Vector2 centerPos = centerObject.position;
            rb.MovePosition(centerPos);
        }

    }
}

//1.สร้าง empty object ใส่ rigidbody2D,ใส่ transform,collider 
//2.สร้าง virtualcamera แล้วให้ follow ตัว object 
//3.วาง object เพื่อใส่ edge collider กับ background
//4.สร้าง collider เพื่อเป็น confiner สำหรับ virtualcamera ปรับขนาดให้พอดีกับ background
//5.สร้าง edge collider เพื่อกันไม่ให้ object ไหลออกไกลเกินไป ปรับขนาดให้เล็ก แต่พอดีที่จะทำให้ virtualcamera เห็น background ทั้งหมด




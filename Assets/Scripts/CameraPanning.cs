using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [SerializeField] private Transform Object;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] public float cameraSpeed = 100f;
    private float cameraDeadzone = 5.005f;
    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePosi = Input.mousePosition;
        mousePosi.z = cameraDeadzone - 0.005f;
        Vector3 toWorldmousePosi = cam.ScreenToWorldPoint(mousePosi);
        Vector3 newPosi = Vector3.MoveTowards(rb.position, toWorldmousePosi, Time.deltaTime * cameraSpeed);
        float deadZone = Vector3.Distance(rb.position, toWorldmousePosi);
        if (deadZone > cameraDeadzone)
        {
            rb.MovePosition(newPosi);
        }
    }
}

//1.สร้าง empty object ใส่ rigidbody2D,ใส่ transform,collider 
//2.สร้าง virtualcamera แล้วให้ follow ตัว object 
//3.วาง object เพื่อใส่ edge collider กับ background
//4.สร้าง collider เพื่อเป็น confiner สำหรับ virtualcamera ปรับขนาดให้พอดีกับ background
//5.สร้าง edge collider เพื่อกันไม่ให้ object ไหลออกไกลเกินไป ปรับขนาดให้เล็ก แต่พอดีที่จะทำให้ virtualcamera เห็น background ทั้งหมด




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room6Star : MonoBehaviour
{
    public float rotateSpeed = 30f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}

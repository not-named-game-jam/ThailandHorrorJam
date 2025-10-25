using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Room3 : MonoBehaviour
{
    public bool isRikaCalm = false;
    public int clickCount = 0;
  

    void Start()
    {
        
    }


    void Update()
    {
        if (clickCount == 30)
        {
            Destroy(gameObject);
            isRikaCalm = true;
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }
    }


}

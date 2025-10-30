using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class Clock : MonoBehaviour
{
    private int hour = 0;
    private int tmpHour = 0;
    private int minute = 0;
    private float degree = 0;
    private bool isSelectingHour = true;
    private bool isAM = true;
    [SerializeField] private RectTransform hourRoot;
    [SerializeField] private RectTransform minRoot;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text time;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setIsHour(bool isHour)
    {
        isSelectingHour = isHour;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePoint = Input.mousePosition;
        if (isSelectingHour)
        {
            Vector3 dir = mousePoint - hourRoot.position;
            degree = Mathf.Atan2(dir.y , dir.x)*Mathf.Rad2Deg;
            if (degree < 0)
            {
                hour = 12 + Mathf.RoundToInt(degree / 30);     
            }
            else
            {
                hour = Mathf.RoundToInt(degree / 30);    
            }

            hour = hour - 3;
            if (hour < 0)
            {
                hour = 12 + hour;
            }
            hour = 12 - hour;
            hourRoot.eulerAngles = new Vector3(0, 0, degree + 30);
        }
        else
        {
            Vector3 dir = mousePoint - minRoot.position;
            degree = Mathf.Atan2(dir.y , dir.x)*Mathf.Rad2Deg;
            if (degree < 0)
            {
                minute = 60 + Mathf.RoundToInt(degree / 6);     
            }
            else
            {
                minute = Mathf.RoundToInt(degree / 6);    
            }
            minute = minute - 15;
            if (minute < 0)
            {
                minute = 60 + minute;
            }
            minute = 60 - minute;
            if (minute == 60)
            {
                minute = 0;
            }
            minRoot.eulerAngles = new Vector3(0, 0, degree);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isSelectingHour)
            {
                isSelectingHour = false;
            }
            else
            {
                canvas.SetActive(false);        
            }
        }

        if (hour.ToString().Length == 1 && minute.ToString().Length == 1)
        {
            time.text = "0" + hour + ":0" + minute;    
        } 
        else if (hour.ToString().Length == 1)
        {
            time.text = "0" + hour + ":" + minute;
        }
        else if (minute.ToString().Length == 1)
        {
            time.text = hour + ":0" + minute;
        }
        else
        {
            time.text = hour + ":" + minute;    
        }
    }
}

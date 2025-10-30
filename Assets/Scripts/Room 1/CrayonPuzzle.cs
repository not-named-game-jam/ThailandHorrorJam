using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrayonPuzzle : MonoBehaviour
{
    [SerializeField] GameObject littlechildnoclick;
    [SerializeField] GameObject littlechildcanclick;
    [SerializeField] float minTimeBetweenKnocks = 8.0f;
    [SerializeField] float maxTimeBetweenKnocks = 15.0f;
    [SerializeField] float knockVolume = 1f;
    
    bool getBlueCrayon = false;
    bool getRedCrayon = false;
    bool getGreenCrayon = false;
    
    private float nextKnockTime;

    void OnEnable() {
        nextKnockTime = Time.time + Random.Range(minTimeBetweenKnocks, maxTimeBetweenKnocks) + 8;
    }

    void Update()
    {
        var door1 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "Left Door Close 1");
        var door2 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "Left Door Close 2");
        var door3 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "Left Door Close 3");
        
        bool anyDoorActive = false;
        bool isLoudKnock = false;
        
        if (door1 != null && door1.activeSelf) anyDoorActive = true;
        if (door2 != null && door2.activeSelf) anyDoorActive = true;
        if (door3 != null && door3.activeSelf) 
        {
            anyDoorActive = true;
            isLoudKnock = true;
        }
        
        if (anyDoorActive && Time.time >= nextKnockTime)
        {
            string knockSound = isLoudKnock ? "DoorKnockLoud" : "DoorKnock";
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySfx(knockSound, knockVolume);
            }
            if(knockSound == "DoorKnockLoud") {
                nextKnockTime = Time.time + Random.Range(4.0f, minTimeBetweenKnocks);
            }
            else {
                nextKnockTime = Time.time + Random.Range(minTimeBetweenKnocks, maxTimeBetweenKnocks);
            }
        }
        if (getBlueCrayon && getRedCrayon && getGreenCrayon)
        {
            Debug.Log("Can Click Child");
            littlechildnoclick.SetActive(false);
            littlechildcanclick.SetActive(true);
            tofalse();
        }
    }

    public void GetBlueCrayon()
    {
        getBlueCrayon = true;
        Debug.Log("GetBlueCrayon"+ getBlueCrayon);
    }

    public void GetRedCrayon()
    {
        getRedCrayon = true;
        Debug.Log("GetRedCrayon"+getRedCrayon);
    }

    public void GetGreenCrayon()
    {
        getGreenCrayon = true;
        Debug.Log("GetGreenCrayon"+getGreenCrayon);
    }
    
    void tofalse()
    {
        getBlueCrayon = false;
        getRedCrayon = false;
        getGreenCrayon = false;
    }
}

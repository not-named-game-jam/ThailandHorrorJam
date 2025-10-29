using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrayonPuzzle : MonoBehaviour
{
    [SerializeField]GameObject littlechildnoclick;
    [SerializeField]GameObject littlechildcanclick;
    bool getBlueCrayon = false;
    bool getRedCrayon = false;
    bool getGreenCrayon = false;

    void Start()
    {
        
    }


    void Update()
    {
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

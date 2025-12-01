using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayActiveFalse : MonoBehaviour
{
    [SerializeField] GameObject FalseObject;

    void Update()
    {   
        CheckOverlayActive.IsOverlayActive = false;
        Debug.Log("false");
        Disable();
    }

    void Disable()
    {
        FalseObject.SetActive(false);
    }
}

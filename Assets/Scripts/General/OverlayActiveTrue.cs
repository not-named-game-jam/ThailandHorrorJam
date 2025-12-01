using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayActiveTrue : MonoBehaviour
{
    [SerializeField] GameObject TrueObject;

    void Update()
    {
        CheckOverlayActive.IsOverlayActive = true;
        Debug.Log("true");
        Disable();
    }

    void Disable()
    {
        TrueObject.SetActive(false);
    }
}

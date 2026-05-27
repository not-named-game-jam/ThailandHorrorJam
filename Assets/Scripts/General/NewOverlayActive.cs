using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOverlayActive : MonoBehaviour
{
    public void ToggleOverlayActive()
    {
        if (CheckOverlayActive.IsOverlayActive)
        {
            CheckOverlayActive.IsOverlayActive = false;
        }
        else
        {
            CheckOverlayActive.IsOverlayActive = true;
        }
    }
}

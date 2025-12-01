using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveOverlayActiveorNot : MonoBehaviour
{
    public void getOverlayTrue()
    {
        CheckOverlayActive.IsOverlayActive = true;
        Debug.Log(CheckOverlayActive.IsOverlayActive);
    }

    public void getOverlayFalse()
    {

        CheckOverlayActive.IsOverlayActive = false;
        Debug.Log(CheckOverlayActive.IsOverlayActive);
    }
    
}

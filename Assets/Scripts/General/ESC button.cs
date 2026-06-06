using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ESCbutton : MonoBehaviour
{
    [SerializeField] GameObject tint;
    [SerializeField] GameObject continuebutton;
    [SerializeField] GameObject settingbutton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CheckOverlayActive.IsOverlayActive && !DialogueSystem.instance.IsActive)
        {
            tint.SetActive(true);
            continuebutton.SetActive(true);
            settingbutton.SetActive(true);
            CheckOverlayActive.IsOverlayActive = true;
        }
    }
}

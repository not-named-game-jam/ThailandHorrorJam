using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ESCbutton : MonoBehaviour
{
    public DialogueSystem dialogueisActive;
    public GameObject menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CheckOverlayActive.IsOverlayActive && !dialogueisActive.IsActive)
        {
            menu.SetActive(true);
            CheckOverlayActive.IsOverlayActive = true;
        }
    }
}

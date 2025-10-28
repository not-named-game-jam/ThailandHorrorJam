using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickEventMethods : MonoBehaviour
{
    // Attach this class to objs/canvas you want to pop up



    [Header("Settings")]
    [SerializeField] ClickEventType clickEventType;

    //[Header("References")]
    // [SerializeField] Animator animator;

    public enum ClickEventType
    {
        Popup, // All room5: Calendar, paper pieces, wood block game, clock game, old wood box
        CloseEvent, // Close the popup (press anywhere that is NOT the popped up obj)
        PlayAnimation // play an interactable-specific animation: Room5-Hand Tree, 

    }

    private Animator animator;
    // private static readonly int PopupHash = Animator.StringToHash("PopUp");

    void Awake()
    {
        if (animator == null)
        {
            if (TryGetComponent<Animator>(out Animator animator))
            {
                this.animator = animator;
            } else
            {
                this.animator = null;
            }
        }
    }



    void Update()
    {
        // if (Pressed() && !EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("hi");
        //     gameObject.transform.parent.gameObject.SetActive(false);
        // }

        // if (Pressed())
        // {
        //     if (!IsPointerOverSpecificUI("puzzleUI")) 
        //     {
        //         gameObject.SetActive(false);
        //     }
        // }
    }
    
    // private bool IsPointerOverSpecificUI(string tag)
    // {
    //     PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
    //     {
    //         position = Input.mousePosition
    //     };

    //     // List of all the raycastresults --> check if the UI with tag is hit
    //     List<RaycastResult> results = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(pointerEventData, results);

    //     foreach (RaycastResult hit in results)
    //     {
    //         if (hit.gameObject.CompareTag(tag))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // void OnEnable()
    // {
    //     if (currentAnimStateID == 0) return;

    //     animator.Play(currentAnimStateID);
    // }


    public void StartEvent() // called by button component
    {
        
        switch (clickEventType)
        {
            case ClickEventType.Popup:
                if (gameObject.activeSelf) return;
                gameObject.SetActive(true);
                break;

            case ClickEventType.CloseEvent:
                gameObject.transform.parent.gameObject.SetActive(false);
                break;
            case ClickEventType.PlayAnimation:
                PlayAnimation();
                break;
        }
    }

    public void PlayAnimation() // For Tree
    {
        if (animator == null) { Debug.LogWarning($"Animator of {gameObject.name} is null!"); return; }

        Debug.Log($"Playing {animator.gameObject.name}'s animator!");
        animator.ResetTrigger("Play");
        animator.SetTrigger("Play");
    }

    private bool Pressed() =>
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Return);

    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("puzzleUI"))
    //     {
    //         gameObject.transform.parent.gameObject.SetActive(false);
    //     }
    // }
}


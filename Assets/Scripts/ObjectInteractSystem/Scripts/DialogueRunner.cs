using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueRunner : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] DialogueMaker dialogueMaker;

    public void OnPointerDown(PointerEventData eventData)
    {
        // if (eventData.pointerPressRaycast.gameObject.TryGetComponent<>)
        // {
        //     PuzzleAnimationEvent?.Invoke(true);
        // }

        dialogueMaker.StartDialogue();
    }

}

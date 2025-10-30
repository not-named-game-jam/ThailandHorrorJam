using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStart : MonoBehaviour
{
    [SerializeField] DialogueMaker dialogue;

    void Start()
    {
        dialogue.StartDialogue();
    }
}

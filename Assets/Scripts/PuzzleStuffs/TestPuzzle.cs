using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPuzzle : MonoBehaviour
{
    [SerializeField] DialogueMaker noFl;
    [SerializeField] DialogueMaker haveFl;

    bool haveFlashlight;

    public void GetFlashlight()
    {
        haveFlashlight = true;
    }

    public void RunDialogue()
    {
        if(haveFlashlight) haveFl.StartDialogue();
        else noFl.StartDialogue();
    }
}

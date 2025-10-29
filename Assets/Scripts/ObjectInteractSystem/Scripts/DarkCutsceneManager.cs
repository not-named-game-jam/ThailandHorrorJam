using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCutsceneManager : MonoBehaviour
{
    [SerializeField] DialogueMaker darkCutsceneDialogue;
    void Start()
    {
        darkCutsceneDialogue.StartDialogue();
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroToRoom1 : MonoBehaviour
{
    [SerializeField] GameObject introparent;
    [SerializeField] DialogueMaker Room1Start;
    bool alreadyDoneIntro;

    void Start()
    {
        
    }


    void Update()
    {
        if (alreadyDoneIntro && !introparent.activeSelf)
        {
            Room1Start.StartDialogue();
            alreadyDoneIntro = false;
        }
        
        if (introparent.activeSelf)
        {
            alreadyDoneIntro = true;
        }
    }
}

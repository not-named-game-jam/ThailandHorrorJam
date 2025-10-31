using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromRoom3toRoom4 : MonoBehaviour
{
    [SerializeField] GameObject room3parent;
    [SerializeField] DialogueMaker Room4Start;
    bool alreadyDoneRoom3;

    void Start()
    {
        
    }


    void Update()
    {
        if (alreadyDoneRoom3 && !room3parent.activeSelf)
        {
            Room4Start.StartDialogue();
            alreadyDoneRoom3 = false;
        }
        
        if (room3parent.activeSelf)
        {
            alreadyDoneRoom3 = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTriggers : MonoBehaviour
{
    [SerializeField] Clock clock;
    [SerializeField] DialogueMaker screech;
    [SerializeField] DialogueMaker puzzle1failure;
    [SerializeField] DialogueMaker puzzle1solved;

    public void Puzzle1Wrong()
    {
        Debug.Log("Wrong");
        screech.StartDialogue();
        if (true) // TBA
        {
            puzzle1failure.StartDialogue();
        }
    }
    public void Puzzle1Correct()
    {
        Debug.Log("Correct");
        puzzle1solved.StartDialogue();
    }
    
    public void TriggerPuzzle3()
    {
        clock.StopAtTime(9, 35);
        // TBA
    }
}

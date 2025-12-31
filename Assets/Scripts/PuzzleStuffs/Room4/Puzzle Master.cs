using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [SerializeField] DialogueMaker puzzle1Start;
    [SerializeField] PuzzleChecker1 puzzle1;
    [SerializeField] PuzzleChecker2 puzzle2;
    [SerializeField] PuzzleChecker3 puzzle3;

    bool triggered;
    bool P3started;

    void Update()
    {
        if (puzzle2.isCompleted() && triggered == false)
        {
            triggered = true;
            puzzle3.TriggerPuzzle3();
        }
        if (puzzle3.getPuzzleState() && P3started == false)
        {
            P3started = true;
        }
    }

    public void clockDialogue()
    {
        puzzle3.clockDialogueCheck(puzzle2.isCompleted());
    }


    public void cabinetDialogue()
    {
        puzzle3.cabinetDialogue(P3started);
    }
    
    public void TriggerPuzzle1()
    {
        puzzle1Start.StartDialogue();
    }
}

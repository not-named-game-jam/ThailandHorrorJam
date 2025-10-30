using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleChecker3 : MonoBehaviour
{
    [SerializeField] Clock clock;
    [SerializeField] DialogueMaker clockBefore;
    [SerializeField] DialogueMaker clockAfter;
    [SerializeField] LockManager lockManager;
    [SerializeField] DialogueMaker cabinetBefore;
    [SerializeField] DialogueMaker cabinetAfter;
    bool puzzleStarted;

    void Awake()
    {
        lockManager.answer = "0935";
        puzzleStarted = false;
    }

    public void TriggerPuzzle3()
    {
        clock.StopAtTime(09, 35);
        lockManager.answer = "0935";
        puzzleStarted = true;
    }

    public void clockDialogueCheck(bool P2Solved)
    {
        if (P2Solved)
        {
            clockAfter.StartDialogue();
        }
        else
        {
            clockBefore.StartDialogue();
        }
    }

    public void cabinetDialogue()
    {
        if (puzzleStarted)
        {
            cabinetAfter.StartDialogue();
        }
        else
        {
            cabinetBefore.StartDialogue();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleChecker : MonoBehaviour
{
    [SerializeField] DialogueMaker triggerDialogue;
    [SerializeField] List<DragableButton> dragPuzzle;
    int puzzleChecker;
    int checkerCount;

    bool dialoguePlayed;

    void Start()
    {
        checkerCount = 0;
        puzzleChecker = dragPuzzle.Count;
    }
    
    void Update()
    {
        if(checkerCount >= puzzleChecker && !dialoguePlayed)
        {
            dialoguePlayed = true;
            triggerDialogue.StartDialogue();
        }

        checkerCount = 0;
        foreach(DragableButton drag in dragPuzzle)
        {
            if(drag.snapped) checkerCount += 1;
        }
    }
}

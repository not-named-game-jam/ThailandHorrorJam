using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleChecker2 : MonoBehaviour
{
    [SerializeField] DialogueMaker triggerDialogue;
    [SerializeField] List<DragableButton> dragPuzzle;
    int puzzleChecker;
    int checkerCount;

    bool isDone;

    void Start()
    {
        checkerCount = 0;
        puzzleChecker = dragPuzzle.Count;
        isDone = false;
    }

    void Update()
    {
        if (checkerCount >= puzzleChecker && !isDone)
        {
            isDone = true;
            gameObject.SetActive(false);
            triggerDialogue.StartDialogue();
        }

        checkerCount = 0;
        foreach (DragableButton drag in dragPuzzle)
        {
            if (drag.snapped) checkerCount += 1;
        }
    }
    
    public bool isCompleted()
    {
        return isDone;
    }
}

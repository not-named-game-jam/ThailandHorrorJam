using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [SerializeField] PuzzleChecker1 puzzle1;
    [SerializeField] PuzzleChecker2 puzzle2;
    [SerializeField] PuzzleChecker3 puzzle3;

    public void clockDialogue()
    {
        puzzle3.clockDialogueCheck(puzzle2.isCompleted());
    }
}

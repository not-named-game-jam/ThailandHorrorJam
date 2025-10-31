using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackboardChecker : MonoBehaviour
{
    [SerializeField] PuzzleChecker1 puzzleChecker1;
    [SerializeField] IndexSender indexSender;
    public void PuzzleCheck()
    {
        Debug.Log("igotpressed");
        int index = indexSender.getIndex();
        if (index == 4)
        {
            puzzleChecker1.Puzzle1Correct(index);
        }
        else
        {
            puzzleChecker1.Puzzle1Wrong(index);
        }
    }
}
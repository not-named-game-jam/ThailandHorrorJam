using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxPuzzleManager : MonoBehaviour
{
    //[SerializeField] int correctNumberCode;
    [SerializeField] DialogueMaker triggerDialogue;
    [SerializeField] List<ScrollSnapWheel> allDigits;

    private bool isDigit1Correct;
    private bool isDigit2Correct;
    private bool isDigit3Correct;

    private string correctCode;

    private bool isPuzzleComplete;

    public static BoxPuzzleManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        foreach (ScrollSnapWheel wheel in allDigits)
        {
            correctCode += wheel.GetCorrectNumberCode().ToString();
        }
        Debug.Log($"Correct Code is: {correctCode}");
    }

    public void CheckCode()
    {

        foreach (ScrollSnapWheel wheel in allDigits)
        {
            if (!wheel.GetIsSelectedNumberCorrect())
            {
                Debug.Log("Wrong sequence!");
                return;
            }
        }

        triggerDialogue.StartDialogue();
        gameObject.SetActive(false);
        
    }




}

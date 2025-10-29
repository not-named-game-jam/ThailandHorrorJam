using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxPuzzleManager : MonoBehaviour
{
    //[SerializeField] int correctNumberCode;
    [SerializeField] DialogueMaker triggerDialogue;
    [SerializeField] List<ScrollSnapWheel> allDigits;
    [SerializeField] GameObject unopenedBoxBackground;
    [SerializeField] GameObject horizontalDigits;
    [SerializeField] GameObject openedBoxBackground;
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
        openedBoxBackground.SetActive(false);
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

        StartCoroutine(Finish());

    }

    IEnumerator Finish()
    {
        unopenedBoxBackground.SetActive(false);
        horizontalDigits.SetActive(false);
        openedBoxBackground.SetActive(true);

        yield return new WaitForSeconds(2f);

        RoomManager instance = RoomManager.instance;
        instance.PlayDialogue(instance.GetWinningDialogue());

        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f; // make invisible
        cg.interactable = false; // disable clicks
        cg.blocksRaycasts = false;  // stop blocking other UI

    }




}

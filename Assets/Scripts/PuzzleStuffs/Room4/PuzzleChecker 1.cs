using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleChecker1 : MonoBehaviour
{
    [SerializeField] DialogueMaker screech;
    [SerializeField] DialogueMaker puzzle1Failure;
    [SerializeField] DialogueMaker puzzle1Solved;
    [SerializeField] GameObject circleMarker;
    [SerializeField] GameObject[] scribbles;
    [SerializeField] GameObject[] dateButtons;

    public int failcount;

    void Awake()
    {
        failcount = 0;
    }


    public void Puzzle1Wrong(int index)
    {
        if (!circleMarker.activeSelf) circleMarker.SetActive(true);
        failcount++;
        Debug.Log("Wrong \n Fail Count : " + failcount);
        if (failcount > 0 && failcount <= 6)
        {
            // Plays screeching noise.
            screech.StartDialogue();
            // Shows scribbling for each failure.
            scribbles[failcount - 1].SetActive(!scribbles[failcount - 1].activeSelf);
            circleMarker.transform.position = dateButtons[index].transform.position;
            if (failcount == 3)
            {
                // Two girls yap about their predicament.
                screech.StartDialogue();
                puzzle1Failure.StartDialogue();
            }
        }
    }

    public void Puzzle1Correct(int index)
    {
        if (!circleMarker.activeSelf) circleMarker.SetActive(true);
        Debug.Log("Correct");
        // Deactivates every unused butttons.
        foreach (GameObject button in dateButtons)
        {
            if (button.activeSelf)
            {
                button.SetActive(false);
            }
        }
        circleMarker.transform.position = dateButtons[index].transform.position;

        gameObject.SetActive(false);
        puzzle1Solved.StartDialogue();
    }
}

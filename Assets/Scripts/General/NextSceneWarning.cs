using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NextSceneWarning : MonoBehaviour
{
    [SerializeField] private GameObject nextSceneWarning;
    [SerializeField] private Button continueButton;
    [SerializeField] private DialogueMaker[] continueDialouge;

    void OnEnable()
    {
        CheckOverlayActive.IsOverlayActive = true;
        GameObject allobject = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.activeInHierarchy && obj.name.StartsWith("----") && obj.name != "-------------------" && obj.name != "----Camera----" && obj.name != "----Game----" && obj.name != "----System----");
        string whichScene = allobject.name.Split("----")[1].Replace("Scene","");
        Debug.Log("I got"+whichScene);
        int.TryParse(whichScene, out int newscenenum);
        int indexnum = newscenenum-1;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => continueDialouge[indexnum].StartDialogue());
        continueButton.onClick.AddListener(() => nextSceneWarning.SetActive(false));
    }
    void OnDisable()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
        }
        CheckOverlayActive.IsOverlayActive = false;
    }
}

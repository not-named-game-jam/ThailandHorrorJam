using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NextSceneWarning : MonoBehaviour
{
    [SerializeField] private GameObject nextSceneWarning;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private DialogueMaker[] continueDialouge;
    private Coroutine nextCoroutine;

    void OnEnable()
    {
        SoundManager.instance?.PlaySfx("lockValid");
        if (nextCoroutine != null)
        {
            nextCoroutine = null;
        }
        CheckOverlayActive.IsOverlayActive = true;
        nextCoroutine = StartCoroutine(TonextScene());
    }
    void OnDisable()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
        }
        CheckOverlayActive.IsOverlayActive = false;
    }

    private IEnumerator TonextScene()
    {
        yield return new WaitForEndOfFrame();
        CheckOverlayActive.IsOverlayActive = true;
        GameObject allobject = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .FirstOrDefault(obj => 
                obj != null &&
                obj.activeInHierarchy && 
                obj.name.StartsWith("----") && 
                obj.name != "-------------------" && 
                obj.name != "----Camera----" && 
                obj.name != "----Game----" && 
                obj.name != "----System----");
        if(allobject == null)
        {
            Debug.Log("no active scene");
            yield break;
        }
        string whichScene = allobject.name.Split("----")[1].Replace("Scene","");
        Debug.Log("I got"+whichScene);
        int.TryParse(whichScene, out int newscenenum);
        int indexnum = newscenenum-1;
        DialogueMaker targetDialogue = continueDialouge[indexnum];
        continueButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => SoundManager.instance?.PlaySfx("PINClick"));
        cancelButton.onClick.AddListener(() => nextSceneWarning.SetActive(false));
        continueButton.onClick.AddListener(() => SoundManager.instance?.PlaySfx("PINClick"));
        continueButton.onClick.AddListener(() => nextSceneWarning.SetActive(false));
        continueButton.onClick.AddListener(() => targetDialogue.StartDialogue());
    }
}

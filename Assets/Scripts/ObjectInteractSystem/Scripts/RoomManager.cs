using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    // public enum RoomManagerStatus
    // {
    //     PlayingDialogue, // any dialogue
    //     PlayingCutscene,
    //     CompletedPuzzle
    // }
    public static RoomManager instance;
    [SerializeField] DialogueMaker startingDialogue; // the immersive one: "Find the Key"
    [SerializeField] DialogueMaker winningDialogue;
    // [SerializeField] DialogueMaker darkCutsceneDialogue;
    [SerializeField] Slider timerSlider;
    // RoomManagerStatus currentRoomManagerStatus;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startingDialogue.StartDialogue();
    }

    public void LoadNextScene()
    {
        Debug.Log("loading nex scene");
        // Uncomment this: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayDialogue(DialogueMaker dialogueMaker)
    {
        dialogueMaker.StartDialogue();
    }

    public void PlayDialogue(DialogueMaker dialogueMaker, float afterDelayTime)
    {
        StartCoroutine(PlayDialogueCoroutine(dialogueMaker, afterDelayTime));
    }

    IEnumerator PlayDialogueCoroutine(DialogueMaker dialogueMaker, float afterDelayTime)
    {
        dialogueMaker.StartDialogue();
        yield return new WaitForSeconds(afterDelayTime);
    }

    public DialogueMaker GetWinningDialogue()
    {
        return winningDialogue;
    }

    // public DialogueMaker GetDarkCutsceneDialogue()
    // {
    //     return darkCutsceneDialogue;
    // }
}

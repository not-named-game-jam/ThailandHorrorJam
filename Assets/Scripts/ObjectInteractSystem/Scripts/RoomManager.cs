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
    [SerializeField] Image fadeOutImageToNextScene;
    [SerializeField, Range(0.5f, 3f)] float fadeOutToNextSceneSpeed;
    // RoomManagerStatus currentRoomManagerStatus;

    bool isFadingOut;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startingDialogue.StartDialogue();
    }

    void Update()
    {
        if (isFadingOut)
        {
            Debug.Log("Fading");
            Color c = fadeOutImageToNextScene.color;
            c.a += Time.deltaTime * fadeOutToNextSceneSpeed;
            fadeOutImageToNextScene.color = c;

            if (fadeOutImageToNextScene.color.a >= 1f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }



    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextSceneWithFadeToDark()
    {
        isFadingOut = true;
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

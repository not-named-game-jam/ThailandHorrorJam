using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneChanger : MonoBehaviour
{
    private Coroutine changeSceneCorutine;
    [SerializeField] DialogueMaker nextSceneDialouge;
    void OnEnable()
    {
        if (changeSceneCorutine != null)
        {
            changeSceneCorutine = null;
        }
        changeSceneCorutine = StartCoroutine(ChangeScene());
    }
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.1f);
        nextSceneDialouge.StartDialogue();
        gameObject.SetActive(false);
    }
}

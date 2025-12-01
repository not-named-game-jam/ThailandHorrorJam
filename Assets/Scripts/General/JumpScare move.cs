using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScaremove : MonoBehaviour
{
    [SerializeField] GameObject jump1;
    [SerializeField] GameObject jump2;
    [SerializeField] GameObject immersivedialogue;
    [SerializeField] GameObject characterdialogue;
    [SerializeField] GameObject justtextdialogue;

    void Update()
    {
        StartCoroutine(JumpMove(jump1, jump2));
    }

    IEnumerator JumpMove(GameObject jump1 , GameObject jump2)
    {
        while (true)
        {
            bool isDialogueActive = immersivedialogue.activeSelf || characterdialogue.activeSelf || justtextdialogue.activeSelf;
            Debug.Log(isDialogueActive);
            if (isDialogueActive)
            {
                Debug.Log("Stop Jumpscare!");
                jump1.SetActive(false);
                jump2.SetActive(false);
                yield return null;
            }

            if (!isDialogueActive)
            {
                Debug.Log("Run Jumpscare!");
                jump1.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                jump1.SetActive(false);
                jump2.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                jump2.SetActive(false);
            }
            
        }

    }
}

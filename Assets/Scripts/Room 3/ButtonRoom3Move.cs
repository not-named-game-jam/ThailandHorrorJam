using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRoom3Move : MonoBehaviour
{
    [SerializeField] GameObject buttonnotpush;
    [SerializeField] GameObject buttonpushed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ButtonRoom3(buttonnotpush, buttonpushed));
        }
    }

    IEnumerator ButtonRoom3(GameObject buttonnotpush, GameObject buttonpushed)
    {
            buttonnotpush.SetActive(false);
            buttonpushed.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            buttonpushed.SetActive(false);
            buttonnotpush.SetActive(true);
    }
}


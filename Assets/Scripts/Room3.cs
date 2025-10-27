using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room3 : MonoBehaviour
{
    public bool isRikaCalm = false;
    public int clickCount = 0;
    [SerializeField] private Button calmButton;
    [SerializeField] private GameObject spamText;

    void Start()
    {
        calmButton.gameObject.SetActive(false);
        spamText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (clickCount == 30)
        {
            isRikaCalm = true;
            calmButton.gameObject.SetActive(false);
            spamText.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        clickCount++;
    }

}

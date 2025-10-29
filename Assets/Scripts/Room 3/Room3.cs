using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Room3 : MonoBehaviour
{
    public bool isRikaCalm = false;
    public int clickCount = 0;
    [SerializeField] private Button calmButton;
    [SerializeField] private GameObject spamText;
    [SerializeField] DialogueMaker triggerAftercalm;

    private TextMeshProUGUI textD;
    private Image buttonImage;
    private Coroutine flickerCoroutine;

    void Start()
    {
        textD = spamText.GetComponent<TextMeshProUGUI>();
        buttonImage = calmButton.GetComponent<Image>();
        flickerCoroutine = StartCoroutine(SpamTextFlicker());
    }

    void Update()
    {
        if (clickCount == 30 && !isRikaCalm)
        {
            isRikaCalm = true;
            //calmButton.gameObject.SetActive(false);
            //spamText.gameObject.SetActive(false);
            if (flickerCoroutine != null)
            {
                StopCoroutine(SpamTextFlicker());
            }
            triggerAftercalm.StartDialogue();
            Settozero();
        }
        

    }

    public void OnButtonClick()
    {
        clickCount++;
        StartCoroutine(ButtonFlicker());
    }

    private void Settozero()
    {
        clickCount = 0;
    }

    IEnumerator SpamTextFlicker()
    {
        while (true)
        {
            textD.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            textD.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ButtonFlicker()
    {
        buttonImage.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        buttonImage.color = Color.white;
    }

}

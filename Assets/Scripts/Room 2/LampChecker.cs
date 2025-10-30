using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LampChecker : MonoBehaviour
{
    [Header("Lamp Settings")]
    [SerializeField] private List<Button> lampButtons;        
    [SerializeField] private List<GameObject> lampLights;     
    [SerializeField] private TMP_Text displayText;            

    [Header("Dialogue")]
    [SerializeField] private DialogueMaker jigsawDialogue;   

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip toggleSound;

    private bool[] lampStates = new bool[3];
    private bool gotJigsaw = false;

    void Update()
    {
        // ปิดไฟทุกดวงตอนเริ่ม
        // for (int i = 0; i < lampLights.Count; i++)
        //     if (lampLights[i] != null)
        //         lampLights[i].enabled = false;

        // ผูกปุ่มแต่ละโคม
        for (int i = 0; i < lampButtons.Count; i++)
        {
            int index = i; 
            lampButtons[i].onClick.AddListener(() => OnLampPressed(index));
        }

        UpdateDisplay();
    }

    private void OnLampPressed(int index)
    {
        // โคม 3 ครั้งแรกไม่เปิดแสง
        if (index == 2 && !gotJigsaw)
        {
            gotJigsaw = true;
            if (jigsawDialogue != null)
                jigsawDialogue.StartDialogue();
            
            PlaySound(toggleSound);
            return;
        }

        lampStates[index] = !lampStates[index];
        UpdateLampVisual(index);

        PlaySound(toggleSound);
        UpdateDisplay();
    }

    private void UpdateLampVisual(int index)
    {
        if (lampLights[index] != null)
        {
            lampLights[index].SetActive(!lampLights[index].activeSelf);
            // lampLights[index].enabled = lampStates[index];
            // lampLights[index].intensity = lampStates[index] ? lightIntensity : 0f;
        }
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = GetBinaryCode();
    }

    public string GetBinaryCode()
    {
        string code = "";
        for (int i = 0; i < lampStates.Length; i++)
            code += lampStates[i] ? "1" : "0";
        return code;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
            audioSource.PlayOneShot(clip);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TVCodeChecker : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text[] digitsText; 
    [SerializeField] private Button[] upButtons;
    [SerializeField] private Button[] downButtons;
    [SerializeField] private Button enterButton;
    [SerializeField] private Button closeButton;

    [Header("Dialogue")]
    [SerializeField] private DialogueMaker correctDialogue;   
    [SerializeField] private DialogueMaker wrongDialogue;     

    [Header("Lamp Reference")]
    [SerializeField] private LampChecker lampChecker;         

    private int[] digits = new int[3];
    private int correctCode = 0;
    private bool isUnlocked = false;

    /// <summary>
    /// เปิด TV panel → รีเซ็ตตัวเลข 0-0-0
    /// </summary>
    public void OpenTVPanel()
    {
        for (int i = 0; i < 3; i++)
        {
            digits[i] = 0;
            UpdateDigitText(i);
        }

        isUnlocked = false;
        gameObject.SetActive(true); // เปิด panel
    }

    void Start()
    {
        // ผูกปุ่ม ↑↓
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            upButtons[i].onClick.AddListener(() => ChangeDigit(index, 1));
            downButtons[i].onClick.AddListener(() => ChangeDigit(index, -1));
            UpdateDigitText(i);
        }

        if (enterButton != null)
            enterButton.onClick.AddListener(CheckPassword);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }

    void ChangeDigit(int index, int change)
    {
        if (isUnlocked) return;

        digits[index] = (digits[index] + change + 10) % 10;
        UpdateDigitText(index);
    }

    void UpdateDigitText(int index)
    {
        digitsText[index].text = digits[index].ToString();
    }

    /// <summary>
    /// ตรวจสอบรหัส
    /// ดึงรหัส Lamp ล่าสุดจาก LampChecker
    /// </summary>
    void CheckPassword()
    {
        if (isUnlocked) return;

        // ดึงรหัส Lamp ล่าสุด
        if (lampChecker != null)
        {
            string binary = lampChecker.GetBinaryCode();
            correctCode = Convert.ToInt32(binary, 2);
            Debug.Log("TV correct code = " + correctCode.ToString("D3"));
        }

        int currentCode = digits[0] * 100 + digits[1] * 10 + digits[2];

        if (currentCode == correctCode)
        {
            isUnlocked = true;
            Debug.Log("✅ รหัสถูกต้อง!");

            if (correctDialogue != null)
                correctDialogue.StartDialogue();
        }
        else
        {
            Debug.Log("❌ รหัสผิด!");
            if (wrongDialogue != null)
                wrongDialogue.StartDialogue();
        }
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

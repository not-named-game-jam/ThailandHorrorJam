using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SafeCodeChecker : MonoBehaviour
{
    [Header("Safe Code Settings")]
    [SerializeField] private string correctCode = "6241";        // ✅ รหัสที่ถูกต้อง
    [SerializeField] private TMP_Text displayText;               // ช่องแสดงรหัสที่พิมพ์
    [SerializeField] private List<Button> numberButtons;         // ปุ่มตัวเลข 0–9
    [SerializeField] private Button clearButton;                 // ปุ่มล้าง
    [SerializeField] private Button enterButton;                 // ปุ่มยืนยัน
    [SerializeField] private Button closeButton;                 // ปุ่มปิด
    [SerializeField] private SafePanelController safePanel;      // ตัวควบคุม Panel

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip errorSound;

    [Header("Dialogue")]
    [SerializeField] private DialogueMaker correctDialogue;      // Dialogue เมื่อรหัสถูก
    [SerializeField] private DialogueMaker wrongDialogue;        // Dialogue เมื่อรหัสผิด

    private string currentInput = "";
    private bool isUnlocked = false;

    void Start()
    {
        if (displayText != null)
            displayText.text = "";

        // 🧩 ผูกปุ่มตัวเลข
        foreach (Button btn in numberButtons)
        {
            string number = btn.GetComponentInChildren<TMP_Text>().text;
            btn.onClick.AddListener(() => OnNumberPressed(number));
        }

        // 🔘 ผูกปุ่ม Clear / Enter / Close
        if (clearButton != null)
            clearButton.onClick.AddListener(OnClearPressed);

        if (enterButton != null)
            enterButton.onClick.AddListener(OnEnterPressed);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);
    }

    private void OnNumberPressed(string number)
    {
        if (isUnlocked) return;

        PlaySound(buttonSound);

        if (currentInput.Length >= correctCode.Length)
            return; // ห้ามเกินจำนวนหลัก

        currentInput += number;
        displayText.text = currentInput;
    }

    private void OnClearPressed()
    {
        if (isUnlocked) return;

        PlaySound(buttonSound);
        currentInput = "";
        displayText.text = "";
    }

    private void OnEnterPressed()
    {
        if (isUnlocked) return;

        PlaySound(buttonSound);

        if (currentInput == correctCode)
        {
            StartCoroutine(UnlockSafe());
        }
        else
        {
            WrongCode();
        }
    }

    private IEnumerator UnlockSafe()
    {
        isUnlocked = true;
        displayText.text = "✅ UNLOCKED";
        PlaySound(openSound);

        yield return new WaitForSeconds(1f);

        if (correctDialogue != null)
            correctDialogue.StartDialogue();

        // ปิดตู้เซฟหลังเปิดสำเร็จ
        yield return new WaitForSeconds(1f);
        if (safePanel != null)
            safePanel.CloseSafe();
    }

    private void WrongCode()
    {
        displayText.text = "❌ WRONG";
        PlaySound(errorSound);

        if (wrongDialogue != null)
            wrongDialogue.StartDialogue();

        // ล้างรหัสหลังจากใส่ผิด
        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        currentInput = "";
        displayText.text = "";
    }

    private void ClosePanel()
    {
        if (safePanel != null)
            safePanel.CloseSafe();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
    public bool IsUnlocked => isUnlocked;
}

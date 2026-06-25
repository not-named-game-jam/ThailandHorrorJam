using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class JigsawBoardController : MonoBehaviour
{
    [Header("Board Settings")]
    [SerializeField] private GameObject jigsawBoardPanel;
    [SerializeField] private CanvasGroup jigsawCanvasGroup;
    //[SerializeField] private float fadeDuration = 0.5f;

    [Header("Pieces")]
    [SerializeField] private List<GameObject> jigsawPieces; // ทั้งหมด 6 ชิ้น
    private int collectedPieces = 0;
    public bool isPlayableeee = false;
    private bool firstTimeOpened = false;

    [Header("Progress UI")]
    [SerializeField] private GameObject progressPanel; // กล่อง UI แสดง progress
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;

    [Header("Dialogue (First Time)")]
    [SerializeField] private DialogueMaker introDialogue; // Dialogue ตอนเปิดครั้งแรก

    void Start()
    {
        jigsawBoardPanel.SetActive(false);

        // ซ่อนทุกชิ้น
        foreach (var piece in jigsawPieces)
        {
            if (piece != null) piece.SetActive(false);
        }

        // ซ่อน progress ตอนเริ่ม
        if (progressPanel != null)
            progressPanel.SetActive(false);
    }

    public void OpenJigsaw()
    {
        // ถ้ายังไม่มีชิ้นเลย ไม่ให้เปิดเล่น
        if (collectedPieces == 0 && !firstTimeOpened)
        {
            // เปิดครั้งแรก → แสดง dialogue แนะนำ
            firstTimeOpened = true;
            if (introDialogue != null)
            {
                introDialogue.StartDialogue();
            }
            else
            {
                Debug.LogWarning("Intro Dialogue not assigned!");
            }
        }

        // แสดงบอร์ด
        jigsawBoardPanel.SetActive(true);
        jigsawCanvasGroup.alpha = 1f;

        // แสดง progress bar
        if (progressPanel != null)
        {
            progressPanel.SetActive(true);
            UpdateProgressUI();
        }
    }

    public void CloseJigsaw()
    {
        jigsawBoardPanel.SetActive(false);
    }

    public void AddPiece(int pieceIndex)
    {
        if (pieceIndex < 0 || pieceIndex >= jigsawPieces.Count)
        {
            Debug.LogWarning("Invalid jigsaw index!");
            return;
        }

        if (!jigsawPieces[pieceIndex].activeSelf)
        {
            jigsawPieces[pieceIndex].SetActive(true);
            collectedPieces++;
            Debug.Log($"Collected piece #{pieceIndex + 1}");

            UpdateProgressUI();

            if (collectedPieces == jigsawPieces.Count)
            {
                jigsawBoardPanel.SetActive(false);
                isPlayableeee = true;
                Debug.Log("All pieces collected! Board is now playable.");
            }
        }
    }

    private void UpdateProgressUI()
    {
        if (progressSlider != null)
        {
            progressSlider.maxValue = jigsawPieces.Count;
            progressSlider.value = collectedPieces;
        }

        if (progressText != null)
        {
            if (collectedPieces < jigsawPieces.Count)
                progressText.text = $"Collected: {collectedPieces}/{jigsawPieces.Count}";
            else
                progressText.text = "All pieces collected!";
        }
    }
}

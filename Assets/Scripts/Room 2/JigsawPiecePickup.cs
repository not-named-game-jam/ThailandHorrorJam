using UnityEngine;
using System.Collections;

public class JigsawPiecePickup : MonoBehaviour
{
    [SerializeField] private int pieceIndex;
    [SerializeField] private DialogueMaker storyDialogue;
    [SerializeField] private DialogueMaker receiveDialogue;
    [SerializeField] private SafeCodeChecker safeChecker; // ✅ อ้างถึง SafeCodeChecker

    private bool collected = false;
    private bool isProcessing = false;

    public void OnClickPickup()
    {
        if (isProcessing) return;
        isProcessing = true;

        // ❌ ถ้ายังไม่ปลดล็อกตู้เซฟ
        if (safeChecker != null && !safeChecker.IsUnlocked)
        {
            Debug.Log("Safe not unlocked yet!");
            // อาจให้พูด Dialogue สั้นๆ เช่น “ต้องปลดล็อกก่อน”
            isProcessing = false;
            return;
        }

        if (!collected)
            StartCoroutine(PlayStoryThenReceive());
        else
            StartCoroutine(PlayStoryOnly());
    }

    private IEnumerator PlayStoryThenReceive()
    {
        if (storyDialogue != null)
            storyDialogue.StartDialogue();

        yield return new WaitUntil(() => !DialogueSystem.instance.IsActive);

        collected = true;
        Debug.Log($"Picked up piece {pieceIndex + 1}");

        if (receiveDialogue != null)
            receiveDialogue.StartDialogue();

        yield return new WaitUntil(() => !DialogueSystem.instance.IsActive);

        FindObjectOfType<JigsawBoardController>().AddPiece(pieceIndex);
        isProcessing = false;
    }

    private IEnumerator PlayStoryOnly()
    {
        if (storyDialogue != null)
            storyDialogue.StartDialogue();

        yield return new WaitUntil(() => !DialogueSystem.instance.IsActive);
        isProcessing = false;
    }
}

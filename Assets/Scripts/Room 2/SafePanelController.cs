using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SafePanelController : MonoBehaviour
{
    [SerializeField] private GameObject safePanel; // หน้า Panel ของตู้เซฟ
    [SerializeField] private CanvasGroup safeCanvasGroup; // ใช้สำหรับ Fade
    [SerializeField] private float fadeDuration = 0.5f; // ระยะเวลา fade

    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        // ปิด Panel ไว้ก่อนเสมอ
        if (safePanel != null)
        {
            safePanel.SetActive(false);
        }
    }

    // ฟังก์ชันสำหรับเปิดหน้า Safe
    public void OpenSafe()
    {
        if (isOpening || (safePanel != null && safePanel.activeSelf))
            return; // ป้องกันกดซ้ำ

        StartCoroutine(FadeIn());
    }

    // ฟังก์ชันสำหรับปิดหน้า Safe
    public void CloseSafe()
    {
        if (isClosing || (safePanel != null && !safePanel.activeSelf))
            return; // ป้องกันกดซ้ำ

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        isOpening = true;
        safePanel.SetActive(true);
        safeCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            safeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        safeCanvasGroup.alpha = 1f;
        isOpening = false;
    }

    private IEnumerator FadeOut()
    {
        isClosing = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            safeCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            yield return null;
        }

        safeCanvasGroup.alpha = 0f;
        safePanel.SetActive(false);
        isClosing = false;
    }
}

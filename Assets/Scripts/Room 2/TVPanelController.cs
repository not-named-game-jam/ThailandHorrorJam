using UnityEngine;
using System.Collections;

public class TVPanelController : MonoBehaviour
{
    [SerializeField] private GameObject tvPanel; // หน้า Panel ของ TV
    [SerializeField] private CanvasGroup tvCanvasGroup; // ใช้สำหรับ Fade
    [SerializeField] private float fadeDuration = 0.5f; // ระยะเวลา fade

    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        // ปิด Panel ไว้ก่อนเสมอ
        if (tvPanel != null)
        {
            tvPanel.SetActive(false);
        }
    }

    // ฟังก์ชันสำหรับเปิดหน้า TV
    public void OpenTV()
    {
        if (isOpening || (tvPanel != null && tvPanel.activeSelf))
            return; // ป้องกันกดซ้ำ

        StartCoroutine(FadeIn());
    }

    // ฟังก์ชันสำหรับปิดหน้า TV
    public void CloseTV()
    {
        if (isClosing || (tvPanel != null && !tvPanel.activeSelf))
            return; // ป้องกันกดซ้ำ

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        isOpening = true;
        tvPanel.SetActive(true);
        tvCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tvCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        tvCanvasGroup.alpha = 1f;
        isOpening = false;
    }

    private IEnumerator FadeOut()
    {
        isClosing = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            tvCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            yield return null;
        }

        tvCanvasGroup.alpha = 0f;
        tvPanel.SetActive(false);
        isClosing = false;
    }
}

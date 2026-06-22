using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextForButton : MonoBehaviour
{
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] Transform parentObject;
    [Header("Text")]
    [SerializeField] private List<string> randomTexts = new List<string>()
    {
        "Locked...", "Maybe you missed something." , "That's hurt." , "Observe more." , ". . ."
    };
    private Coroutine floatingText;

    private bool canclick = true;
    private GameObject currentText;

    public void SpawnRandomText(Vector3 position)
    {
        if (!canclick) return;
        if (floatingText != null)
        {
            floatingText = null;
        }
        if (randomTexts.Count == 0 || floatingTextPrefab == null || randomTexts == null) return;

        int randomIndex = Random.Range(0, randomTexts.Count);
        string choosenText = randomTexts[randomIndex];
        floatingText = StartCoroutine(Floatingtext(position, choosenText));
    }

    private IEnumerator Floatingtext(Vector3 position, string mytext)
    {
        canclick = false;
        Transform parentTransform;
        if (parentObject != null)
        {
            parentTransform = parentObject;
        }
        else
        {
            parentTransform = transform;
        }
        currentText = Instantiate(floatingTextPrefab,parentTransform);
        currentText.transform.localScale = Vector3.one;
        currentText.transform.position = position+ new Vector3(0,1f,0);

        TextMeshProUGUI prefabtext = currentText.GetComponentInChildren<TextMeshProUGUI>();
        if (prefabtext != null) prefabtext.text = mytext;
        
        CanvasGroup canvasGroup = currentText.GetComponent<CanvasGroup>();
        Vector3 startpos = currentText.transform.position;
        Vector3 endpos = startpos + new Vector3(0,3f,0);
        

        float elapsed = 0f;
        float duration = 1.8f;

        while (elapsed < duration)
        {
            if (currentText == null) yield break;
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            currentText.transform.position = Vector3.Lerp(startpos, endpos, progress);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Sin(progress * Mathf.PI);
            }

            yield return null;
        }
        canclick = true;
        floatingText = null;
        if(currentText != null)
        {
            Destroy(currentText);
        }
    }
    private void OnDisable()
    {
        if (currentText != null)
        {
            Destroy(currentText);
        }
        canclick = true;
        floatingText = null;
    }
}

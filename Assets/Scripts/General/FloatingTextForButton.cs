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

    private bool canclick = true;

    public void SpawnRandomText(Vector3 position)
    {
        if (!canclick) return;
        if (randomTexts.Count == 0 || floatingTextPrefab == null || randomTexts == null) return;

        int randomIndex = Random.Range(0, randomTexts.Count);
        string choosenText = randomTexts[randomIndex];
        StartCoroutine(Floatingtext(position, choosenText));
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
        GameObject textObject = Instantiate(floatingTextPrefab,parentTransform);
        textObject.transform.localScale = Vector3.one;
        textObject.transform.position = position+ new Vector3(0,1f,0);

        TextMeshProUGUI prefabtext = textObject.GetComponentInChildren<TextMeshProUGUI>();
        if (prefabtext != null) prefabtext.text = mytext;
        
        CanvasGroup canvasGroup = textObject.GetComponent<CanvasGroup>();
        Vector3 startpos = textObject.transform.position;
        Vector3 endpos = startpos + new Vector3(0,3f,0);
        

        float elapsed = 0f;
        float duration = 1.8f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            textObject.transform.position = Vector3.Lerp(startpos, endpos, progress);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Sin(progress * Mathf.PI);
            }

            yield return null;
        }
        canclick = true;
        Destroy(textObject);
    }
}

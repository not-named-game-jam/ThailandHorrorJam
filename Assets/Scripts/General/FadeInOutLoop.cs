using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutLoop : MonoBehaviour
{
    [SerializeField] GameObject loopobj;
    [SerializeField] float Fadeduration;

    void Start()
    {
        StartCoroutine(LoopFade(loopobj));
    }

    public IEnumerator LoopFade(GameObject loopobj)
    {
        
        CanvasGroup canvasGroup = loopobj.GetComponent<CanvasGroup>();
        
        while (true)
        {   
            //Fade In
            yield return StartCoroutine(Fade(canvasGroup, 1f, Fadeduration));
            yield return new WaitForSeconds(0.5f);

            //Fade Out
            yield return StartCoroutine(Fade(canvasGroup, 0f, Fadeduration));
            yield return new WaitForSeconds(0.2f); 
        }
        
    }

    private IEnumerator Fade(CanvasGroup obj , float toalpha , float duration)
    {
        float startAlpha = obj.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            obj.alpha = Mathf.Lerp(startAlpha, toalpha, time / duration);
            yield return null;
        }

        obj.alpha = toalpha;
    }

}

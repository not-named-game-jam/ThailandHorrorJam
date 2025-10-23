using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class FunctionCalls
{
    public enum FunctionType
    {
        None,
        PlaySound,
        PlayMusic,
        StopMusic,
        EnableObject,
        DisableObject,
        FadeIn,
        FadeOut
    }

    public int index;
    public FunctionType functionType;
    public List<string> parameter = new List<string>();

    public void CallFunction(MonoBehaviour monoBehaviour)
    {
        if (functionType == FunctionType.None) return;
        
        switch (functionType)
        {
            case FunctionType.PlaySound:
                if (!IsParameterValid(1)) return;
                Debug.Log("Playing sound: " + parameter[0]);
                SoundManager.instance.PlaySfx(parameter[0]);
                break;

            case FunctionType.PlayMusic:
                if (!IsParameterValid(1)) return;
                Debug.Log("Playing music: " + parameter[0]);
                SoundManager.instance.PlayMusic(parameter[0]);
                break;

            case FunctionType.StopMusic:
                Debug.Log("Stopping music");
                SoundManager.instance.StopMusic();
                break;

            case FunctionType.EnableObject:
                if (!IsParameterValid(1)) return;
                var objToEnable = Resources.FindObjectsOfTypeAll<GameObject>()
                    .FirstOrDefault(x => x.name == parameter[0]);
                if (objToEnable != null) objToEnable.SetActive(true);
                Debug.Log("Enabled object: " + parameter[0]);
                break;

            case FunctionType.DisableObject:
                if (!IsParameterValid(1)) return;
                var objToDisable = Resources.FindObjectsOfTypeAll<GameObject>()
                    .FirstOrDefault(x => x.name == parameter[0]);
                if (objToDisable != null) objToDisable.SetActive(false);
                Debug.Log("Disabled object: " + parameter[0]);
                break;

            case FunctionType.FadeIn:
                if (!IsParameterValid(2)) return;
                var objToFadeIn = Resources.FindObjectsOfTypeAll<GameObject>()
                    .FirstOrDefault(x => x.name == parameter[0]);
                if (objToFadeIn == null || objToFadeIn.GetComponent<CanvasGroup>() == null) return;
                monoBehaviour.StartCoroutine(Fade(objToFadeIn, float.Parse(parameter[1]), 0f, 1f));
                Debug.Log("Faded in object: " + parameter[0]);
                break;

            case FunctionType.FadeOut:
                if (!IsParameterValid(2)) return;
                var objToFadeOut = Resources.FindObjectsOfTypeAll<GameObject>()
                    .FirstOrDefault(x => x.name == parameter[0]);
                if (objToFadeOut == null || objToFadeOut.GetComponent<CanvasGroup>() == null) return;
                monoBehaviour.StartCoroutine(Fade(objToFadeOut, float.Parse(parameter[1]), 1f, 0f));
                Debug.Log("Faded out object: " + parameter[0]);
                break;

            default:
                Debug.LogWarning($"Unknown function name: {functionType}");
                break;
        }
    }

    private bool IsParameterValid(int requiredParams)
    {
        if (parameter.Count < requiredParams) return false;
        for (int i = 0; i < requiredParams; i++)
        {
            if (string.IsNullOrEmpty(parameter[i])) return false;
        }
        return true;
    }

    private IEnumerator Fade(GameObject obj, float duration, float from, float to)
    {
        if (obj == null)
        {
            Debug.LogError("Fade target object is null!");
            yield break;
        }

        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"No CanvasGroup component found on {obj.name}");
            yield break;
        }
        
        float elapsed = 0f;
        canvasGroup.alpha = from;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(from, to, progress);
            
            yield return null;
        }
        
        canvasGroup.alpha = to;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoClickButton : MonoBehaviour
{
    [SerializeField] Button button;
    private Coroutine autoclickbutt;
    void OnEnable()
    {
        if (autoclickbutt != null)
        {
            StopCoroutine(autoclickbutt);
            autoclickbutt = null;
        }
        autoclickbutt = StartCoroutine(AutoClick());
    }
    private IEnumerator AutoClick()
    {
        yield return null;
        if (button != null)
        {
            button.onClick.Invoke();
        }
        gameObject.SetActive(false);
    }
}

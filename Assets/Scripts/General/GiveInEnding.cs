using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveInEnding : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(ChangeInEnding());
        
    }
    private IEnumerator ChangeInEnding()
    {
        yield return null;
        InEndingtrue.InEnding = !InEndingtrue.InEnding;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    [SerializeField] GameObject TheObject;
    [SerializeField] Transform Objectposition;
    [SerializeField] CameraPanning CameraPanningObject;
    void Update()
    {
        Debug.Log("On");
        Vector2 Objpos = Objectposition.position;
        CameraPanningObject.rb.MovePosition(Objpos);
        StartCoroutine(deactivateself());
    }

    IEnumerator deactivateself()
    {
        yield return new WaitForSeconds(0.1f);
        TheObject.SetActive(false);
    }
}

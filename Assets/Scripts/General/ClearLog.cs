using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLog : MonoBehaviour
{
    void OnEnable()
    {
        TestLog.Instance?.ClearLog();
    }
}

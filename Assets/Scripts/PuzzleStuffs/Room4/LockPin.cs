using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPin : MonoBehaviour
{
    [SerializeField] LockManager lockManager;
    [SerializeField] IndexSender indexSender;

    public void pressPin()
    {
        lockManager.recievePINInput(indexSender.getIndex());
    }
}

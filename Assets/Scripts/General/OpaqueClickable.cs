using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpaqueClickable : MonoBehaviour
{
    [SerializeField] Image Button;
    // Start is called before the first frame update
    void Start()
    {
        Button.alphaHitTestMinimumThreshold = 1f;
    }

}

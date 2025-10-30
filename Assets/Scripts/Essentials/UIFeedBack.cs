using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFeedBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float sizeMult = 1.5f;

    Vector3 startSize;
    bool hovering;

    void Start() 
    {
        startSize = transform.localScale;
    }

    void Update()
    {
        if(hovering) transform.localScale = Vector3.Lerp(transform.localScale, startSize * sizeMult, Time.deltaTime * 3f);
        else transform.localScale = Vector3.Lerp(transform.localScale, startSize, Time.deltaTime * 4f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}

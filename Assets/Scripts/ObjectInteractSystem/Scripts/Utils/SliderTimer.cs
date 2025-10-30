using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] float secondsDuration;
    //[SerializeField] DialogueMaker winDialogue; // open the box successfully


    float currentTimeLeft;
    bool timerStopped;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start()
    {
        Time.timeScale = 1f;
        currentTimeLeft = secondsDuration;
        slider.maxValue = secondsDuration;  
        slider.value = secondsDuration;
    }

    void Update()
    {
        if (timerStopped) return;
        
        currentTimeLeft -= Time.deltaTime;
        slider.value = currentTimeLeft;

        if (slider.value <= 0) EndPuzzle();
    }

    private void EndPuzzle()
    {
        RoomManager.instance.LoadNextSceneWithFadeToDark();
    }


    public void StopSlider()
    {
        timerStopped = true;
    }
}

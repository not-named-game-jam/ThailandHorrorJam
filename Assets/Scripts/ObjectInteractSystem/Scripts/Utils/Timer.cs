using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{


    [Header("Settings")]
    [SerializeField] int duration;


    // Private Fields
    TextMeshProUGUI timerText;
    float timeRemaining;
    bool isCountingDown = true;
    bool timeIsUp = false;

    void Awake()
    {
        if (TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI tmp))
        {
            timerText = tmp;
        }
        else
        {
            Debug.LogWarning("Time TMPro not found!");
        }
    }

    void Start()
    {
        int minutesDuration = (int)(duration / 60);
        int secondsDuration = (int)(duration - minutesDuration * 60);
        displayTime(minutesDuration, secondsDuration);
        timeRemaining = duration;
        beginTimer();
    }

    void Update()
    {
        if (timeIsUp == true)
        {
            return;
        }
        stopTimer();
    }

    public void beginTimer()
    {
        isCountingDown = true;
        Invoke("decrementTimeRemaining", 1f);

    }

    void decrementTimeRemaining()
    {
        if (timeRemaining >= 0.9 && isCountingDown == true) // 0.9 instead of 1 just in case
        {
            timeRemaining--;
            int minutesRemaining = (int)(timeRemaining / 60);
            int secondsRemaining = (int)(timeRemaining - minutesRemaining * 60);
            displayTime(minutesRemaining, secondsRemaining);
            Invoke("decrementTimeRemaining", 1f);
        }
        else
        {
            
            stopTimer();
        }
    }

    private void displayTime(int minutesRemaining, int secondsRemaining)
    {
        string secondsRemainingString = secondsRemaining.ToString();
        string minutesRemainingString = minutesRemaining.ToString();

        if (secondsRemaining < 10)
        {
            secondsRemainingString = "0" + secondsRemaining.ToString();
        }

        if (minutesRemaining < 10)
        {
            minutesRemainingString = "0" + minutesRemaining.ToString();
        }
        timerText.text = minutesRemainingString + ":" + secondsRemainingString;
    }

    void stopTimer()
    {
        if (timeRemaining <= 0)
        {
            Debug.Log("Time is up");
            isCountingDown = false;
            timerText.text = "00:00";
            timeIsUp = true;
        }
    }

    public bool getIsCountingDown()
    {
        return isCountingDown;
    }

    public bool getTimeIsUp()
    {
        return timeIsUp;
    }
}

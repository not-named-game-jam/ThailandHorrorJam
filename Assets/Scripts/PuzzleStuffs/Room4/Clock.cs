using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Clock : MonoBehaviour
{
    public Transform minuteHand;
    public Transform hourHand;
    const float minuteDegree = 6f;
    const float hourDegree = 30f;
    public float spinrate = 600f;
    public float stopLerpSpeed = 120f;
    bool stopped;
    bool isStopping;
    float totalSecondsElapsed;
    float targetSeconds;
    float minuteAngle;
    float hourAngle;
    
    void Start()
    {
        totalSecondsElapsed = 0;
        minuteAngle = 0;
        hourAngle = 0;
        stopped = false;
        isStopping = false;
    }

    void Update()
    {
        if (isStopping)
        {
            totalSecondsElapsed = Mathf.Lerp(totalSecondsElapsed, targetSeconds, Time.deltaTime * stopLerpSpeed);

            if (Mathf.Abs(targetSeconds - totalSecondsElapsed) < 0.1f)
            {
                totalSecondsElapsed = targetSeconds;
                isStopping = false;
                stopped = true;
                Debug.Log("The clock has come to a stop.");
            }
        }
        else if (stopped)
        {
            // Makes the clock do exactly nothing after it stopped.
        }
        else
        {
            totalSecondsElapsed += Time.deltaTime * spinrate;
        }

        float continuousMinute = totalSecondsElapsed / 60f;
        float continuousHour = continuousMinute / 60f;

        minuteAngle = continuousMinute * minuteDegree;
        hourAngle = continuousHour * hourDegree;

        minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
    }

    public void StopAtTime(int hour, int minute)
    {
        float secondsPer12Hours = 43200f;
        float targetCycleSeconds = hour % 12 * 3600f + minute * 60f;

        float currentCycleSeconds = totalSecondsElapsed % secondsPer12Hours;
        float fullCyclesInSeconds = totalSecondsElapsed - currentCycleSeconds;

        targetSeconds = fullCyclesInSeconds + targetCycleSeconds;

        if (targetSeconds <= totalSecondsElapsed)
        {
            targetSeconds += secondsPer12Hours;
        }

        isStopping = true;
        stopped = false;
    }

    public void ResumeTime()
    {
        stopped = false;
        isStopping = false;
    }
}

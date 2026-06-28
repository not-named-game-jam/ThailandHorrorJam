using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ESCbutton : MonoBehaviour
{
    [SerializeField] GameObject tint;
    [SerializeField] GameObject continueobject;
    [SerializeField] GameObject settingobject;
    [SerializeField] Button continuebutton;
    [SerializeField] Button settingbutton;
    [SerializeField] Button backbutton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CheckOverlayActive.IsOverlayActive && !DialogueSystem.instance.IsActive && !InEndingtrue.InEnding)
        {
            SoundManager.instance?.PlaySfx("KeyPickUp");
            UnityAction PINClicklistener = () => SoundManager.instance?.PlaySfx("PINClick");
            tint.SetActive(true);
            continuebutton.onClick.RemoveListener(PINClicklistener);
            continuebutton.onClick.AddListener(PINClicklistener);
            settingbutton.onClick.RemoveListener(PINClicklistener);
            settingbutton.onClick.AddListener(PINClicklistener);
            backbutton.onClick.RemoveListener(PINClicklistener);
            backbutton.onClick.AddListener(PINClicklistener);
            continueobject.SetActive(true);
            settingobject.SetActive(true);
            CheckOverlayActive.IsOverlayActive = true;
        }
    }
}

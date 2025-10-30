using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    [SerializeField] DialogueMaker invalid;
    [SerializeField] DialogueMaker unlocked;
    [SerializeField] TextMeshProUGUI displayPIN;

    public string answer = "0935";
    string code = "";

    public void recievePINInput(int pinNum)
    {
        SoundManager.instance.PlaySfx("PINClick");
        if (pinNum >= 0 && pinNum <= 9 && code.Length < 4)
        {
            code += pinNum.ToString();
            displayPIN.text = code;
        }
        else if (pinNum == 10 && !string.IsNullOrEmpty(code))
        {
            code = code.Remove(code.Length - 1);
            displayPIN.text = code;
        }
        else if (pinNum == 11)
        {
            checkPINCode();
        }
        else
        {
            Debug.Log("Invalid PIN Length, Try Again!\n");
            invalid.StartDialogue();
        }
        Debug.Log(code);
    }

    public void checkPINCode()
    {
        if (string.IsNullOrEmpty(code) || code.Length != 4)
        {
            Debug.Log("Invalid PIN Length, Try Again!\n");
            invalid.StartDialogue();
        }
        else if (code == answer)
        {
            Debug.Log("Correct!\n");
            unlocked.StartDialogue();
        }
        else
        {
            Debug.Log("Incorrect!\n");
            invalid.StartDialogue();
        }
    }
}

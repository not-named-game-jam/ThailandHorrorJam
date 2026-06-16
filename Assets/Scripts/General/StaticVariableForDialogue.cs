using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticVariableForDialogue
{
    public static List<string> alreadyreadthisDialogue = new List<string>();
    public static Dictionary<string,bool> boolforDialogue = new Dictionary<string, bool>();

    public static Dictionary<string,int> statwithvalue = new Dictionary<string, int>()
    {
        {"SelfDoubt", 15}, {"SelfBlame", 70}
    };

    public static List<string> claimedRewards = new List<string>();
    
    public static void AddAlreadyRead(string dialogueTitle)
    {
        if (!alreadyreadthisDialogue.Contains(dialogueTitle))   
        {
            alreadyreadthisDialogue.Add(dialogueTitle);
        }
    }

    public static bool CheckForAlreadyRead(string dialogueTitle)
    {
        return alreadyreadthisDialogue.Contains(dialogueTitle);
    }
}
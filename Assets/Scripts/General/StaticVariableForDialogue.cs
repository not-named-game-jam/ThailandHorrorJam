using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticVariableForDialogue
{
    public static List<string> alreadyreadthisDialogue = new List<string>();
    public static List<string> conditionforDialogue = new List<string>();
    
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

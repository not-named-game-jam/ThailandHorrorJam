using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class DialogueSelector : MonoBehaviour
{
    public enum CompareTypes { None, LessEqual, MoreEqual, Equal, Range }

    [System.Serializable]
    public struct StatToCompare
    {
        [SerializeField] public string statName;
        [SerializeField] public string compareValue;
        [SerializeField] public CompareTypes blameCompareType;
    }
    
    [System.Serializable]
    public struct Dialogues
    {
        [SerializeField] public DialogueMaker dialogue;

        [SerializeField] public StatToCompare[] statToCompares;
    }

    [SerializeField] public Dialogues[] dialogues;

    public void RunDialogue()
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            bool allConditionpassed = true;
            for (int o = 0; o < dialogues[i].statToCompares.Length; o++)
            {
                if (!CheckCondition(dialogues[i].statToCompares[o]))
                {
                    allConditionpassed = false;
                    break;
                }
            }
            if (allConditionpassed)
            {
                dialogues[i].dialogue.StartDialogue();
                break;
            }
        }
    }

    bool CheckCondition(StatToCompare value)
    {
        int currentStatValue = 0;
        if (StaticVariableForDialogue.statwithvalue.ContainsKey(value.statName))
        {
            currentStatValue = StaticVariableForDialogue.statwithvalue[value.statName];
        }
        switch (value.blameCompareType)
        {
            case CompareTypes.Equal:
                int.TryParse(value.compareValue, out int EqualValue);
                return currentStatValue == EqualValue;
            case CompareTypes.LessEqual:
                int.TryParse(value.compareValue, out int LessEqualValue);
                return currentStatValue <= LessEqualValue;
            case CompareTypes.MoreEqual:
                int.TryParse(value.compareValue, out int MoreEqualValue);
                return currentStatValue >= MoreEqualValue;
            case CompareTypes.Range:
                string[] rangedValue = value.compareValue.Split('-');
                if (rangedValue.Length > 1)
                {
                    int.TryParse(rangedValue[0], out int minValue);
                    int.TryParse(rangedValue[1], out int maxValue);
                    return currentStatValue >= minValue && currentStatValue <= maxValue;
                }
                return false;
            case CompareTypes.None:
            default:
                return false;
        }
    }
}

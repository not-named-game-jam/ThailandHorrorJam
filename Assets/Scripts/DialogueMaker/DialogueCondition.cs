using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public enum ConditionTypes{ Boolean , Integer }
    public enum CompareTypes{None, LessEqual, MoreEqual , Equal , Range}
    public ConditionTypes conditionTypes;
    public string Name;
    public CompareTypes comparisionType;
    public string Value;
    public bool CheckCondition()
    {
        switch (conditionTypes)
        {
            case ConditionTypes.Boolean:
                if (StaticVariableForDialogue.boolforDialogue.ContainsKey(Name.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case ConditionTypes.Integer:
                int currentStatValue = 0;
                if (StaticVariableForDialogue.statwithvalue.ContainsKey(Name))
                {
                    currentStatValue = StaticVariableForDialogue.statwithvalue[Name];
                }
                else
                {
                    Debug.LogWarning("Statwithvalue of that key not found. Return false");
                    return false;
                }
                switch (comparisionType)
                {
                    case CompareTypes.Equal:
                        int.TryParse(Value, out int EqualValue);
                        return currentStatValue == EqualValue;
                    case CompareTypes.LessEqual:
                        int.TryParse(Value, out int LessEqualValue);
                        return currentStatValue <= LessEqualValue;
                    case CompareTypes.MoreEqual:
                        int.TryParse(Value, out int MoreEqualValue);
                        return currentStatValue >= MoreEqualValue;
                    case CompareTypes.Range:
                        string[] rangedValue = Value.Split('-');
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
    
            default:
                return false;
        }   
    }
}

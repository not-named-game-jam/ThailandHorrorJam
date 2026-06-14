using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition
{
    public enum ConditionTypes{ Boolean , Integer }
    public ConditionTypes conditionTypes;
    public string Name;
    public string Value;

    //------- In progress krub ----------
    //public bool CheckCondition(DialogueSystem name , DialogueSystem value)
    //{
        //switch (conditionTypes)
        //{
            //case ConditionTypes.Boolean:
                //if (StaticVariableForDialogue.conditionforDialogue.Contains())
                //{
                    //return true;
                //}
                //else
                //{
                    //return false;
                //}

            //case ConditionTypes.Integer:

        //}
    //}
}

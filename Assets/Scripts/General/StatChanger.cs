using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatChanger : MonoBehaviour
{
    public enum ChangeType
    {
        Boolean, Integer
    }
        
    [SerializeField] string Valuename;
    [SerializeField] int ValueChange;
    [SerializeField] private ChangeType type;
    public void valueChanger()
    {
        switch (type)
        {
            case ChangeType.Boolean:
                StaticVariableForDialogue.boolforDialogue[Valuename] = true;
                Debug.Log("Set"+Valuename+StaticVariableForDialogue.boolforDialogue[Valuename]);
                this.enabled = false;
                break;
            
            case ChangeType.Integer:
                if (StaticVariableForDialogue.statwithvalue.ContainsKey(Valuename))
                {
                    StaticVariableForDialogue.statwithvalue[Valuename] += ValueChange;
                }
                else
                {
                    StaticVariableForDialogue.statwithvalue.Add(Valuename, ValueChange);
                    Debug.Log("The Value not in Storage, adding value and keys");
                }
                Debug.Log(Valuename+" value change by "+ValueChange.ToString()+" and now equal "+StaticVariableForDialogue.statwithvalue[Valuename]);
                this.enabled = false;  
                break;
        }
    }

}

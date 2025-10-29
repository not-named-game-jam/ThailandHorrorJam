using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAllObject : MonoBehaviour
{
    [SerializeField] DialogueMaker afterLook;
    bool clickedCactus;
    bool clickedBox;
    bool clickedPlate;
    bool clickedTree;
    bool clickedPedestal;
    bool clickedBush;

    bool onetimecoroutine;

    void Start()
    {

    }

    void Update()
    {
        if (!onetimecoroutine && clickedBox && clickedBush && clickedCactus && clickedPedestal && clickedPlate && clickedTree)
        {
            Debug.Log("firstline");
            StartCoroutine(afterall());
        }
    }

    public void cactus()
    {
        clickedCactus = true;
        Debug.Log("clickedCactus" + clickedCactus);
    }

    public void box()
    {
        clickedBox = true;
        Debug.Log("clickedBox" + clickedBox);
    }

    public void bush()
    {
        clickedBush = true;
        Debug.Log("clickedBush" + clickedBush);
    }

    public void pedestal()
    {
        clickedPedestal = true;
        Debug.Log("clickedPedestal" + clickedPedestal);
    }

    public void plate()
    {
        clickedPlate = true;
        Debug.Log("clickedPlate" + clickedPlate);
    }

    public void tree()
    {
        clickedTree = true;
        Debug.Log("clickedTree" + clickedTree);
    }
    
    IEnumerator afterall()
    {
        Debug.Log("corutinestart");
        onetimecoroutine = true;
        Debug.Log("before yield");
        yield return new WaitForSeconds(10f);
        afterLook.StartDialogue();
        tofalse();
    }
    
    
    public void tofalse()
    {
        Debug.Log("tofalse");
        clickedBox = false;
        clickedBush = false;
        clickedCactus = false;
        clickedPedestal = false;
        clickedPlate = false;
        clickedTree = false;
    }

}

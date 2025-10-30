using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour
{
    [SerializeField] int spawnPaperClickThreshold; // how many times before the paper spawns
    [SerializeField] GameObject paperPrefab;
    [SerializeField] GameObject closedHand;
    [SerializeField] GameObject openedHand;
    private int clickedAmount;
    ClickEventMethods clickEventMethods;
    bool hasSpawnPaper;

    void Awake()
    {
        if (TryGetComponent<ClickEventMethods>(out ClickEventMethods clickEventMethods))
        {
            this.clickEventMethods = clickEventMethods;
        }
        else
        {
            Debug.LogError("Click Event Methods Component Not Found!");
        }
    }

    void Start()
    {
        clickedAmount = 0;
        hasSpawnPaper = false;
        // closedHand.SetActive(true);
        // openedHand.SetActive(false);
    }
    public void IncrementClick() // Called by button 
    {
        if (hasSpawnPaper) return;

        closedHand.SetActive(closedHand.activeSelf ? false : true);
        openedHand.SetActive(openedHand.activeSelf ? false : true);
        clickedAmount++;

        CheckState(clickedAmount);
    }

    private void CheckState(int clickedAmount)
    {
        if (clickedAmount >= spawnPaperClickThreshold && !hasSpawnPaper)
        {
            hasSpawnPaper = true;
            GetComponent<Image>().raycastTarget = false;
            SpawnPaper();
        }
        else // Less than threshold -> Play only animation
        {
            if (clickEventMethods == null) { Debug.LogError("Click Event Methods Component Not Found!"); return; }

            clickEventMethods?.StartEvent();
        }
    }

    private void SpawnPaper()
    {
        if (paperPrefab == null) return;
        Debug.Log("Spawning Paper...");
        hasSpawnPaper = true;
        GameObject prefab = Instantiate(
            paperPrefab,
            parent: this.transform.parent,
            position: transform.position,
            rotation: Quaternion.identity
        );
        prefab.transform.SetAsFirstSibling();
        prefab.GetComponent<ClickEventMethods>().StartEvent();
        //StartCoroutine(SpawnPaperCoroutine());
    }
    
    // private IEnumerator SpawnPaperCoroutine()
    // {
        
    //     yield return new WaitForEndOfFrame();

    //     if (prefab.TryGetComponent<ClickEventMethods>(out ClickEventMethods paperMethods))
    //     {
    //         Debug.Log("The paper animation belongs to: " + paperMethods.gameObject.name);
    //         paperMethods.StartEvent();
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Paper's Click Event Method's Component is not found!");
    //     }
    // }
}

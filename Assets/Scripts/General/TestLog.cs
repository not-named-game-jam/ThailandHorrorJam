using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public struct DialogueEntry
{
    public string speakerName;
    public string dialogueText;
}

public class TestLog : MonoBehaviour
{   
    public static TestLog Instance;

    [Header("Data Storage")]
    private List<DialogueEntry> logHistory = new List<DialogueEntry>();

    [Header("UI References")]
    [SerializeField] private GameObject logPanel;          // The overall Scroll View panel
    [SerializeField] private GameObject logEntryPrefab;    // Prefab with a TextMeshProUGUI component
    [SerializeField] private Transform logContentParent;   // The Content container inside the Scroll View
    public static bool logisActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !InEndingtrue.InEnding)
        {
            SetLogWindowState(!logisActive);
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Add from dialoguesystem to list
    public void AddToLog(string speaker, string text)
    {
        string cleanSentence = text.Trim();

        if (string.IsNullOrEmpty(text)) return;

        if (string.IsNullOrWhiteSpace(text) || cleanSentence.StartsWith('$')) return;

        logHistory.Add(new DialogueEntry { speakerName = speaker, dialogueText = text });
    }

    // To open log interface
    public void SetLogWindowState(bool open)
    {
        logisActive = open;
        logPanel.SetActive(open);
        CheckOverlayActive.IsOverlayActive = open;

        if (open)
        {
            GenerateLogUI();
        }
    }

    public void CloseLogWindow()
    {
        if(logisActive == false || !logPanel.activeSelf)
        {
            return;
        }
        else
        {
           SetLogWindowState(false); 
        }
        
    }

    private void GenerateLogUI()
    {
        // Clear the object in content.
        foreach (Transform child in logContentParent)
        {
            Destroy(child.gameObject);
        }

        // Create new text object in content.
        foreach (var entry in logHistory)
        {
            GameObject newGo = Instantiate(logEntryPrefab, logContentParent);
            TextMeshProUGUI tmp = newGo.GetComponent<TextMeshProUGUI>();
            
            if (tmp != null)
            {
                // Formats the name to look clean and separate from body text
                if (entry.speakerName == "")
                {
                    tmp.text = entry.dialogueText;
                }
                else
                { 
                    tmp.text = $"<color=#FFCC00><b>{entry.speakerName}:</b></color> {entry.dialogueText}";
                }
            }
        }
    }
}

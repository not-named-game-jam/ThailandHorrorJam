using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType { TextImmersive, CharacterDialogue, JustText, Wait }

public enum TextColor { White, Red, Gold, Navy, Teal, Gray, Pink, Orange }

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 5), SerializeField] 
    private string text = "Hi~";

    [HideInInspector] public string displayText;
    public TextColor color = TextColor.White;
    public Sprite characterSprite;
    [Range(0.01f, 0.12f)]
    public float typeInterval = 0.036f;

    [HideInInspector] public DialogueType dialogueType = DialogueType.CharacterDialogue;
    [HideInInspector] public string typeSound;
    [HideInInspector] public string characterName = "";
    [HideInInspector] public Color textColor;
    [HideInInspector] public List<int> pauseIndex = new List<int>();
    [SerializeField] public List<FunctionCalls> functionCalls = new List<FunctionCalls>();
    
    public void Initialize() {
        typeSound = "speak";
        displayText = text;
        
        if (string.IsNullOrWhiteSpace(text)) {
            dialogueType = DialogueType.Wait;
            displayText = text;
        }
        else if (characterSprite != null) {
            characterName = characterSprite.name.Split('_')[0];
            dialogueType = DialogueType.CharacterDialogue;
        }
        else if (!string.IsNullOrEmpty(text) && text.StartsWith("```")) {
            dialogueType = DialogueType.TextImmersive;
            displayText = displayText.Substring(3);
        }
        else {
            dialogueType = DialogueType.JustText;
        }

        pauseIndex.Clear();
        for(int i = 0; i < displayText.Length; i++) {
            if(displayText[i] == '$') {
                pauseIndex.Add(i);
                displayText = displayText.Remove(i, 1);
                i--;
            }
        }

        textColor = color switch {
            TextColor.Red => new Color(0.733f, 0.0667f, 0.2157f),  // #BB1537 (Red)
            TextColor.Gold => new Color(1f, 0.84f, 0f),            // Gold
            TextColor.Navy => new Color(0f, 0f, 0.5f),             // Navy blue
            TextColor.Teal => new Color(0f, 0.5f, 0.5f),           // Teal
            TextColor.Gray => new Color(0.6f, 0.6f, 0.6f),         // Gray
            TextColor.Pink => new Color(1f, 0.41f, 0.71f),         // Pink
            TextColor.Orange => new Color(1f, 0.65f, 0f),          // Orange
            _ => new Color(0.9098f, 0.9255f, 0.9333f)              // #E8ECEE (White)
        };
    }
}


[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueMaker/Dialogue")]
public class DialogueMaker : ScriptableObject
{
    [Header("Dialogue Sequence")]
    [Tooltip("List of dialogue lines in this sequence")]
    [SerializeField] private List<DialogueLine> dialogueSequence;

    [Header("Runtime State")]
    [Tooltip("Current line being displayed")]
    [HideInInspector] public int _currentLineIndex = -1;
    
    [Tooltip("Coroutine reference for auto-continue")]
    [HideInInspector] public Coroutine _autoContinueCoroutine;

    // Public property for external access
    public bool IsActive { get; private set; }

    void OnEnable()
    {
        _currentLineIndex = -1;
        foreach (DialogueLine line in dialogueSequence)
        {
            line.Initialize();
        }
    }

    /// Advances to the next line in the sequence. Called by DialogueUI.
    public void StartDialogue()
    {
        DialogueSystem.instance.SetCurrentSequence(this);

        _currentLineIndex++;

        if (_currentLineIndex < dialogueSequence.Count)
        {
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = dialogueSequence[_currentLineIndex];

        DialogueSystem.instance.SetDialogueLine(
            line.displayText,
            line.textColor,
            line.characterName,
            line.characterSprite,
            line.typeInterval,
            line.typeSound,
            line.dialogueType,
            line.pauseIndex,
            line.functionCalls
        );
    }

    public void EndDialogue()
    {
        IsActive = false;
        _currentLineIndex = -1;
        DialogueSystem.instance.EndDialogue();
    }
}

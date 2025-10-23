using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType { TextImmersive, CharacterDialogue, JustText }

[System.Serializable]
public class DialogueLine
{
    [TextArea(3, 5), SerializeField] 
    private string text = "Hi~";

    [HideInInspector] public string displayText;
    public Color textColor = Color.white;
    public Sprite characterSprite;
    [Range(0.01f, 0.12f)]
    public float typeInterval = 0.05f;

    [HideInInspector] public DialogueType dialogueType = DialogueType.CharacterDialogue;
    [HideInInspector] public AudioClip typeSound;
    [HideInInspector] public string characterName = "";
    [HideInInspector] public List<int> pauseIndex = new List<int>();
    [SerializeField] public List<FunctionCalls> functionCalls = new List<FunctionCalls>();
    
    public void Initialize() {
        typeSound = Resources.Load<AudioClip>("Sounds/Dialogue/speak");
        displayText = text;
        
        if (characterSprite != null) {
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

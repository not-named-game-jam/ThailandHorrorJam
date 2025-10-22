using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType { TextImmersive, CharacterDialogue, JustText }

[System.Serializable]
public class DialogueLine
{
    public DialogueType dialogueType = DialogueType.CharacterDialogue;

    [Header("Text Data")]
    [TextArea(3, 5)]
    public string text = "Hi~";
    public Color textColor = Color.white;
    public float typeInterval = 0.05f;
    public AudioClip typeSound;
    
    [Header("Character Data [Character Dialogue]")]
    public string characterName = ""; 
    public Sprite characterSprite;
}


[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueMaker/Dialogue")]
public class DialogueMaker : ScriptableObject
{
    [SerializeField] private List<DialogueLine> dialogueSequence;

    // Runtime state variables
    [HideInInspector] public int _currentLineIndex = -1;
    private Coroutine _autoContinueCoroutine;

    // Public property for external access
    public bool IsActive { get; private set; }

    void OnEnable()
    {
        _currentLineIndex = -1;
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
            line.text,
            line.textColor,
            line.characterName,
            line.characterSprite,
            line.typeInterval,
            line.typeSound,
            line.dialogueType
        );
    }

    public void EndDialogue()
    {
        IsActive = false;
        _currentLineIndex = -1;
        DialogueSystem.instance.EndDialogue();
    }
}

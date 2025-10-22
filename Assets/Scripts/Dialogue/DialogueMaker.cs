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
    public float typeInterval = 0.05f;
    public AudioClip typeSound;
    
    [Header("Character Data [Character Dialogue]")]
    public string characterName = ""; 
    public Sprite characterSprite;
}


[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueMaker/Dialogue")]
public class DialogueMaker : ScriptableObject
{
    [SerializeField] private DialogueLine[] dialogues;
}

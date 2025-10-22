using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Needed for coroutines

public class DialogueSystem : MonoBehaviour
{
    // --- UI References ---
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] sentenceText;

    [SerializeField] private GameObject immersiveDialoguePanel;
    
    [SerializeField] private GameObject characterDialoguePanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image image;
    // --- Sound ---
    // Removed direct AudioClip field, now set per line

    // --- Core State ---
    public static DialogueSystem instance;
    public bool IsActive { get; private set; } = false;
    public bool IsTyping { get; private set; } = false;

    // --- Typing Logic ---
    private float _secondsPerChar = 0.06f; // Default typing speed
    private AudioClip _dialogueAudio;      // Sound set by the current line

    // Reference to the currently playing sequence
    private DialogueMaker _currentSequence;

    private float skipCooldown;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Sets and starts typing a new line.
    /// </summary>
    public void SetDialogueLine(string sentence, Color textCol, string speaker, Sprite sprite, float typeInterval, AudioClip sound, DialogueType type)
    {
        IsActive = true;

        if (type == DialogueType.TextImmersive)
        {
            immersiveDialoguePanel.SetActive(true);
            characterDialoguePanel.SetActive(false);
            dialoguePanel.SetActive(false);
        }
        else if(type == DialogueType.CharacterDialogue)
        {
            immersiveDialoguePanel.SetActive(false);
            characterDialoguePanel.SetActive(true);
            dialoguePanel.SetActive(false);
        }
        else if(type == DialogueType.JustText)
        {
            immersiveDialoguePanel.SetActive(false);
            characterDialoguePanel.SetActive(false);
            dialoguePanel.SetActive(true);
        }

        // Set UI elements
        image.sprite = sprite;
        nameText.text = speaker;

        foreach(TextMeshProUGUI x in sentenceText)
        {
            x.text = sentence;
            x.color = textCol;
        }
        // sentenceText.text = sentence;

        _secondsPerChar = typeInterval;
        _dialogueAudio = sound;

        // Clear previous text and start typing effect
        foreach(TextMeshProUGUI x in sentenceText)
        {
            x.maxVisibleCharacters = 0;
        }
        // sentenceText.maxVisibleCharacters = 0;

        StopAllCoroutines();
        StartCoroutine(TypeSentence());
    }

    // New method to visually hide the dialogue panel content (e.g., reset text/image)
    public void ClearDialoguePanel()
    {
        nameText.text = "";

        foreach(TextMeshProUGUI x in sentenceText)
        {
            x.text = "";
        }
        // sentenceText.text = "";
        // You might set the image to a placeholder or transparent sprite here
        image.sprite = null; 
    }

    /// <summary>
    /// Coroutine to handle the gradual typing effect.
    /// </summary>
    private IEnumerator TypeSentence()
    {
        IsTyping = true;
        int totalLength = sentenceText[0].text.Length;
        
        while (sentenceText[0].maxVisibleCharacters < totalLength)
        {
            foreach (TextMeshProUGUI x in sentenceText)
            {
                x.maxVisibleCharacters++;
            }
            // sentenceText.maxVisibleCharacters++;
            // Play sound if available
            AudioSource sound = gameObject.GetComponent<AudioSource>();
            if (_dialogueAudio != null && gameObject.GetComponent<AudioSource>() != null) {
                sound.clip = _dialogueAudio;
                sound.Play();
            }
            
            // Use WaitForSecondsRealtime since Time.timeScale is 0
            yield return new WaitForSecondsRealtime(_secondsPerChar);
        }
        IsTyping = false;
    }


    /// <summary>
    /// Fully hides the dialogue system and resumes game time.
    /// </summary>
    public void EndDialogue()
    {
        Time.timeScale = 1;
        IsActive = false;
        immersiveDialoguePanel.SetActive(false);
        characterDialoguePanel.SetActive(false);
        dialoguePanel.SetActive(false);
        _currentSequence = null; // Clear the sequence reference
        skipCooldown = 0;
    }

    void Update()
    {
        if (!IsActive) return;

        skipCooldown += Time.unscaledDeltaTime;

        // Check for player input to continue
        if (Pressed() && skipCooldown >= 0.5f)
        {
            skipCooldown = 0;
            ContinueDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (!IsActive) return;
        
        if (IsTyping)
        {
            // Skip typing animation
            StopAllCoroutines();
            foreach(TextMeshProUGUI x in sentenceText)
            {
                x.maxVisibleCharacters = x.text.Length;
            }
            // sentenceText.maxVisibleCharacters = sentenceText.text.Length;
            IsTyping = false;
        }
        else if (_currentSequence != null)
        {
            // Typing is done, advance to the next line managed by the ScriptableObject
            _currentSequence.StartDialogue();
        }
    }

    /// <summary>
    /// Sets the active dialogue sequence. Called by the DialogueStarter.
    /// </summary>
    public void SetCurrentSequence(DialogueMaker sequence)
    {
        Time.timeScale = 0;
        _currentSequence = sequence;
    }

    private bool Pressed() =>
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Return) ||
        Input.GetMouseButtonDown(0);
}

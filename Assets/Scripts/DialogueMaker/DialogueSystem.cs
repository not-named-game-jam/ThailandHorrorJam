using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Needed for coroutines
using System.Linq;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    // --- UI References ---
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] sentenceText;

    [SerializeField] private GameObject immersiveDialoguePanel;
    [SerializeField] private GameObject characterDialoguePanel;
    [SerializeField] private GameObject justTextPanel;

    [SerializeField] private CanvasGroup immersiveContinueIndicator;
    [SerializeField] private CanvasGroup characterContinueIndicator;
    [SerializeField] private CanvasGroup justTextContinueIndicator;
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
    private float continueIndicatorCooldown;
    private float continueIndicatorAlpha;
    private float targetAlpha;

    private List<int> _pauseIndices;
    private List<FunctionCalls> _functionCalls;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        justTextPanel.SetActive(false);
    }

    /// <summary>
    /// Sets and starts typing a new line.
    /// </summary>
    public void SetDialogueLine(string sentence, Color textCol, string speaker, Sprite sprite, float typeInterval, AudioClip sound, DialogueType type, List<int> pauseIndices, List<FunctionCalls> functionCalls)
    {
        IsActive = true;

        if (type == DialogueType.TextImmersive)
        {
            immersiveDialoguePanel.SetActive(true);
            characterDialoguePanel.SetActive(false);
            justTextPanel.SetActive(false);
        }
        else if(type == DialogueType.CharacterDialogue)
        {
            immersiveDialoguePanel.SetActive(false);
            characterDialoguePanel.SetActive(true);
            justTextPanel.SetActive(false);
        }
        else if(type == DialogueType.JustText)
        {
            immersiveDialoguePanel.SetActive(false);
            characterDialoguePanel.SetActive(false);
            justTextPanel.SetActive(true);
        }

        // Set UI elements
        image.sprite = sprite;
        nameText.text = speaker;

        foreach(TextMeshProUGUI x in sentenceText)
        {
            x.gameObject.SetActive(true);
            x.text = sentence;
            x.color = new Color(textCol.r, textCol.g, textCol.b, 1f);
        }

        _secondsPerChar = typeInterval;
        _dialogueAudio = sound;
        _pauseIndices = pauseIndices;
        _functionCalls = functionCalls;

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
        int currentCharIndex = 0;
        
        while (currentCharIndex < totalLength)
        {
            // Check for pause at current character index
            int pauseCount = _pauseIndices.Count(i => i == currentCharIndex);
            if (pauseCount > 0) {
                float pauseDuration = _secondsPerChar * 3 * pauseCount;
                yield return new WaitForSecondsRealtime(pauseDuration);
            }

            _functionCalls
                .FindAll(x => x.index == currentCharIndex)
                .ForEach(x => x.CallFunction(this));

            // Update visible characters
            foreach (TextMeshProUGUI textElement in sentenceText)
            {
                textElement.maxVisibleCharacters = currentCharIndex + 1;
            }
            currentCharIndex++;

            // Play sound if available
            AudioSource sound = gameObject.GetComponent<AudioSource>();
            if (_dialogueAudio != null && sound != null) {
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
        justTextPanel.SetActive(false);
        _currentSequence = null; // Clear the sequence reference
        skipCooldown = 0;
        continueIndicatorCooldown = 0;
        continueIndicatorAlpha = 0f;
        targetAlpha = 0f;
        immersiveContinueIndicator.alpha = 0;
        characterContinueIndicator.alpha = 0;
        justTextContinueIndicator.alpha = 0;
    }

    void Update()
    {
        if (!IsActive) return;

        skipCooldown += Time.unscaledDeltaTime;

        // Check for player input to continue
        if (Pressed() && skipCooldown >= 0.2f)
        {
            skipCooldown = 0;
            ContinueDialogue();
        }

        if(!IsTyping && _currentSequence != null)
        {
            continueIndicatorCooldown += Time.unscaledDeltaTime;

            targetAlpha = continueIndicatorCooldown >= 0.5f ? 1f : 0f;
            
            float smoothTime = 0.07f;
            continueIndicatorAlpha = Mathf.Lerp(continueIndicatorAlpha, targetAlpha, Time.unscaledDeltaTime / smoothTime);
            
            immersiveContinueIndicator.alpha = continueIndicatorAlpha;
            characterContinueIndicator.alpha = continueIndicatorAlpha;
            justTextContinueIndicator.alpha = continueIndicatorAlpha;
            
            if(continueIndicatorCooldown >= 1f) {
                continueIndicatorCooldown = 0;
            }
        }
    }

    public void ContinueDialogue()
    {
        if (!IsActive) return;
        
        if (IsTyping)
        {
            // Skip typing animation
            StopAllCoroutines();
            
            // Call all remaining functions that would have been called during typing
            int totalLength = sentenceText[0].text.Length;
            for (int i = sentenceText[0].maxVisibleCharacters; i < totalLength; i++)
            {
                _functionCalls
                    .FindAll(x => x.index == i)
                    .ForEach(x => x.CallFunction(this));
            }
            
            // Show all text
            foreach(TextMeshProUGUI x in sentenceText)
            {
                x.maxVisibleCharacters = x.text.Length;
            }
            IsTyping = false;
        }
        else if (_currentSequence != null)
        {
            // Typing is done, advance to the next line managed by the ScriptableObject
            immersiveContinueIndicator.alpha = 0;
            characterContinueIndicator.alpha = 0;
            justTextContinueIndicator.alpha = 0;
            continueIndicatorCooldown = 0;
            continueIndicatorAlpha = 0;
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

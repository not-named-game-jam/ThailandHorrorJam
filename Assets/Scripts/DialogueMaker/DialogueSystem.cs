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

    [SerializeField] private GameObject stopInteraction;

    public static DialogueSystem instance;
    public bool IsActive { get; private set; } = false;
    public bool IsTyping { get; private set; } = true;
    private string _dialogueAudio;
    private List<int> _pauseIndices;
    private List<FunctionCalls> _functionCalls;
    private DialogueType _type = DialogueType.Wait;

    private float _secondsPerChar = 0.06f;
    private DialogueMaker _currentSequence;
    private float skipCooldown;
    private float continueIndicatorCooldown;
    private float continueIndicatorAlpha;
    private float targetAlpha;

    private bool isFading = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

//duration = 0.3f
    private IEnumerator FadePanel(GameObject panel, bool isFadeIn, float duration = 0.1f) {
        isFading = true;
        panel.SetActive(true);
        var canvasGroup = panel.GetComponent<CanvasGroup>();
        
        float elapsed = 0f;
        float startAlpha = isFadeIn ? 0f : 1f;
        float targetAlpha = isFadeIn ? 1f : 0f;
        canvasGroup.alpha = startAlpha;
        
        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            yield return null;
        }
        
        canvasGroup.alpha = targetAlpha;
        panel.SetActive(isFadeIn);
        isFading = false;
    }

    private IEnumerator SetDialogueCoroutine(string sentence, Color textCol, string speaker, Sprite sprite, float typeInterval, string sound, DialogueType type, List<int> pauseIndices, List<FunctionCalls> functionCalls) {
        IsTyping = true;

        if(stopInteraction) stopInteraction.SetActive(true);

        if (_type != type) {
            if (_type == DialogueType.TextImmersive) {
                yield return StartCoroutine(FadePanel(immersiveDialoguePanel, false));
            } else if (_type == DialogueType.CharacterDialogue) {
                yield return StartCoroutine(FadePanel(characterDialoguePanel, false));
            } else if (_type == DialogueType.JustText) {
                yield return StartCoroutine(FadePanel(justTextPanel, false));
            }
        }

        IsActive = true;
        image.sprite = sprite;
        nameText.text = speaker;
        _dialogueAudio = type == DialogueType.Wait ? "" : sound;
        _pauseIndices = pauseIndices;
        _functionCalls = functionCalls;
        _secondsPerChar = typeInterval;

        // Set the text content for all text elements
        foreach (TextMeshProUGUI x in sentenceText) {
            x.text = sentence;
            x.color = textCol;
            x.maxVisibleCharacters = 0;
        }

        // Fade in the new panel if changing types and not changing to Wait type
        if (_type != type) {
            if (type == DialogueType.TextImmersive) {
                yield return StartCoroutine(FadePanel(immersiveDialoguePanel, true));
            } else if (type == DialogueType.CharacterDialogue) {
                yield return StartCoroutine(FadePanel(characterDialoguePanel, true));
            } else if (type == DialogueType.JustText) {
                yield return StartCoroutine(FadePanel(justTextPanel, true));
            }
        }
        _type = type;

        yield return StartCoroutine(TypeSentence());
    }

    /// <summary>
    /// Sets and starts typing a new line.
    /// </summary>
    public void SetDialogueLine(string sentence, Color textCol, string speaker, Sprite sprite, float typeInterval, string sound, DialogueType type, List<int> pauseIndices, List<FunctionCalls> functionCalls)
    {
        StopAllCoroutines();
        StartCoroutine(SetDialogueCoroutine(sentence, textCol, speaker, sprite, typeInterval, sound, type, pauseIndices, functionCalls));
    }

    public void ClearDialoguePanel()
    {
        nameText.text = "";
        foreach (TextMeshProUGUI x in sentenceText) x.text = "";
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

        _functionCalls
            .FindAll(x => x.index == -1)
            .ForEach(x => x.CallFunction(this));

        while (currentCharIndex < totalLength)
        {
            // Check for pause at current character index
            int pauseCount = _pauseIndices.Count(i => i == currentCharIndex);
            if (pauseCount > 0)
            {
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

            // Play sound
            SoundManager.instance.PlaySfx(_dialogueAudio, 0.5f);

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
        // Time.timeScale = 1;
        IsActive = false;
        immersiveDialoguePanel.SetActive(false);
        characterDialoguePanel.SetActive(false);
        justTextPanel.SetActive(false);
        _currentSequence = null; // Clear the sequence reference
        _type = DialogueType.Wait;
        skipCooldown = 0;
        continueIndicatorCooldown = 0;
        continueIndicatorAlpha = 0f;
        targetAlpha = 0f;
        immersiveContinueIndicator.alpha = 0;
        characterContinueIndicator.alpha = 0;
        justTextContinueIndicator.alpha = 0;

        if(stopInteraction) stopInteraction.SetActive(false);
    }

    void Update()
    {
        if (!IsActive) return;

        skipCooldown += Time.unscaledDeltaTime;
        
        if (((Pressed() && skipCooldown >= 0.3f) || Input.GetKey(KeyCode.Tab)) && !isFading && _type != DialogueType.Wait)
        {
            skipCooldown = 0.0f;
            ContinueDialogue();
        }

        if (!IsTyping && _currentSequence != null)
        {
            if(_type == DialogueType.Wait) {
                ContinueDialogue();
                return;
            }
            continueIndicatorCooldown += Time.unscaledDeltaTime;

            targetAlpha = continueIndicatorCooldown >= 0.3f ? 1f : 0f;

            float smoothTime = 0.07f;
            continueIndicatorAlpha = Mathf.Lerp(continueIndicatorAlpha, targetAlpha, Time.unscaledDeltaTime / smoothTime);

            immersiveContinueIndicator.alpha = continueIndicatorAlpha;
            characterContinueIndicator.alpha = continueIndicatorAlpha;
            justTextContinueIndicator.alpha = continueIndicatorAlpha;

            if (continueIndicatorCooldown >= 1f)
            {
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
            _functionCalls
                .FindAll(x => x.index >= sentenceText[0].maxVisibleCharacters)
                .ForEach(x => x.CallFunction(this));

            // Show all text
            foreach (TextMeshProUGUI x in sentenceText) {
                x.maxVisibleCharacters = x.text.Length;
            }
            
            IsTyping = false;
        }
        else if (_currentSequence != null)
        {
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
        // Time.timeScale = 0;
        _currentSequence = sequence;
    }

    private bool Pressed() =>
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Return) ||
        Input.GetMouseButtonDown(0);
}

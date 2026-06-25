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
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject skipPanel;
    [SerializeField] private GameObject skipWarning;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TextMeshProUGUI[] choiceTexts;

    [SerializeField] private CanvasGroup immersiveContinueIndicator;
    [SerializeField] private CanvasGroup characterContinueIndicator;
    [SerializeField] private CanvasGroup justTextContinueIndicator;
    [SerializeField] private Image image;

    [SerializeField] private GameObject stopInteraction;
    [SerializeField] private FloatingTextForButton spawnFloatingText;

    public static DialogueSystem instance;
    public bool IsActive { get; private set; } = false;
    public bool IsTyping { get; private set; } = true;
    private bool IsShowChoices;
    private string _dialogueAudio;
    private List<int> _pauseIndices;
    private List<FunctionCalls> _functionCalls;
    private DialogueType _type = DialogueType.Wait;

    private float _secondsPerChar = 0.06f;
    private DialogueMaker _currentSequence;
    private string saveCurrentSequencename;
    private float skipCooldown;
    private float continueIndicatorCooldown;
    private float continueIndicatorAlpha;
    private float targetAlpha;

    private bool isFading = false;
    
    private Coroutine dialogueLoopCoroutine;
    private Coroutine typingCoroutine;

    [Header("Auto Settings")]
    [SerializeField] bool isAuto = false;
    [SerializeField] float minAutotime = 1.0f;
    [SerializeField] float autotimeperCharacter = 0.015f;

    private Coroutine autoCoroutine;
    [SerializeField] GameObject autoButtonparent;
    [SerializeField] Image autoButton;
    [SerializeField] TextMeshProUGUI autoText;

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

        if(autoButtonparent != null) autoButtonparent.SetActive(true);
        UpdateAutoButton();

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

        UpdateAutoButton();

        typingCoroutine = StartCoroutine(TypeSentence());
        //yield return StartCoroutine(TypeSentence());
    }

    /// <summary>
    /// Sets and starts typing a new line.
    /// </summary>
    public void SetDialogueLine(string sentence, Color textCol, string speaker, Sprite sprite, float typeInterval, string sound, DialogueType type, List<int> pauseIndices, List<FunctionCalls> functionCalls)
    {
        if(dialogueLoopCoroutine != null) StopCoroutine(dialogueLoopCoroutine);
        if(typingCoroutine != null) StopCoroutine(typingCoroutine);
        if(autoCoroutine != null) StopCoroutine(autoCoroutine);
        //StopAllCoroutines();
        TestLog.Instance?.AddToLog(speaker, sentence);
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
        
        for (int i = 0; i < _functionCalls.Count; i++)
        {
            if (_functionCalls[i].index == currentCharIndex)
            {
                _functionCalls[i].CallFunction(this);
            }
        }
        //_functionCalls
            //.FindAll(x => x.index == -1)
            //.ForEach(x => x.CallFunction(this));

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

        if (isAuto && _currentSequence != null)
        {
            autoCoroutine = StartCoroutine(AutoDialogue(totalLength));
        }
    }


    /// <summary>
    /// Fully hides the dialogue system and resumes game time.
    /// </summary>
    public void EndDialogue()
    {
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        // Time.timeScale = 1;
        IsActive = false;
        immersiveDialoguePanel.SetActive(false);
        characterDialoguePanel.SetActive(false);
        justTextPanel.SetActive(false);
        skipPanel.SetActive(false);
        if(autoButtonparent != null) autoButtonparent.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.A) && !IsShowChoices)
        {
            ToggleAuto();
        }

        if (Input.GetKeyDown(KeyCode.P) && skipPanel.activeSelf)
        {
            SkipButtonWarning();
        }
        

        skipCooldown += Time.unscaledDeltaTime;
        
        if (((Pressed() && skipCooldown >= 0.1f) || Input.GetKey(KeyCode.Tab)) && !isFading && _type != DialogueType.Wait && !TestLog.logisActive && !skipWarning.activeSelf)
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

            targetAlpha = continueIndicatorCooldown >= 0.1f ? 1f : 0f;

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
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            //StopAllCoroutines();

            // Call all remaining functions that would have been called during typing
            _functionCalls
                .FindAll(x => x.index >= sentenceText[0].maxVisibleCharacters)
                .ForEach(x => x.CallFunction(this));

            // Show all text
            foreach (TextMeshProUGUI x in sentenceText) {
                x.maxVisibleCharacters = x.text.Length;
            }
            if (isAuto)
            {
                Debug.Log("Pass here");
                int halflength = sentenceText[0].text.Length/2;
                autoCoroutine = StartCoroutine(AutoDialogue(sentenceText[0].maxVisibleCharacters/2));
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
        saveCurrentSequencename = sequence.name;
    }

    private bool Pressed() =>
        Input.GetKeyDown(KeyCode.Space) ||
        Input.GetKeyDown(KeyCode.E) ||
        Input.GetKeyDown(KeyCode.Return) ||
        Input.GetMouseButtonDown(0);

    private IEnumerator AutoDialogue(int textLength)
    {
        float autoTime = minAutotime + (autotimeperCharacter*textLength);

        yield return new WaitForSecondsRealtime(autoTime);
        autoCoroutine = null;

        ContinueDialogue();
    }

    public void ToggleAuto()
    {
        isAuto = !isAuto;
        UpdateAutoButton();

        if(isAuto)
        {
            if (!IsTyping && _currentSequence != null && autoCoroutine == null)
            {
                autoCoroutine = StartCoroutine(AutoDialogue(sentenceText[0].text.Length/4));
            }
        }
        else
        {
            if (autoCoroutine != null)
            {
                StopCoroutine(autoCoroutine);
                autoCoroutine = null;
            }
        }
    }

    private void UpdateAutoButton()
    {
        if (autoButton == null && autoText == null) return;

        if (isAuto)
        {
            autoButton.color = Color.yellow;
            autoText.color = Color.black;
        }
        else
        {
            autoButton.color = Color.black;
            autoText.color = Color.white;
        }
    }

    public void ShowChoices(List<DialogueChoices> dialogueChoices)
    {
        IsShowChoices = true;
        IsActive = true;
        IsTyping = false;
        immersiveDialoguePanel.SetActive(false);
        characterDialoguePanel.SetActive(false);
        justTextPanel.SetActive(false);
        immersiveContinueIndicator.alpha = 0;
        characterContinueIndicator.alpha = 0;
        justTextContinueIndicator.alpha = 0;

        StartCoroutine(FadePanel(choicesPanel, true));

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < dialogueChoices.Count)
            {
                choiceButtons[i].interactable = true;
                choiceButtons[i].gameObject.SetActive(true);
                bool allConditionpassed = true;
                foreach(DialogueCondition condition in dialogueChoices[i].dialogueConditions)
                    {
                        if (condition.CheckCondition() == false)
                        {
                            allConditionpassed = false;
                            break;
                        }
                    }
                choiceButtons[i].onClick.RemoveAllListeners();
                if (allConditionpassed)
                {
                    choiceButtons[i].interactable = true;
                    choiceTexts[i].text = dialogueChoices[i].choicesText;
                    DialogueMaker choicesresult = dialogueChoices[i].nextDialogue;
                    List<DialogueRewards> rewardresult = dialogueChoices[i].dialogueRewards;
                    int choiceindex = i;
                    choiceButtons[i].onClick.AddListener(() => StartCoroutine(SelectChoices(choicesresult, rewardresult, choiceindex)));
                }
                else
                {
                    choiceTexts[i].text = "? ? ?";
                    Vector3 choicePos = choiceButtons[i].transform.position;
                    choiceButtons[i].onClick.AddListener(() => spawnFloatingText.SpawnRandomText(choicePos));
                }                      
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SelectChoices(DialogueMaker nextDialogue, List<DialogueRewards> dialogueRewards,int choiceindex)
    {
        foreach (var button in choiceButtons)
        {
            if (button != null) button.interactable = false;
        }
        yield return StartCoroutine(FadePanel(choicesPanel,false));
        string nameforkey;
        if (saveCurrentSequencename != null)
        {
            nameforkey = saveCurrentSequencename;
        }
        else
        {
            nameforkey = "Unknown";
            Debug.Log("Unknown sequence");
        }
        IsShowChoices = false;
        string rewardkey = nameforkey+"UIIA"+" choice no. "+(choiceindex+1).ToString();

        if (StaticVariableForDialogue.claimedRewards == null)
        {
            StaticVariableForDialogue.claimedRewards = new List<string>();
        }

        if(nextDialogue != null)
        {
            if (dialogueRewards.Count() > 0 && !StaticVariableForDialogue.claimedRewards.Contains(rewardkey))
            {
                StaticVariableForDialogue.claimedRewards.Add(rewardkey);
                foreach(DialogueRewards reward in dialogueRewards)
                {
                    if(reward == null || string.IsNullOrEmpty(reward.rewardName)) continue;
                    if(reward.rewardTypes == DialogueRewards.RewardTypes.Boolean)
                    {
                        StaticVariableForDialogue.boolforDialogue[reward.rewardName] = true;
                    }
                    else if(reward.rewardTypes == DialogueRewards.RewardTypes.Integer)
                    {
                        int.TryParse(reward.rewardValue, out int newrewardValue);
                        if (StaticVariableForDialogue.statwithvalue.ContainsKey(reward.rewardName))
                        {
                            StaticVariableForDialogue.statwithvalue[reward.rewardName] += newrewardValue;
                        }
                        else
                        {
                            StaticVariableForDialogue.statwithvalue[reward.rewardName] = newrewardValue;
                        }
                    }
                }
            }
            else if(dialogueRewards.Count() > 0)
            {
                Debug.Log("Reward already claimed"+rewardkey);
            }
            nextDialogue.StartDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    public void ShowSkipButton()
    {
        skipPanel.SetActive(true);
    }

    public void SkipButtonWarning()
    {
        if (skipWarning.activeSelf)
        {
            skipWarning.SetActive(false);
        }
        else
        {
            skipWarning.SetActive(true);
        }
    }

    public void SkipConfirm()
    {
        _currentSequence.Skip();
    }
}

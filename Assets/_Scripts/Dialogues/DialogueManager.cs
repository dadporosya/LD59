using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueWindow;
    public Image portraitImage;
    public TextMeshProUGUI portraitTitle;
    public TextMeshProUGUI dialogueText;
    [HideInInspector] public ScalingText portraitScalingTitle;
    [HideInInspector] public ScalingText dialogueScalingText;
    
    private bool dialogueActive=false;
    private DialogueContainer dialogue;
    private Queue<DialogueNode> currentParagraphs = new Queue<DialogueNode>();

    [SerializeField] private float defaultTypeSpeed = 10f;
    [HideInInspector] public float typeSpeed;
    
    private enum TypingState { notStarted, active, finished }
    private TypingState typingState = TypingState.notStarted;
    
    private Coroutine typeCoroutine;
    private DialogueNode paragraph;
    
    [SerializeField] private const float MAX_TYPE_SPEED = 0.1f;

    public UnityEvent onDialogueEnd;

    private GameFlowManager gameFlowManager;
    
    private void Start()
    {
        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        
        EndDialogue();
        portraitScalingTitle = portraitTitle.GetComponent<ScalingText>();
        dialogueScalingText = dialogueText.GetComponent<ScalingText>();
        
        typeSpeed = defaultTypeSpeed;
    }

    private void Update()
    {
        if (typingState == TypingState.notStarted)
            return;

        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextParagraph();
        }
    }

    public void SetText(
        ScalingText scalingHolder,
        TextMeshProUGUI originalHolder, 
        string text)
    {
        if (scalingHolder)
        {
            scalingHolder.SetText(text);
            return;
        }

        originalHolder.text = text;
    }
    
    public void StartDialogue(DialogueContainer dialogueIn)
    {
        dialogueActive = true;
        dialogueWindow.SetActive(true);
        dialogue = dialogueIn;
        currentParagraphs = new Queue<DialogueNode>(dialogueIn.nodes);

        typingState = TypingState.notStarted;
        
        DisplayNextParagraph();
    }

    public void DisplayNextParagraph()
    {
        if (typingState == TypingState.active)
        {
            StopTyping();
            return;
        }
        if (typingState == TypingState.finished)
        {
            typingState = TypingState.notStarted;
            DisplayNextParagraph();
            return;
        }
        
        if (currentParagraphs.Count <= 0 || !dialogueActive)
        {
            EndDialogue();
            return;
        }
        
        paragraph = currentParagraphs.Dequeue();
        portraitImage.sprite = paragraph.speakerPortrait;
        SetText(portraitScalingTitle, portraitTitle, paragraph.speakerName);
        
        typeCoroutine = StartCoroutine(
            TypingDialogue(
                paragraph.text,
                dialogueScalingText,
                dialogueText,
                audioClips:paragraph.speakerVoiceClips
                )
            );

    }

    private IEnumerator TypingDialogue(
        string textIn,
        ScalingText scalingTextBox,
        TextMeshProUGUI defaultTextBox,
        List<AudioClip> audioClips=null
        )
    {
        typingState = TypingState.active;

        // Slice textIn by color tags
        Queue<string> textSlicesByColor = new Queue<string>();
        string colorTagPattern = @"(<color[^>]*>)";
        System.Text.RegularExpressions.Regex colorRegex = new System.Text.RegularExpressions.Regex(colorTagPattern);

        string[] parts = colorRegex.Split(HTML.RemoveUniqueTags(textIn));

// Recombine each tag with the text that follows it
        for (int i = 0; i < parts.Length; i++)
        {
            if (colorRegex.IsMatch(parts[i]) && i + 1 < parts.Length)
            {
                textSlicesByColor.Enqueue(parts[i] + parts[i + 1]);
                i++; // skip the next part, already consumed
            }
            else if (!string.IsNullOrEmpty(parts[i]))
            {
                textSlicesByColor.Enqueue(parts[i]);
            }
        }

        //h.Out(textSlicesByColor, textSlicesByColor.Count);

        string filteredText = "";
        
        void AddSplit()
        {
            if (textSlicesByColor.Count == 0) return;
            filteredText += textSlicesByColor.Dequeue();
        }
        
        AddSplit();
        
        string displayedText = "";
        int alphaIndex = 0;
        
        int characterCount = textIn.Length;
        // foreach (char c in textIn)

        bool playVoice = audioClips != null && audioClips.Count > 0;
        
        for (int i = 0; i < characterCount; i++)
        {
            char c =  textIn[i];
            
            // Process unique tags
            if (c == '<')
            {
                //todo: mb fix, so it detects only tags, not string like: hello! '< priiiiii, hhasdfhs >'
                int endIndex = textIn.IndexOf('>', i);
                if (endIndex != -1)
                {
                    string tag = textIn.Substring(i, endIndex - i + 1);
                    //h.Out(tag);
                    // Check if tag is in HTML.uniqueTags
                    bool isUniqueTag = false;
                    foreach (string uniqueTag in HTML.ALL_UNIQUE_TAGS)
                    {
                        if (tag.StartsWith("<" + uniqueTag))
                        {
                            isUniqueTag = true;
                            break;
                        }
                    }
                    
                    // If not a unique tag, skip it
                    if (!isUniqueTag)
                    {
                        alphaIndex += tag.Length;
                        i = endIndex;
                        continue;
                    }
                    
                    // process unique tags (e.g., pause)
                    // Process pause
                    if (tag.Contains(HTML.DIALOGUE_PAUSE_PLACEHOLDER))
                    {
                        int l = HTML.DIALOGUE_PAUSE_PLACEHOLDER.Length + 2; // pause + =

                        string pauseStr = textIn.Substring(i + l, endIndex - i - (l));
                        if (float.TryParse(pauseStr, out float pauseTime))
                        {
                            yield return new WaitForSeconds(pauseTime);
                            i = endIndex;
                            continue;
                        }
                    }
                    
                }
            }
            
            alphaIndex++;
            // SetText(dialogueScalingText, dialogueText, originalText);
            // displayedText = dialogueText.text.originalText.Insert(alphaIndex, HTML_ALPHA);

            displayedText = filteredText.Insert(alphaIndex, HTML.ALPHA);
            SetText(scalingTextBox, defaultTextBox, displayedText);
            
            // Sound FX of typing
            if (playVoice && !SFXManager.Instance.defaultAudioSource.isPlaying)
            {
                // SFXManager.Instance.PlayRandomClip(audioClips);
                //TODO
                AudioClip clip = h.RandChoice(audioClips);
                // SFXManager.Instance.PlayClip(AudioClipController.Generate(clip, dPitch:h.Range(0.05f), dVolume:h.Range(0.05f)));
                SFXManager.Instance.PlayClip(clip, volumeIn:3);
            }

            if (alphaIndex == filteredText.Length)
            {
                AddSplit();
            }
            
            yield return new WaitForSeconds(MAX_TYPE_SPEED / typeSpeed);
        }

        StopTyping();
    }

    private void StopTyping()
    {
        if (typeCoroutine != null) StopCoroutine(typeCoroutine);
        if (paragraph != null) SetText(dialogueScalingText, dialogueText, HTML.RemoveUniqueTags(paragraph.text));
        typingState = TypingState.finished;
    }
    

    public void EndDialogue()
    {
        typingState = TypingState.notStarted;
        dialogueActive = false;
        dialogueWindow.SetActive(false);
        dialogue = null;
        currentParagraphs.Clear();
        
        gameFlowManager.SetOnGame();
        onDialogueEnd?.Invoke();
    }
}

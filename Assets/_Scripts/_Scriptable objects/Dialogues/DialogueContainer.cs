using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueSpeaker", menuName = "Dialogues/DialogueSpeaker")]
public class DialogueSpeaker : ScriptableObject
{
    public string name;
    public Sprite portrait;
    public List<AudioClip> voiceClips = SFXManager.Instance.defaultDialogueVoiceList;
}

[Serializable]
public class DialogueNode
{
    [HideInInspector] public bool initialized = false;
    public bool speakerIsThis=true; // 
    public bool overWriteValues = true;
    [TextArea(3, 7)]
    public string text;
    
    [SerializeField] bool initFromScriptableObject=true;
    public DialogueSpeaker speaker;
    public GameObject speakerGameObject;
    public string speakerName;
    public Sprite speakerPortrait;
    public List<AudioClip> _speakerVoiceClips = new List<AudioClip>();

    public UnityEvent onNodeStart;
    public UnityEvent onNodeEnd;
    
    
    
    public List<AudioClip> speakerVoiceClips
    {
        get { return _speakerVoiceClips; }
        set
        {
            _speakerVoiceClips = value;
            if (_speakerVoiceClips == null || _speakerVoiceClips.Count == 0)
            {
                _speakerVoiceClips = SFXManager.Instance.defaultDialogueVoiceList;
            }
        }
    }

    public void Init()
    {
        initialized = false;
        if (speakerIsThis) return;
        
        if (initFromScriptableObject && speaker != null)
        {
            InitFromScriptableObject(speaker);
            return;
        }

        if (speakerGameObject)
        {
            InitFromGameObject(speakerGameObject);
            return;
        }

        if(h.CheckIfAllExist(speakerName, speakerPortrait))
        {
            InitFromDirectValues(speakerName, speakerPortrait, speakerVoiceClips);
            return;
        }
        
        speakerIsThis = true;
    }

    public void InitFromScriptableObject(DialogueSpeaker data)
    {
        initialized = true;
        if (overWriteValues || speaker == null) speaker = data;
        if (overWriteValues || string.IsNullOrEmpty(speakerName)) speakerName = speaker.name;
        if (overWriteValues || speakerPortrait == null) speakerPortrait = speaker.portrait;
        if (overWriteValues || _speakerVoiceClips.Count == 0) speakerVoiceClips = speaker.voiceClips;
    }
    public void InitFromGameObject(GameObject go)
    {
        initialized = true;
        if (go.TryGetComponent<Talkable>(out var talkable))
        {
            if (overWriteValues || string.IsNullOrEmpty(speakerName)) speakerName = talkable.Name;
            if (overWriteValues || speakerPortrait == null) speakerPortrait = talkable.Portrait;
            if (overWriteValues || _speakerVoiceClips.Count == 0) speakerVoiceClips = talkable.voiceClips;
        }
        else
        {
            if (overWriteValues || string.IsNullOrEmpty(speakerName)) speakerName = go.name;
            if (overWriteValues || speakerPortrait == null) speakerPortrait = go.GetComponent<SpriteRenderer>().sprite;
        }

        InitSpeaker();
    }

    public void InitFromDirectValues(
        string nameIn,
        Sprite spriteIn,
        List<AudioClip> voiceIn
        )
    {
        if (overWriteValues || string.IsNullOrEmpty(speakerName)) speakerName = nameIn;
        if (overWriteValues || speakerPortrait == null) speakerPortrait = spriteIn;
        if (overWriteValues || _speakerVoiceClips.Count == 0) speakerVoiceClips = voiceIn;
        InitSpeaker();
    }

    private void InitSpeaker()
    {
        speaker = ScriptableObject.CreateInstance<DialogueSpeaker>();
        speaker.name = speakerName;
        speaker.portrait = speakerPortrait;
    }
    
    void OnDisable() // ???
    {
        initialized = false;
        if (!initFromScriptableObject && !speakerIsThis) speaker = null;
    }
}


[CreateAssetMenu(fileName = "DialogueContainer", menuName = "Dialogues/DialogueContainer")]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
}


[Serializable]
public class SerializableDialogueContainer
{
    public List<DialogueNode> values = new List<DialogueNode>();

    public DialogueContainer Convert()
    {
        DialogueContainer d = ScriptableObject.CreateInstance<DialogueContainer>();
        d.nodes = values;
        return d;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Talkable : MonoBehaviour
{
    
    private GameFlowManager gameFlowManager;
    
    public string name;

    public string Name
    {
        get => name;
        set => name = value;
    }

    public Sprite portrait;
    public Sprite Portrait
    {
        get => portrait;
        set
        {
            portrait = value;
            GetComponent<SpriteRenderer>().sprite = portrait;
        }
    }
    
    public List<AudioClip> voiceClips;
    
    [HideInInspector] public DialogueManager dialogueManager;
    
    [Header("Simple Monologues")]
    public List<ListOfStrWrapper> simpleMonologues = new List<ListOfStrWrapper>();
    
    [Header("Serializable Dialogues")]
    public List<SerializableDialogueContainer> rawDialogues = new List<SerializableDialogueContainer>();
    
    [Header("Dialogues' Scriptable Objects")]
    public List<DialogueContainer> dialogues = new List<DialogueContainer>();
        
    private void Awake()
    {
        gameFlowManager =  FindFirstObjectByType<GameFlowManager>();
        
        if (name == default)
            name = gameObject.name;
        if (portrait == null)
            portrait = GetComponentInChildren<SpriteRenderer>().sprite;
        
        if (!dialogueManager) dialogueManager = FindFirstObjectByType<DialogueManager>();
        
        if (voiceClips==null || voiceClips.Count==0)voiceClips =  SFXManager.Instance.defaultDialogueVoiceList;
        // h.Out(gameObject.name, voiceClips);
        
        h.ForEach(rawDialogues, (d) =>
        {
            dialogues.Add(d.Convert());
        });
        
        h.ForEach(simpleMonologues, (m) =>
        {
            DialogueContainer d = ScriptableObject.CreateInstance<DialogueContainer>();
            
            h.ForEach(m.values, (s) =>
            {
                DialogueNode node = new DialogueNode();
                node.speakerIsThis = true;
                node.text = s;
                node.InitFromGameObject(gameObject);
                d.nodes.Add(node);
            });
            dialogues.Add(d);
        });
        
        // Initialize all dialogue nodes that haven't been initialized yet
        h.ForEach(dialogues, (dialogue) =>
        {
            if (!dialogue) return;
            
            // h.Out(dialogue);
            h.ForEach(dialogue.nodes, (node) =>
            {
                node.Init();
                if (!node.initialized || node.speakerIsThis)
                {
                    node.InitFromGameObject(gameObject);
                }
            });
        });
    }

    public void Talk(int i = -1)
    {
        DialogueContainer dialogue;
        if (i >= 0 && i < dialogues.Count)  dialogue =  dialogues[i];
        else dialogue = h.RandChoice(dialogues);
        dialogueManager.StartDialogue(dialogue);
        
        gameFlowManager.SetOnPause();
    }

}

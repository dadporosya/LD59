using System;
using UnityEngine;

public class Heart : MonoBehaviour, IAction
{
    [SerializeField] private Sprite _actionIcon;
    public int BMPPerBeat = 10;
    private BPMManager bpmManager;
    public Sprite actionIcon
    {
        get
        {
            if (!_actionIcon)
            {
                _actionIcon = GetComponentInChildren<SpriteRenderer>().sprite;
            }
            return _actionIcon;
        }
        set { _actionIcon = value; }
    }

    private void Awake()
    {
        if (!bpmManager) bpmManager = FindFirstObjectByType<BPMManager>();
        if (!actionIcon)  actionIcon = GetComponent<SpriteRenderer>().sprite;
    }

    public void Action()
    {
        bpmManager.ChangeBMP(BMPPerBeat);
    }
}

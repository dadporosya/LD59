using System;
using UnityEngine;

public class Heart : MonoBehaviour, IAction
{
    [SerializeField] private Sprite _actionIcon;
    public int BMPPerBeat = 10;
    private GameManager gameManager;
    public Sprite actionIcon
    {
        get { return _actionIcon; }
        set { _actionIcon = value; }
    }

    private void Start()
    {
        if (!gameManager) gameManager = FindFirstObjectByType<GameManager>();
        if (!actionIcon)  actionIcon = GetComponent<SpriteRenderer>().sprite;
    }

    public void Action()
    {
        gameManager.ChangeBMP(BMPPerBeat);
    }
}

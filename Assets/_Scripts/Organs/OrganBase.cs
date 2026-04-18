using System;
using UnityEngine;

public class OrganBase : MonoBehaviour, IAction
{
    public enum LocationInAquarium
    {
        In, Out
    }
    public LocationInAquarium locationInAquarium = LocationInAquarium.In;
    
    [SerializeField] private Sprite _actionIcon;
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

    public virtual void Awake()
    {
        if (!actionIcon)  actionIcon = GetComponent<SpriteRenderer>().sprite;
    }

    public virtual void Action()
    {
        
    }

    public bool IsIn()
    {
        return  locationInAquarium == LocationInAquarium.In;
    }
}

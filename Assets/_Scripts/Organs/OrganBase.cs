using System;
using UnityEngine;

public class OrganBase : MonoBehaviour, IAction
{
    public enum LocationInAquarium
    {
        In, Out, OutIn
    }

    
    // in - floatin in aqua
    // out - completely outside
    // outin - outside, but in the same parent
    public LocationInAquarium locationInAquarium = LocationInAquarium.In;
    public bool overlapAquarium = false;
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

        UpdateOverlap(overlapAquarium);
    }

    public void UpdateOverlap(bool overlap)
    {
        if (locationInAquarium == LocationInAquarium.In) return;
        
        overlapAquarium = overlap;
        if (overlapAquarium)
        {
            // h.UpdateLayersRecursively(transform, Preferences.aquariumOrderInLayer);
            h.SetSpriteMaskInteractionRecursively(transform, SpriteMaskInteraction.None);
        }
        else
        {
            h.SetSpriteMaskInteractionRecursively(transform, SpriteMaskInteraction.VisibleOutsideMask);
        }
    }

    public virtual void Action()
    {
        
    }

    public bool IsIn()
    {
        return  locationInAquarium == LocationInAquarium.In;
    }
}

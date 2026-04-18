using UnityEngine;

public class Brain : MonoBehaviour, IAction
{
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
    
    public int cellsPerTap = 10;
    private CellsManager cellsManager;
    

    private void Awake()
    {
        if (!cellsManager) cellsManager = FindFirstObjectByType<CellsManager>();
        if (!actionIcon)  actionIcon = GetComponent<SpriteRenderer>().sprite;
    }

    public void Action()
    {
        cellsManager.ChangeCellCount(cellsPerTap);
    }
    
    
}

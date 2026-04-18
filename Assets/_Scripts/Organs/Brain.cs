using UnityEngine;

public class Brain : OrganBase
{
    public int cellsPerTap = 10;
    private CellsManager cellsManager;

    public override void Awake()
    {
        base.Awake();
        if (!cellsManager) cellsManager = FindFirstObjectByType<CellsManager>();
    }

    public override void Action()
    {
        base.Action();
        cellsManager.ChangeCellCount(cellsPerTap);
    }
}

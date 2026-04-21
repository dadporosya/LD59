using System.Collections.Generic;
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
        SFXManager.Instance.PlayRandomClip(new List<string>()
        {
            "Audio/SFX/brainActive",
        }, volumeIn:0.5f);
        cellsManager.ChangeCellCount(cellsPerTap);
    }
}

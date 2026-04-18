using System;
using UnityEngine;
using M=System.MathF;
public class CellsManager : MonoBehaviour
{
    [SerializeField] private int _cellCount = 0;
    public int cellCount
    {
        get { return _cellCount; }
        set
        {
            _cellCount = value;
        }
    }
    
    public int maxCellCount=0;
    [SerializeField] private int initialMaxCell=50;
    public int level=1;
    
    public Bar cellBar;

    [SerializeField] private float levelMult=1.25f;
    [SerializeField] private float levelConst = 10;

    private void Start()
    {
        if (!cellBar) cellBar = GameObject.Find("CellBar").GetComponent<Bar>();
        GenerateLevel(levelIn:level);
    }

    public void UpdateCellBar()
    {
        cellBar.Init(cellCount, maxCellCount);
    }

    public void GenerateLevel(bool nextLevel=true, int levelIn=-1)
    {
        cellCount = h.Max(0, cellCount-maxCellCount);
        
        if (nextLevel) level++;
        if (levelIn > 0) level = levelIn; 
        
        maxCellCount = (int)M.Ceiling(initialMaxCell * M.Pow(levelMult, level - 1) + levelConst * (level-1));
        UpdateCellBar();
        CheckOverflow();
    }

    public void SetCellCount(int value)
    {
        cellCount = value;
        UpdateCellBar();
        CheckOverflow();
    }

    public void ChangeCellCount(int value)
    {
        cellCount += value;
        UpdateCellBar();
        CheckOverflow();
    }

    public void CheckOverflow()
    {
        // new organ or smt else
        if (cellCount < maxCellCount) return;
        
        GenerateLevel();
        
    }

}

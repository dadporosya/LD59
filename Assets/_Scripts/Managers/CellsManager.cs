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
    [SerializeField] private int initialMaxCell=10;
    public int level=1;
    
    public Bar cellBar;

    [SerializeField] private float levelMult=1.2f;
    [SerializeField] private float levelConst = 5;

    private void Start()
    {
        if (!cellBar) cellBar = GameObject.Find("CellBar").GetComponent<Bar>();
        GenerateLevel(levelIn:level);
    }

    public void GenerateLevel(bool nextLevel=true, int levelIn=-1)
    {
        cellCount = h.Max(0, cellCount-maxCellCount);
        
        if (nextLevel) level++;
        if (levelIn > 0) level = levelIn; 
        
        maxCellCount = (int)M.Ceiling(initialMaxCell * M.Pow(levelMult, level - 1) + levelConst * (level-1));
        cellBar.Init(cellCount, maxCellCount);

        CheckOverflow();
    }

    public void SetCellCount(int value)
    {
        cellCount = value;
        CheckOverflow();
    }

    public void ChangeCellCount(int value)
    {
        cellCount += value;
        CheckOverflow();
    }

    public void CheckOverflow()
    {
        // new organ or smt else
        if (cellCount < maxCellCount) return;
        
        GenerateLevel();
        
    }

}

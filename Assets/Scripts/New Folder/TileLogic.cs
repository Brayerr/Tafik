using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic
{
    public int State { get; private set; } // 0=empty 1=Dug 2=trail  -1=enemy area
    public Vector2Int Position { get; }

    public TileLogic()
    {
        
    }
    public TileLogic(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public void SetBorder()
    {
        State = 1;
    }

    public void SetDug()
    {
        State = 1;
        TileBoardLogic.AddDugTile();
    }
    public void SetTrail()
    {
        State = 2;
    }
    public TileLogic SetState(int stateNum)
    {
        State = stateNum;
        return this;
    }
}

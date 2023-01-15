using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardLogic
{
    Vector2Int dimen;
    public TileLogic[,] Tiles { get; }

    public TileBoardLogic(Vector2Int d)
    {
        dimen = d;
        Tiles = new TileLogic[dimen.x, dimen.y];
        for (int i = 0; i < dimen.y; i++)
        {
            for (int j = 0; j < dimen.x; j++)
            {
                Tiles[j, i] = new();
            }
        }
        Debug.Log($"created array od {dimen.x}{dimen.y}");

    }
    public void Frame()
    {
        for (int i = 0; i < dimen.y; i++)
        {
            Tiles[i, 0].SetFilled();
            Tiles[i, dimen.y - 1].SetFilled();
            Tiles[0, i].SetFilled();
            Tiles[dimen.x - 1, i].SetFilled();
        }

    }
}

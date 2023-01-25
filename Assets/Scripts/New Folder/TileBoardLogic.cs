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
        Debug.Log($"created array of {dimen.x}{dimen.y}");

    }
    public void Frame()
    {

        for (int i = 0; i < dimen.x; i++)
        {
            Tiles[i, 0].SetFilled();
            Tiles[i, dimen.y - 1].SetFilled();
        }
        for (int i = 0; i < dimen.y; i++)
        {
            Tiles[0, i].SetFilled();
            Tiles[dimen.x - 1, i].SetFilled();
        }

    }

    public void SetTileBad(int posX, int posY)
    {
        if (Tiles[posX, posY].State != 0)
            return;
        Tiles[posX, posY].SetState(-1);
        SetTileBad(posX + 1, posY);
        SetTileBad(posX - 1, posY);
        SetTileBad(posX, posY + 1);
        SetTileBad(posX, posY - 1);
    }

    public void Convert()
    {
        foreach (var item in Tiles)
        {
            if (item.State == 0)
                item.SetFilled();
            if (item.State == -1)
                item.SetState(0);
        }
    }
}

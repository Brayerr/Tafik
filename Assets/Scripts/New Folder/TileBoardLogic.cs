using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardLogic
{
    public Vector2Int Dimen { get; }
    public TileLogic[,] Tiles { get; }

    static public event Action OnConvert;

    public TileBoardLogic(Vector2Int d)
    {
        Dimen = d;
        Tiles = new TileLogic[Dimen.x, Dimen.y];
        for (int i = 0; i < Dimen.y; i++)
        {
            for (int j = 0; j < Dimen.x; j++)
            {
                Tiles[j, i] = new(j, i);
            }
        }
        Debug.Log($"created array of {Dimen.x}{Dimen.y}");

        PlayerLogic.OnTrailStart += StartTrail;
        PlayerLogic.OnTrailEnd += EndTrail;
        EnemyManager.OnThreatenComplete += Convert;
    }
    public void Frame()
    {

        for (int i = 0; i < Dimen.x; i++)
        {
            Tiles[i, 0].SetFilled();
            Tiles[i, Dimen.y - 1].SetFilled();
        }
        for (int i = 0; i < Dimen.y; i++)
        {
            Tiles[0, i].SetFilled();
            Tiles[Dimen.x - 1, i].SetFilled();
        }

    }

    void StartTrail()
    {
        Debug.Log("start trail called");
        PlayerLogic.OnPlayerTileChanged += Trail;
    }

    void EndTrail()
    {
        Debug.Log("end trail called");
        PlayerLogic.OnPlayerTileChanged -= Trail;
        EnemyManager.ThreatenTile();
    }

    void Trail(TileLogic t)
    {
        t.SetTrail();
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
            if (item.State == 0 || item.State == 2)
                item.SetFilled();
            if (item.State == -1)
                item.SetState(0);
        }
        OnConvert.Invoke();
    }
}

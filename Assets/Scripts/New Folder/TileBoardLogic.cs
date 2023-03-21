using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoardLogic
{
    static public int TotalTiles { get; private set; }
    static public int DugTiles { get; private set; }
    public Vector2Int Dimen { get; }
    public TileLogic[,] Tiles { get; }

    static public event Action OnConvert;

    public TileBoardLogic(Vector2Int d)
    {
        Dimen = d;

        TotalTiles = (Dimen.x - 2) * (Dimen.y - 2);
        if (TotalTiles < 1)
            TotalTiles = 1;
        DugTiles = 0;

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
            Tiles[i, 0].SetBorder();
            Tiles[i, Dimen.y - 1].SetBorder();
        }
        for (int i = 0; i < Dimen.y; i++)
        {
            Tiles[0, i].SetBorder();
            Tiles[Dimen.x - 1, i].SetBorder();
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
                item.SetDug();
            if (item.State == -1)
                item.SetState(0);
        }
        Debug.Log($"{DugTiles} / {TotalTiles} - {DugTiles * 100 / TotalTiles} %"); //prints dig progress, but includes outer area
        OnConvert.Invoke();
    }

    static public void AddDugTile()
    {
        DugTiles++;
    }
}

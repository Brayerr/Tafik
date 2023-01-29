using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Vector2 position { get; protected set; }
    TileLogic tile;

    public abstract void Move();

    virtual protected void Start()
    {
        tile = new TileLogic();

        EnemyManager.AddEnemy(this);
        position = new Vector2(transform.position.x, transform.position.z);
    }

    virtual protected void Update()
    {
        Move();
        //UpdateGridPosition();
    }
    
    //might have problems
    virtual protected void UpdateGridPosition()
    {
        if (tile != TileBoardManager.Board.Tiles[(int)position.x, (int)position.y].SetState(-1))
        {
            tile.SetState(0);
            tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
        }

    }

}

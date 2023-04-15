using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Vector2 position { get; protected set; }
    TileLogic tile;
    public byte state { get; protected set; } = 0;

    static public event Action<int,int> OnEnemyTrailCollision; 

    public abstract void Move();

    virtual protected void Start()
    {
        tile = new TileLogic();

        EnemyManager.AddEnemy(this);
        //for now, initial position is transform position
        position = new Vector2(transform.position.x, transform.position.z);
    }

    virtual protected void Update()
    {
        Move();
        UpdateGridPosition();
    }

    //might have problems
    virtual protected void UpdateGridPosition()
    {
        if (tile != TileBoardManager.Board.Tiles[(int)position.x, (int)position.y])
        {
            tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
            if (tile.State == 2)
            {
                OnEnemyTrailCollision.Invoke(tile.Position.x, tile.Position.y);
            }
        }

    }

    public abstract void RotateEnemy();

}
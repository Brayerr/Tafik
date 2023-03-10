using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class EnemyManager
{
    static List<Enemy> enemies = new List<Enemy>();

    static public event Action OnThreatenComplete;

    static public void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }

    static public void ThreatenTile()
    {
        foreach (Enemy e in enemies)
        {
            Debug.Log("threatening");
            TileBoardManager.Board.SetTileBad((int)e.position.x, (int)e.position.y);
        }
        OnThreatenComplete.Invoke();
    }
}

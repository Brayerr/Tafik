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

    static public T FindEnemy<T>(Vector2 sPos, float range) where T : Enemy
    {
        foreach (var item in enemies)
        {
            if (item.GetType() == typeof(T))
            {
                if (item.position.InRange(sPos, range))
                    return (T)item;
            }
        }
        return null;
    }
}

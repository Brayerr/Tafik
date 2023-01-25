using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class EnemyManager
{
    static List<Enemy> enemies = new List<Enemy>();

    static public void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }

    static public void ThreatenTile()
    {
        foreach (Enemy e in enemies)
        {
            TileBoardManager.Board.SetTileBad((int)e.position.x, (int)e.position.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension
{
    public static bool InRange(this Vector2 pos1, Vector2 pos2, float range)
    {
        Vector2 delta = pos2 - pos1;
        if (delta.sqrMagnitude > range * range)
        {
            return false;
        }
        return true;
    }

    public static Vector2Int RoundVector(this Vector2 pos)
    {
        return new Vector2Int((int)pos.x, (int)pos.y);
    }
    public static Vector2Int RoundVector(this Vector3 pos)
    {
        return new Vector2Int((int)pos.x, (int)pos.z);
    }
}

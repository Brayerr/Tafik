using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension
{
    public static bool InRange(this Vector2 pos1, Vector2 pos2, float range)
    {
        Vector2 d = pos2 - pos1;
        if (d.sqrMagnitude > range * range)
        {
            return false;
        }
        return true;
    }
}

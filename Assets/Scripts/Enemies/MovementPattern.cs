using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct MovementPattern
{
    public float speedMin;
    public float speedMax;
    public float durationMin;
    public float durationMax;
    public bool isTurning;

    /// <summary>
    /// Will return cardinals if true, intercardinals if false
    /// </summary>
    public bool isCardinal;

    public float GetSpeed()
    {
        return Random.Range(speedMin, speedMax);
    }
    public float GetDuration()
    {
        return Random.Range(durationMin, durationMax);
    }

    public Vector2 GetDirection()
    {
        return isCardinal ? GetDirectionCardinal() : GetDirectionIntercardinal();
    }

    private Vector2 GetDirectionCardinal()
    {
        return Random.Range(0, 4) switch
        {
            0 => Vector2.up,
            1 => Vector2.right,
            2 => Vector2.down,
            3 => Vector2.left,
            _ => Vector2.zero,
        };
    }

    private Vector2 GetDirectionIntercardinal()
    {
        return Random.Range(0, 4) switch
        {
            0 => new(1, 1),
            1 => new(1, -1),
            2 => new(-1, -1),
            3 => new(-1, 1),
            _ => Vector2.zero,
        };
    }
}

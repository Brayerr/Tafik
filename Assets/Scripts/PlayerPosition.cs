using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public Vector2Int tilePosition { get; set; }

    [SerializeField] Tile.State _state;
    bool _isTrailing = false;

    static public event Action<Tile> OnTrailStart;
    static public event Action AreaFilled;
    static public event Action onTrailStart2;

    private void Start()
    {
        Tile.OnPlayerTileChanged += CheckTileState;
        Tile.OnPlayerTileChanged += UpdatePosition;
    }

    void CheckTileState(Tile t)
    {
        if (_state != t.TileState) //true if starts/stops filling
        {
            _state = t.TileState;

            if (_state == Tile.State.empty) //true if starts filling
            {
                OnTrailStart.Invoke(t);
                onTrailStart2.Invoke();
                ToggleTrail();
            }

            if (_state == Tile.State.filled) //true if stops filling
            {
                AreaFilled.Invoke();
                ToggleTrail();
            }
        }
    }

    void UpdatePosition(Tile t)
    {
        tilePosition = t.Position;
    }

    void ToggleTrail()
    {
        _isTrailing = !_isTrailing;
    }
}

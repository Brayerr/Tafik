using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    [SerializeField] float space; //space between instances
    [SerializeField] Vector2Int dimen; //board size
    [SerializeField] Tile[,] tiles;
    [SerializeField] GameObject prefabObject;
    Quaternion krok;

    [SerializeField] List<Tile> trail = new();

    Area _area1 = new Area(1);
    Area _area2 = new(2);

    private void Start()
    {
        PlayerPosition.OnTrailStart += SetTrailStart;
        PlayerPosition.AreaFilled += TrailBrain;
        //SpawnGrid();
    }

    [ContextMenu("Spawn Grid")]
    public void SpawnGrid()
    {
        tiles = new Tile[dimen.x, dimen.y];
        for (int i = 1; i <= dimen.y; i++)
        {
            for (int j = 1; j <= dimen.x; j++)
            {
                GameObject go = Instantiate(prefabObject, new Vector3(j * space - space / 2, 0, i * space - (space / 2)), krok, this.transform);
                Tile t = go.GetComponent<Tile>();
                if (!t)
                {
                    Debug.Log("tile component missing");
                    return;
                }
                tiles[j - 1, i - 1] = t;
                t.SetPosition(j - 1, i - 1);
                if (i == 1 || i == dimen.y || j == 1 || j == dimen.x)
                    t.SetTileState(Tile.State.filled);
            }
        }
    }

    void CompareAreaSize(Tile t)
    {
        CheckIfFilled(new Vector2Int(), _area1);
        CheckIfFilled(new Vector2Int(), _area2);
    }

    void CheckIfFilled(Vector2Int pos, Area areaSize)
    {
        if (tiles[pos.x, pos.y].TileState != Tile.State.empty)
            return;

        areaSize.IncreaseSize();
        if (areaSize.ID == 1)
            tiles[pos.x, pos.y].SetTileState(Tile.State.tofill1);
        else tiles[pos.x, pos.y].SetTileState(Tile.State.tofill2);

        CheckIfFilled(pos + Vector2Int.right, areaSize);
        CheckIfFilled(pos + Vector2Int.up, areaSize);
        CheckIfFilled(pos + Vector2Int.left, areaSize);
        CheckIfFilled(pos + Vector2Int.down, areaSize);

    }

    void SetTrailStart(Tile t)
    {
        tiles[t.Position.x, t.Position.y].SetTileState(Tile.State.trailStart);
        trail.Add(t);
        Tile.OnPlayerTileChanged += SetTrailTile;
    }

    void SetTrailTile(Tile t)
    {
        tiles[t.Position.x, t.Position.y].SetTileState(Tile.State.trail);
        trail.Add(t);
    }

    void TrailBrain()
    {
        _area1.ResetSize();
        _area2.ResetSize();

        for (int i = 0; i < trail.Count - 1; i++)
        {
            Vector2Int direction = new Vector2Int(trail[i + 1].Position.x - trail[i].Position.x, trail[i + 1].Position.y - trail[i].Position.y);
            if (direction.Equals(Vector2Int.right))
            {
                CheckIfFilled(new Vector2Int(trail[i].Position.x, trail[i].Position.y) + Vector2Int.up, _area1);
                CheckIfFilled(new Vector2Int(trail[i].Position.x, trail[i].Position.y) + Vector2Int.down, _area2);
            }
        }

        if (_area1.Size >= _area2.Size)
            ConvertArea(Tile.State.tofill1);
        else ConvertArea(Tile.State.tofill2);
    }

    void ConvertArea(Tile.State s)
    {
        foreach (Tile tile in tiles)
        {
            switch (tile.TileState)
            {
                case Tile.State.trail:
                    tile.SetTileState(Tile.State.filled);
                    break;
                case Tile.State.trailStart:
                    tile.SetTileState(Tile.State.filled);
                    break;
                case Tile.State.tofill1:
                    if (s == Tile.State.tofill1)
                        tile.SetTileState(Tile.State.filled);
                    else tile.SetTileState(Tile.State.empty);
                    break;
                case Tile.State.tofill2:
                    if (s == Tile.State.tofill2)
                        tile.SetTileState(Tile.State.filled);
                    else tile.SetTileState(Tile.State.empty);
                    break;
                default:
                    break;
            }
        }
    }
}

class Area
{
    public int ID { get; private set; }
    public int Size { get; private set; }

    public Area(int id)
    {
        ID = id;
    }

    public void ResetSize()
    {
        Size = 0;
    }
    public void IncreaseSize()
    {
        Size++;
    }
}

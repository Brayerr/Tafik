using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class TileBoardManager : MonoBehaviour
{
    [SerializeField] float space; //space between instances
    [SerializeField] Vector2Int dimensions;

    [SerializeField] UnityEngine.GameObject EmptyPrefabObject;
    [SerializeField] UnityEngine.GameObject FilledPrefabObject;
    [SerializeField] Sprite[] gridSprites = new Sprite[1280];
    Vector3 tileRot = new(90, 0, 0);


    static public TileBoardLogic Board { get; private set; }
    UnityEngine.GameObject[,] _tileGraphics;

    [SerializeField] List<Vector3> specialTiles = new List<Vector3>();


    [ContextMenu("Spawn Grid")]
    public void SpawnGrid()
    {
        Board = new(dimensions);
        SetSpecialTiles();
        Board.Frame();
        _tileGraphics = new UnityEngine.GameObject[dimensions.x, dimensions.y];


        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                if (Board.Tiles[j, i].State == 0)
                    _tileGraphics[j, i] = Instantiate(EmptyPrefabObject, new Vector3((j + 1) * space - (space / 2), 1, (i + 1) * space - (space / 2)), Quaternion.Euler(new(90, 0, 0)), this.transform);
                else
                {
                    _tileGraphics[j, i] = Instantiate(FilledPrefabObject, new Vector3((j + 1) * space - (space / 2), 0, (i + 1) * space - (space / 2)), Quaternion.Euler(new(90, 0, 0)), this.transform);
                }
            }
        }
        PlayerLogic.OnTrailStart += StartDrawTrail;
        PlayerLogic.OnTrailEnd += EndDrawTrail;

        TileBoardLogic.OnConvert += DrawFill;
    }

    [ContextMenu("Initialize sprites in Grid")]
    public void AddSpritesToGrid()
    {
        UnityEngine.Object[] sprite = Resources.LoadAll($"substance/sand_V2_graph_0/sand_V2_basecolor");
        int count = 1;
        //for (int i = 0; i < gridSprites.Length; i++)
        //{
        //    gridSprites[i] = sprite[i + 1] as Sprite;
        //}

        foreach (var item in _tileGraphics)
        {
            SpriteRenderer _spriteRenderer = item.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = sprite[count] as Sprite;
            _spriteRenderer.size = new Vector2(1f, 1f);
            //item.transform.Rotate(tileRot);
            count++;
        }
    }

    public void SetSpecialTiles()
    {
        int count = 0;
        foreach (var item in specialTiles)
        {
            for (int i = (int)item.y ; i <= (int)item.z; i++)
            {
                Board.Tiles[i, (int)item.x].SetBorder();
            }
            count++;
        }
    }

    void StartDrawTrail()
    {
        PlayerLogic.OnPlayerTileChanged += DrawTrail;
    }

    void EndDrawTrail()
    {
        PlayerLogic.OnPlayerTileChanged -= DrawTrail;
    }

    public void DrawTrail(TileLogic t)
    {
        _tileGraphics[t.Position.x, t.Position.y].transform.Translate(Vector3.forward);
    }

    public void DrawFill()
    {
        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                if (Board.Tiles[j, i].State == 1)
                    _tileGraphics[j, i].transform.position = new Vector3(j + 0.5f, 0, i + 0.5f);
            }
        }
    }

    // area closer
    // an algorithm that records a trail of all the points on the board where the player turned
    // start from the top left turning point, looks for the next point on the same x.
    // use that as first rect width. go down in y within this x range and look for turning points
    // use that y for height. build a rect with those dimensions and mark every tile within the rect to be filled
    // if the corners of that rect arent in the list, add them to the list.
    // keep going looking for corners after the 2nd turning point

}

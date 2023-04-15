using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class TileBoardManager : MonoBehaviour
{

    [SerializeField] float space; //space between instances
    [SerializeField] Vector2Int dimensions;

    [SerializeField] UnityEngine.GameObject EmptyPrefabObject;
    [SerializeField] UnityEngine.GameObject FilledPrefabObject;

    [SerializeField] float trailCollapseSpeed = 0.06f;
    [SerializeField] Vector2 burnRange = new(0.7f, 0.8f);
    [SerializeField] Vector3 collapseTimes = new(0.5f, 0.5f, 0.5f);
    [SerializeField] float fillTime = 1.5f;


    Vector3 tileRot = new(90, 0, 0);


    static public TileBoardLogic Board { get; private set; }
    UnityEngine.GameObject[,] _tileGraphics;

    [SerializeField] List<Vector3> specialTiles = new List<Vector3>();

    IEnumerator CollapseEnumarator;
    static public event Action<int, int> OnCollapseStep;
    Sequence fillSequence;
    Sequence collapseSequence;
    bool orQuakeStarted;

    private void Start()
    {
        SpawnGrid();
        TileBoardLogic.OnCreatedList += ComparePickupPositions;
        TileBoardLogic.OnTileCollapse += TileCollapseVisual;
        GameManager.OnFakeRestart += FakeRestart;
        Danger.OnErupt += Eruption;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) OrQuake();
    }

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
            for (int i = (int)item.y; i <= (int)item.z; i++)
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
        //_tileGraphics[t.Position.x, t.Position.y].transform.Translate(new(0, 0, 0.5f));
        _tileGraphics[t.Position.x, t.Position.y].transform.DOMoveY(burnRange.y, 1);
    }

    public void DrawFill()
    {
        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                if (Board.Tiles[j, i].State == 1 && _tileGraphics[j, i].transform.position.y != 0)
                {
                    fillSequence = DOTween.Sequence().Append(_tileGraphics[j, i].transform.DOMoveY(burnRange.y, fillTime))
                        .Append(_tileGraphics[j, i].transform.DOMoveY(0, 0));
                    //_tileGraphics[j, i].transform.DOMoveY(0, 1f).SetEase(Ease.OutExpo);
                    //_tileGraphics[j, i].transform.position = new Vector3(j + 0.5f, 0, i + 0.5f);
                }
            }
        }
    }

    public void TileCollapseVisual(int posX, int posY)
    {
        _tileGraphics[posX, posY].transform.DOKill();
        //_tileGraphics[posX, posY].transform.DOMoveY(1, 1.5f).SetEase(Ease.OutElastic);
        //DOTween.Sequence(_tileGraphics[posX, posY])
        collapseSequence = DOTween.Sequence().Append(_tileGraphics[posX, posY].transform.DOMoveY((burnRange.x + burnRange.y) / 2, collapseTimes.x))
            .AppendInterval(collapseTimes.y)
            .Append(_tileGraphics[posX, posY].transform.DOMoveY(1, collapseTimes.z));
        collapseSequence.Play();
        CollapseEnumarator = CollapseDelay(posX, posY);
        StartCoroutine(CollapseEnumarator);
    }

    IEnumerator CollapseDelay(int posX, int posY)
    {
        yield return new WaitForSeconds(trailCollapseSpeed);
        OnCollapseStep.Invoke(posX, posY);

    }

    void ComparePickupPositions(List<Vector2Int> tilesPositions)
    {
        foreach (var item in tilesPositions)
        {
            foreach (var pickup in PickupManager.pickupList)
            {
                if (pickup.position == item) pickup.RevealPickup();
            }
        }
    }

    void FakeRestart()
    {
        foreach (var item in _tileGraphics)
        {
            Destroy(item);
        }
        PlayerLogic.OnTrailStart -= StartDrawTrail;
        PlayerLogic.OnTrailEnd -= EndDrawTrail;

        TileBoardLogic.OnConvert -= DrawFill;
        SpawnGrid();
    }

    [ContextMenu("OrQuake")]
    void OrQuake()
    {
        //if (orQuakeStarted)
        //    return;
        //orQuakeStarted = true;
        foreach (var item in _tileGraphics)
        {
            if (Board.Tiles[(int)item.transform.position.x, (int)item.transform.position.z].State == 0)
            {
                item.transform.DOMoveY(Random.Range(1, 1.45f), Random.Range(0.2f, 0.4f)).SetLoops(6, LoopType.Yoyo);
            }
        }
        StartCoroutine(Fix());
    }
    IEnumerator Fix()
    {
        yield return new WaitForSeconds(2.5f);
        FixGrid();
    }

    void FixGrid()
    {
        foreach (var item in _tileGraphics)
        {
            if (Board.Tiles[(int)item.transform.position.x, (int)item.transform.position.z].State == 0)
            {
                item.transform.DOMoveY(1, 1);
            }
        }
    }

    [ContextMenu("Earthquake")]
    void Earthquake()
    {
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2);
        _tileGraphics[Random.Range(0, dimensions.x - 1), Random.Range(0, dimensions.y - 1)].transform.DOMoveY(1.5f, 0.4f).SetEase(Ease.Flash, 2).onComplete = Earthquake;
    }

    void Eruption(Vector2Int epicenter, Vector2Int area)
    {
        StartCoroutine(Ripple(epicenter, area, 0.1f));
    }
    //never considers orientation
    IEnumerator Ripple(Vector2Int epicenter, Vector2Int area, float delay)
    {
        int counter = 0;
        for (int i = epicenter.y + (area.y - 1) / 2; i >= epicenter.y - 1 - (area.y - 1) / 2 + area.y % 2; i--)
        {
            for (int j = epicenter.x - (area.x - 1) / 2; j <= epicenter.x + 1 + (area.x - 1) / 2 - area.x % 2; j++)
            {
                TileErupt(j, i);
                counter++;
                if (counter == area.x * area.y)
                {
                    TileErupt2(j, i);
                }
            }
            yield return new WaitForSeconds(delay);
        }
        Debug.Log(counter);
    }

    void TileErupt(int x, int y)
    {
        _tileGraphics[x, y].transform.DOMoveY(10, 1.5f).SetLoops(2, LoopType.Yoyo);
    }
    void TileErupt2(int x, int y)
    {
        _tileGraphics[x, y].transform.DOMoveY(10, 1.5f).SetLoops(2, LoopType.Yoyo).onComplete = OrQuake;
    }

    // area closer
    // an algorithm that records a trail of all the points on the board where the player turned
    // start from the top left turning point, looks for the next point on the same x.
    // use that as first rect width. go down in y within this x range and look for turning points
    // use that y for height. build a rect with those dimensions and mark every tile within the rect to be filled
    // if the corners of that rect arent in the list, add them to the list.
    // keep going looking for corners after the 2nd turning point

}

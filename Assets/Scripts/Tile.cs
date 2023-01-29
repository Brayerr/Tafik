using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int Position { get; private set; }
    [SerializeField] Renderer _renderer;
    public SpriteRenderer _spriteRenderer;
    [SerializeField] float speed = 3;
    public Sprite sprite;
    TileBoard levelBoard;
    public State TileState { get => tileState; set => tileState = value; }
    [SerializeField]
    private State tileState = State.empty;

    Vector2Int trailStart;
    Vector2Int trailEnd;

    bool isCounted = false;

    static public event Action<Tile> OnPlayerTileChanged;
    static public event Action<Tile> OnGrappleTileChanged;
    public static event Action OnShootFinished;


    public enum State
    {
        empty,
        trail,
        trailStart,
        tofill1,
        tofill2,
        filled
    }

    private void Start()
    {
        //_renderer = GetComponent<Renderer>();
    }

    public void SetPosition(Vector2Int pos)
    {
        Position = pos;
    }
    public void SetPosition(int posx, int posy)
    {
        Position = new Vector2Int(posx, posy);
    }

    private void OnMouseDown()
    {
        Debug.Log($"{Position.x},{Position.y}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerTileChanged.Invoke(this);
            if (TileState == State.empty)
            {
                //Fill();
                Debug.Log("triggering");
            }

            if (TileState == State.filled)
            {
                OnShootFinished.Invoke();
                Debug.Log("invoked shot finished");
            }
        }

        if (other.CompareTag("Grapple Hook"))
        {
            if (this.TileState == State.filled)
            {
                OnGrappleTileChanged.Invoke(this);
                Debug.Log("locked input, hit is true, set targetpos");
            }
        }
        
    }

    public bool IsTileFilled()
    {
        if (TileState == State.filled)
            return true;
        return false;
    }

    void SetColor(Color c)
    {
        _renderer.material.color = c;
        //Debug.Log("changing color to" + c);
    }

    public void SetTileState(State s)
    {
        TileState = s;
        switch (s)
        {
            case State.empty:
                SetColor(Color.white);
                break;
            case State.trail:
                SetColor(Color.blue);
                break;
            case State.trailStart:
                SetColor(Color.blue);
                break;
            case State.tofill1:
                break;
            case State.tofill2:
                break;
            case State.filled:

                SetColor(Color.red);
                break;
            default:
                break;
        }
    }
}

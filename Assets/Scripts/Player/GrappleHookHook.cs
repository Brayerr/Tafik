using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrappleHookHook : MonoBehaviour
{
    [SerializeField] float _hookSpeed = 26;
    [SerializeField] float _reelSpeed = 26;
    [SerializeField] float range = 15;
    Vector2 _position;
    Vector2 _direction;
    float _maxDistance;
    float _distanceTraveled;
    bool _hooked;

    PlayerLogic _player;
    [SerializeField] LineRenderer _lineRenderer;

    private void Start()
    {
        _distanceTraveled = 0;
        _maxDistance = range * range;
        _position = new Vector2(transform.position.x, transform.position.z);
        _lineRenderer.SetPosition(0, _player.transform.position);
    }

    private void Update()
    {
        if (!_hooked)
        {
            Move();
            CheckHook();
        }
        else
        {
            Reel();
            CheckPlayer();
        }
    }

    public void GetPlayer(PlayerLogic player)
    {
        _player = player;
        _direction = player.Direction;
    }

    //move the hook
    void Move()
    {
        Vector2 dMovement = _direction * _hookSpeed * Time.deltaTime;
        Vector2 dPosition = new Vector2(_position.x + dMovement.x, _position.y + dMovement.y);
        //if the next position is out of bounds, movement must be greater than 1, so move 1 instead
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
        {
            dPosition = new Vector2(_position.x + _direction.x, _position.y + _direction.y);
        }
        _position = dPosition;
        _distanceTraveled += dMovement.x + dMovement.y;
        transform.position = new(_position.x, 1, _position.y);
        _lineRenderer.SetPosition(1, transform.position);
    }

    //check if the hook hit a destination
    void CheckHook()
    {
        if (TileBoardManager.Board.Tiles[(int)_position.x, (int)_position.y].State == 1)
        {
            _hooked = true;
        }
        if (_distanceTraveled * _distanceTraveled > _maxDistance)
        {
            _player.EnableMove();
            Destroy(gameObject);
        }
    }

    //pull the player to the hook
    public void Reel()
    {
        //Vector2 dir = (tPos - _player.position).normalized;
        //calculate movement
        Vector2 dMovement = _direction * _reelSpeed * Time.deltaTime;
        //calculate next position
        Vector2 dPosition = new Vector2(_player.position.x + dMovement.x, _player.position.y + dMovement.y);
        //check if the position is outside the arena
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
        {
            //make next position closer by normalizing it
            dPosition = new Vector2(_position.x + _direction.x, _position.y + _direction.y);
        }

        _player.position = dPosition;
        _lineRenderer.SetPosition(0, _player.transform.position);
    }

    //check if player reached the hook
    void CheckPlayer()
    {
        Vector2 _delta = _player.position - _position;
        if (_delta.sqrMagnitude < (_delta + (_direction * 2)).sqrMagnitude)
        //if ((_player.position - _position).sqrMagnitude < 1)
        {
            _player.EnableMove();
            Destroy(gameObject);
        }
    }

}

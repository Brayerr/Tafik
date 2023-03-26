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
            Reel(_position);
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
    public void Reel(Vector2 tPos)
    {
        Vector2 dir = (tPos - _player.position).normalized;
        Vector2 dMovement = dir * _reelSpeed * Time.deltaTime;
        _player.position = new Vector2(_player.position.x + dMovement.x, _player.position.y + dMovement.y);
        _lineRenderer.SetPosition(0, _player.transform.position);
    }

    //check if player reached the hook
    void CheckPlayer()
    {
        if ((_player.position - _position).sqrMagnitude < 1)
        {
            _player.EnableMove();
            Destroy(gameObject);
        }
    }

}

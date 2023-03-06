using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrappleHookHook : MonoBehaviour
{
    float _hookSpeed = 26;
    float _reelSpeed = 26;
    Vector2 _position;
    Vector2 _direction;
    bool _hooked;

    PlayerLogic _player;

    private void Start()
    {
        _position = new Vector2(transform.position.x, transform.position.z);

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

    void Move()
    {
        Vector2 dMovement = _direction * _hookSpeed * Time.deltaTime;
        Vector2 dPosition = new Vector2(_position.x + dMovement.x, _position.y + dMovement.y);
        _position = dPosition;
        transform.position = new(_position.x, 1, _position.y);
    }

    void CheckPlayer()
    {
        if ((_player.position- _position).magnitude < 0.5)
        {
            _player.EnableMove();
            Destroy(gameObject);
        }
    }

    public void GetPlayer(PlayerLogic player)
    {
        _player = player;
        _direction = player.Direction;
    }

    void CheckHook()
    {
        if (TileBoardManager.Board.Tiles[(int)_position.x, (int)_position.y].State == 1)
        {
            _hooked = true;
            //MoveTowards(_position);
            //_player.DisableMove();
            //Destroy(gameObject);
        }
    }

    public void Reel(Vector2 tPos)
    {
        Vector2 dir = (tPos - _player.position).normalized;
        Vector2 dMovement = dir * _reelSpeed * Time.deltaTime;
        _player.position = new Vector2(_player.position.x + dMovement.x, _player.position.y + dMovement.y);

    }


}

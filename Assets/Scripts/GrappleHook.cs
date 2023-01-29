using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// might become an interface
public class GrappleHook : Ability
{
    public static event Action OnShoot;

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController controller;
    [SerializeField] private GameObject grapple;
    [SerializeField] private Transform grapplePos;
    public bool Hit = false;
    public override string abilityName { get; protected set; } = "Grapple Hook";
    public override int MPUsage { get; protected set; } = 5; // might change
    public virtual Vector3 position { get; protected set; }
    public Vector3 targetPos;
    public virtual Vector2 direction { get; protected set; }
    public virtual Vector2 directionHolder { get; protected set; }
    public virtual float speed { get; protected set; } = 15f;
    public Type type { get; protected set; } = Type.Active;



    private void Start()
    {
        grapple.gameObject.SetActive(false);
        Tile.OnShootFinished += HitToggler;
        Tile.OnShootFinished += GrappleSetActiveFalse;
        Tile.OnGrappleTileChanged += SetTargetPosition;
    }

    private void Update()
    {
        if(!Hit) MoveGrapple();
        if (Hit) MovePlayer(targetPos);
    }



    public void ShootGrapple()
    {
        grapple.gameObject.SetActive(true);
        Debug.Log("shooting grapple");
        OnShoot.Invoke();
        SetDirection(controller.direction);
        directionHolder = direction;
        Hit = false;
        //MoveGrapple();
        if (Hit)
        {
            Debug.Log("grapple hit");
            //MovePlayer(targetPos);
        }
    }

    public void SetDirection(Vector2 dir) => direction = dir;

    public void SetTargetPosition(Tile t)
    {
        targetPos = t.transform.position;
        Hit = true;
        Debug.Log("hit is now true");
    }

    public void MovePlayer(Vector3 targetPos)
    {      
        player.transform.Translate(new Vector3(-targetPos.x, 0,  -targetPos.y) * speed * 4 * Time.deltaTime, Space.World);
        //Hit = false;
    }

    public void MoveGrapple() => grapple.transform.Translate(new Vector3(directionHolder.x, 0, directionHolder.y) * speed * Time.deltaTime, Space.World);

    public void HitToggler() => Hit = false;

    public void GrappleSetActiveFalse()
    {
        transform.position = grapplePos.position;
        if (grapple.gameObject == isActiveAndEnabled) grapple.SetActive(false);
    }
}

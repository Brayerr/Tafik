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
    public virtual Vector2 direction { get; protected set; }
    public virtual Vector2 directionHolder { get; protected set; }
    public virtual float speed { get; protected set; } = 15f;
    public Type type { get; protected set; } = Type.Active;



    private void Start()
    {
        grapple.gameObject.SetActive(false);
        Tile.OnShootFinished += HitSetFalse;
        Tile.OnShootFinished += GrappleSetActiveFalse;
        Tile.OnGrappleTileChanged += HitSetTrue;
        OnShoot += ResetGrapplePos;
        OnShoot += GrappleSetActiveTrue;
    }

    private void Update()
    {
        if(!Hit && grapple.gameObject == isActiveAndEnabled) MoveGrapple();
        if (Hit) MovePlayer();
    }

    public void ShootGrapple()
    {
        Debug.Log("shooting grapple");
        OnShoot.Invoke();
        SetDirection(controller.direction);
        directionHolder = direction;
    }

    public void MovePlayer()
    {
        if (directionHolder == Vector2.down) player.transform.Translate(Vector3.back * (speed * 2) * Time.deltaTime, Space.World);
        if (directionHolder == Vector2.up) player.transform.Translate(Vector3.forward * (speed * 2) * Time.deltaTime, Space.World);
        if (directionHolder == Vector2.left) player.transform.Translate(Vector3.left * (speed * 2) * Time.deltaTime, Space.World);
        if (directionHolder == Vector2.right) player.transform.Translate(Vector3.right * (speed * 2) * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector2 dir) => direction = dir;

    public void MoveGrapple() => grapple.transform.Translate(new Vector3(directionHolder.x, 0, directionHolder.y) * speed * Time.deltaTime, Space.World);

    public void HitSetFalse() => Hit = false;
    public void HitSetTrue(Tile t) => Hit = true;

    public void GrappleSetActiveFalse()
    {
        if (grapple.gameObject == isActiveAndEnabled) grapple.SetActive(false);
    }

    public void GrappleSetActiveTrue()
    {
        if (grapple.gameObject == !isActiveAndEnabled) grapple.SetActive(true);
    }

    public void ResetGrapplePos() => transform.position = grapplePos.position;

    public override void Activate(PlayerLogic player)
    {
        throw new NotImplementedException();
    }
}

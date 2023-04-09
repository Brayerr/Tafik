using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Pickup : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [SerializeField] Transform playerPos;
    bool revealed = false;
    bool pickedUp = false;
    [SerializeField] Vector3 startRotation;
    [SerializeField] Vector3 rotationTarget;
    [SerializeField] Vector3 startScale;

    private void Start()
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
        IdlePickup();
    }


    public void RevealPickup()
    {
        if(!revealed)
        {
            gameObject.SetActive(true);
            StartCoroutine(RevealCoroutine());
        }
    }

    public void IdlePickup()
    {
        Sequence Idle = DOTween.Sequence().Append(transform.DORotate(rotationTarget, 1)).Append(transform.DORotate(startRotation, 1)).SetLoops(-1);
        Idle.Play();
    }

    public void InteractWithPickup()
    {
        if(revealed && !pickedUp)
        {
            StartCoroutine(InteractCoroutine());
        }
    }


    IEnumerator InteractCoroutine()
    {
        Sequence onInteract = DOTween.Sequence().Append(transform.DOMoveY(5, 1f));
        onInteract.Play();
        yield return onInteract.WaitForCompletion();
        Sequence onInteract2 = DOTween.Sequence().Append(transform.DOMove(new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z), .5f)).Append(transform.DOScale(new Vector3(.3f, .3f, .3f), .5f));
        onInteract2.Play();
        yield return onInteract2.WaitForCompletion();
        gameObject.SetActive(false);
        pickedUp = true;
        onInteract.Kill();
        onInteract2.Kill();
    }

    IEnumerator RevealCoroutine()
    {
        Sequence onReveal = DOTween.Sequence().Append(transform.DOScale(new Vector3(transform.lossyScale.x * 1.5f, transform.lossyScale.y * 1.5f, transform.lossyScale.z * 1.5f), .5f));
        onReveal.Play();
        yield return onReveal.WaitForCompletion();
        Sequence onReveal2 = DOTween.Sequence().Append(transform.DOScale(new Vector3(transform.lossyScale.x / 1.5f, transform.lossyScale.y / 1.5f, transform.lossyScale.z / 1.5f), .5f));
        onReveal2.Play();
        revealed = true;
        yield return onReveal2.WaitForCompletion();
        onReveal.Kill();
        onReveal2.Kill();
        
    }


}

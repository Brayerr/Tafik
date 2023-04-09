using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickup : MonoBehaviour
{
    [SerializeField] public Vector2Int position;
    [SerializeField] Transform playerPos;
    bool revealed = false;
    bool pickedUp = false;

    private void Start()
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    public void RevealPickup()
    {
        if(!revealed)
        {
            gameObject.SetActive(true);
            StartCoroutine(RevealCoroutine());
        }
    }

    public void InteractWithPickup()
    {
        if(!pickedUp)
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
    }

    IEnumerator RevealCoroutine()
    {
        Sequence onReveal = DOTween.Sequence().Append(transform.DOScale(new Vector3(transform.lossyScale.x * 1.5f, transform.lossyScale.y * 1.5f, transform.lossyScale.z * 1.5f), .5f));
        onReveal.Play();
        yield return onReveal.WaitForCompletion();
        Sequence onReveal2 = DOTween.Sequence().Append(transform.DOScale(new Vector3(transform.lossyScale.x / 1.5f, transform.lossyScale.y / 1.5f, transform.lossyScale.z / 1.5f), .5f));
        onReveal2.Play();
        revealed = true;
    }
}

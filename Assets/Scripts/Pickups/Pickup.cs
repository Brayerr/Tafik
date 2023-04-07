using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickup : MonoBehaviour
{
    [SerializeField] public Vector2Int position;

    private void Start()
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    public void InteractWithPickup()
    {
        Debug.Log("interacted with pickup");
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    Vector2Int area; //change to start and width
    Vector2Int center;
    static public event Action<Vector2Int, Vector2Int> OnErupt;

    private void Start()
    {
        TelegraphZone();
        center = transform.position.RoundVector();
        StartCoroutine(PreSnap());
    }

    void TelegraphZone()
    {
        transform.localScale = new(transform.localScale.x * area.x, 1, transform.localScale.x * area.y);
    }

    public void GetAreaSize(Vector2Int size)
    {
        area = size;
    }
    void ApplyDanger()
    {
        OnErupt.Invoke(center, area);
    }

    void RemoveDanger()
    {

    }
    IEnumerator PreSnap()
    {
        yield return new WaitForSeconds(3);
        ApplyDanger();
        Destroy(gameObject);
    }
}

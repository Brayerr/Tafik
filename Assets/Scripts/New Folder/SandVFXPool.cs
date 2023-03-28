using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandVFXPool : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    GameObject objectToPool;
    [SerializeField] int amountToPool;
    [SerializeField] PlayerLogic player;

    private void Start()
    {
        objectToPool = Resources.Load<GameObject>("Prefabs/SandVFX");

        GameObject sandVFX;
        for (int i = 0; i < amountToPool; i++)
        {
            sandVFX = Instantiate(objectToPool);
            sandVFX.gameObject.SetActive(false);
            pooledObjects.Add(sandVFX);
        }

        PlayerLogic.OnPlayerTileChanged += InvokeVFX;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    void InvokeVFX(TileLogic t)
    {
        if (player.buildMode)
        {
            GameObject sandVFX = GetPooledObject();
            if (sandVFX != null)
            {
                sandVFX.transform.position = new(t.Position.x + 0.5f, sandVFX.transform.position.y, t.Position.y);
                sandVFX.SetActive(true);
            }
        }
    }
}

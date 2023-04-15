using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandVFXPool : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    GameObject objectToPool;
    GameObject objectToPool1;
    GameObject objectToPool2;
    [SerializeField] int amountToPool;
    [SerializeField] PlayerLogic player;
    
    private void Start()
    {
        
        objectToPool = Resources.Load<GameObject>("Prefabs/SandVFX");
        objectToPool1 = Resources.Load<GameObject>("Prefabs/SandVFX2");
        objectToPool2 = Resources.Load<GameObject>("Prefabs/SandVFX3");

        GameObject sandVFX;
        for (int i = 0; i < amountToPool; i++)
        {
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    {
                        sandVFX = Instantiate(objectToPool);
                        break;
                    }
                case 2:
                    {
                        sandVFX = Instantiate(objectToPool1);
                        break;
                    }
                case 3:
                    {
                        sandVFX = Instantiate(objectToPool2);
                        break;
                    }
                default:
                    {
                        sandVFX = Instantiate(objectToPool2);
                        break;
                    }
            }    
            sandVFX.gameObject.SetActive(false);
            pooledObjects.Add(sandVFX);
        }
        PlayerLogic.OnTrailStart += UseVFX;
        PlayerLogic.OnTrailEnd += StopVFX;
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

    void UseVFX()
    {
        PlayerLogic.OnPlayerTileChanged += InvokeVFX;
    }

    void StopVFX()
    {
        PlayerLogic.OnPlayerTileChanged -= InvokeVFX;
    }

    void InvokeVFX(TileLogic t)
    {
        if (PlayerLogic.buildMode)
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
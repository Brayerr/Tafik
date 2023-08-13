using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeVFXPool : MonoBehaviour
{
    public List<GameObject> pooledObjects = new List<GameObject>();
    GameObject objectToPool;
    [SerializeField] int amountToPool;

    private void Start()
    {
        objectToPool = Resources.Load<GameObject>("Prefabs/SmokeVFX");

        GameObject smokeVFX;
        for (int i = 0; i < amountToPool; i++)
        {
            smokeVFX = Instantiate(objectToPool);
            smokeVFX.gameObject.SetActive(false);
            pooledObjects.Add(smokeVFX);
        }

        EnemyHCrab2.OnDropShell += InvokeVFX;
        EnemyHCrab2.OnPickShell += InvokeVFX;
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

    void InvokeVFX(Vector3 position)
    {
        GameObject smokeVFX = GetPooledObject();
        if (smokeVFX != null)
        {
            smokeVFX.transform.position = new(position.x, transform.position.y, position.z);
            smokeVFX.SetActive(true);
        }      
    }

    private void OnDestroy()
    {
        EnemyHCrab2.OnDropShell -= InvokeVFX;
        EnemyHCrab2.OnPickShell -= InvokeVFX;
    }
}

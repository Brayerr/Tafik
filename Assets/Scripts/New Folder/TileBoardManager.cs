using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBoardManager : MonoBehaviour
{
    [SerializeField] float space; //space between instances
    [SerializeField] Vector2Int dimensions;

    [SerializeField] GameObject EmptyPrefabObject;
    [SerializeField] GameObject FilledPrefabObject;

    TileBoardLogic _tileBoard;
    GameObject[,] _tileGraphics;




    [ContextMenu("Spawn Grid")]
    public void SpawnGrid()
    {
        _tileBoard = new(dimensions);
        _tileBoard.Frame();

        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                if (_tileBoard.Tiles[i, j].State == 0)
                    Instantiate(EmptyPrefabObject, new Vector3((i + 1) * space - (space / 2), 0, (j + 1) * space - (space / 2)), Quaternion.identity, this.transform);
                else
                {
                    Instantiate(FilledPrefabObject, new Vector3((i + 1) * space - (space / 2), 0, (j + 1) * space - (space / 2)), Quaternion.identity, this.transform);

                }
            }
        }
    }

    

}

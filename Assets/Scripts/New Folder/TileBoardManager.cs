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

    static public TileBoardLogic Board { get; private set; }
    GameObject[,] _tileGraphics;




    [ContextMenu("Spawn Grid")]
    public void SpawnGrid()
    {
        Board = new(dimensions);
        Board.Frame();

        for (int i = 0; i < dimensions.y; i++)
        {
            for (int j = 0; j < dimensions.x; j++)
            {
                if (Board.Tiles[j, i].State == 0)
                    Instantiate(EmptyPrefabObject, new Vector3((j + 1) * space - (space / 2), 0, (i + 1) * space - (space / 2)), Quaternion.identity, this.transform);
                else
                {
                    Instantiate(FilledPrefabObject, new Vector3((j + 1) * space - (space / 2), 0, (i + 1) * space - (space / 2)), Quaternion.identity, this.transform);
                }
            }
        }
    }
    
    // area closer
    // an algorithm that records a trail of all the points on the board where the player turned
    // start from the top left turning point, looks for the next point on the same x.
    // use that as first rect width. go down in y within this x range and look for turning points
    // use that y for height. build a rect with those dimensions and mark every tile within the rect to be filled
    // if the corners of that rect arent in the list, add them to the list.
    // keep going looking for corners after the 2nd turning point
    
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class LevelGen : MonoBehaviour
{
    [SerializeField]
    UnityEngine.GameObject PrefabObj;
    [SerializeField]
    TileBoard tileBoard;
}
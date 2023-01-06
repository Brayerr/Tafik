using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class LevelGen : MonoBehaviour
{
    [SerializeField]
    GameObject PrefabObj;
    [SerializeField]
    TileBoard tileBoard;
}
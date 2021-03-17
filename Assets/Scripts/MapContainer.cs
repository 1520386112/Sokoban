using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放地图快照
/// </summary>
public class MapContainer : MonoBehaviour
{
    public static int[,] snapshoot = {
    { 1, 1, 1, 1, 1, 1, 0, 0, 0},
    { 1, 0, 0, 0, 5, 1, 1, 0, 0},
    { 1, 2, 0, 0, 0, 0, 1, 0, 0},
    { 1, 0, 1, 0, 1, 1, 1, 1, 1},
    { 1, 0, 1, 0, 1, 0, 0, 5, 1},
    { 1, 0, 1, 0, 0, 3, 0, 1, 1},
    { 1, 0, 0, 0, 1, 3, 0, 1, 0},
    { 1, 0, 0, 0, 0, 0, 0, 1, 0},
    { 1, 1, 1, 1, 1, 1, 1, 1, 0}
    };
}

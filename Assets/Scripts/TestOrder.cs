/// Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestOrder : MonoBehaviour
{
    [Space(10, order = -2)]
    [Header("Header with some space around it", order = -1)]
    [Space(40, order = -3)]

    public string playerName = "Unnamed";
}
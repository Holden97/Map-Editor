//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public const int mapLength = 50;
    public const int mapWidth = 50;
    public int randomFillExpectation = 50;
    public int thresholdToWall = 4;
    public int involveCount = 3;
    public int[,] map = new int[mapLength, mapWidth];
    public Transform mapParent;

    public GameObject wall;
    public GameObject ground;

    private CellularAutomaton cellular = new CellularAutomaton();
    void Start()
    {
        //生成初始随机地图
        map = cellular.CreateMap(mapWidth, mapLength, randomFillExpectation, thresholdToWall, involveCount);
    }

    private void RenderMap(int[,] map)
    {
        Debug.Log($"mapParent.childCount:{mapParent.childCount}");
        //后期用对象池修改
        for (int i = mapParent.childCount - 1; i >= 0; i--)
        {
            Destroy(mapParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                var go = map[i, j] == 0 ? Instantiate(ground) : Instantiate(wall);
                go.transform.position = new Vector3(i, 0, j);
                if (mapParent != null)
                {
                    go.transform.SetParent(mapParent);
                }
            }
        }
    }

    public void OnClickCreate()
    {
        //渲染
        RenderMap(map);
    }

    public void OnClickEvolve()
    {
        //演变
        this.map = cellular.EvolveMap();
        RenderMap(map);
    }

    public void OnClickEvolveOnce()
    {
        //演变
        this.map = cellular.EvolveMapOnce();
        RenderMap(map);
    }
}

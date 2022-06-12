//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomaton
{
    private int[,] map;
    private int thresholdToWall;
    private int involveCount;

    public int[,] CreateMap(int mapWidth, int mapLength, int randomFillExpectation, int thresholdToWall, int involveCount)
    {
        var seed = Random.Range(int.MinValue, int.MaxValue);
        System.Random random = new System.Random(seed.GetHashCode());

        Debug.Log($"seed:{seed}");
        Debug.Log($"seed.GetHashCode():{seed.GetHashCode()}");

        int[,] map = new int[mapWidth, mapLength];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapLength; j++)
            {
                //四面造墙
                if (i == 0 || j == mapLength - 1 || i == mapWidth - 1 || j == 0)
                {
                    map[i, j] = 1;
                }
                else
                {
                    map[i, j] = random.Next(0, 100) < randomFillExpectation ? 1 : 0;
                }
            }
        }

        this.thresholdToWall = thresholdToWall;
        this.involveCount = involveCount;

        this.map = map;
        return map;
    }

    public int GetSurroundWallCount(int x, int z)
    {
        if (map == null)
        {
            Debug.LogError("map is null");
            return 0;
        }
        int wallCount = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = z - 1; j < z + 2; j++)
            {
                //剔除自身
                if (i == x && j == z)
                {
                    continue;
                }
                //地图外的点均默认为墙
                if (i < 0 || i > map.GetLength(0) - 1 || j < 0 || j > map.GetLength(1) - 1)
                {
                    wallCount++;
                }
                else
                {
                    if (map[i, j] == 1)
                    {
                        wallCount++;
                    }
                }
            }
        }

        Debug.Log($"pos:{x},{z}--wallCount:{wallCount}");
        return wallCount;
    }

    public int EvolveIn(int x, int z, int threshold)
    {
        return GetSurroundWallCount(x, z) >= threshold ? 1 : 0;
    }

    public int[,] EvolveMapOnce(int[,] originalMap)
    {
        int[,] nextMap = new int[originalMap.GetLength(0), originalMap.GetLength(1)];
        for (int i = 0; i < originalMap.GetLength(0); i++)
        {
            for (int j = 0; j < originalMap.GetLength(1); j++)
            {
                nextMap[i, j] = EvolveIn(i, j, thresholdToWall);
            }
        }
        this.map = nextMap;
        return nextMap;
    }

    public int[,] EvolveMapOnce()
    {
        int[,] nextMap = new int[map.GetLength(0), map.GetLength(1)];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                nextMap[i, j] = EvolveIn(i, j, thresholdToWall);
            }
        }
        this.map = nextMap;
        return nextMap;
    }

    public int[,] EvolveMap()
    {
        if (map == null)
        {
            Debug.LogError("map is null!");
            return null;
        }
        int[,] nextMap = EvolveMapOnce(map);
        for (int i = 0; i < involveCount - 1; i++)
        {
            nextMap = EvolveMapOnce(nextMap);
        }
        this.map = nextMap;
        return nextMap;
    }

}

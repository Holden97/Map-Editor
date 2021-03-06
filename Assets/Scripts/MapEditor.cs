//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public const int mapLength = 10;
    public const int mapWidth = 10;
    public int randomFillExpectation = 50;
    public int thresholdToWall = 4;
    public int involveCount = 3;
    public int[,] map = new int[mapLength, mapWidth];
    public Transform mapParent;

    /// <summary>
    /// 墙壁mesh
    /// </summary>
    public MeshFilter walls;
    /// <summary>
    /// 墙壁顶mesh
    /// </summary>
    public MeshFilter topMesh;

    public GameObject wall;
    public GameObject ground;

    private CellularAutomaton cellular = new CellularAutomaton();
    void Awake()
    {
        //生成初始随机地图
        map = cellular.CreateMap(mapWidth, mapLength, randomFillExpectation, thresholdToWall, involveCount);
    }

    private void RenderMapByGameObject(int[,] map)
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

    private void RenderMapByGizmos(int[,] map)
    {
        //Debug.Log($"mapParent.childCount:{mapParent.childCount}");
        //后期用对象池修改
        for (int i = mapParent.childCount - 1; i >= 0; i--)
        {
            Destroy(mapParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Gizmos.color = map[i, j] == 0 ? Color.white : Color.black;
                Gizmos.DrawCube(new Vector3(i, j), Vector3.one);
            }
        }
    }

    public void OnClickCreate()
    {

    }

    public void OnClickEvolve()
    {

    }

    public void OnClickEvolveOnce()
    {

    }

    private void OnDrawGizmos()
    {
        var squareGrid = new MapGrids(map, 1);

        if (squareGrid != null)
        {
            for (int i = 0; i < squareGrid.squares.GetLength(0); i++)
            {
                for (int j = 0; j < squareGrid.squares.GetLength(1); j++)
                {
                    Gizmos.color = (squareGrid.squares[i, j].topLeft.IsWall) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].topLeft.pos, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squares[i, j].topRight.IsWall) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].topRight.pos, Vector3.one * 0.4f);
                    Gizmos.color = (squareGrid.squares[i, j].bottomLeft.IsWall) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].bottomLeft.pos, Vector3.one * 0.4f);
                    Gizmos.color = (squareGrid.squares[i, j].bottomRight.IsWall) ? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squares[i, j].bottomRight.pos, Vector3.one * 0.4f);

                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.squares[i, j].centerTop.pos, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centerBottom.pos, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centerLeft.pos, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squares[i, j].centerRight.pos, Vector3.one * 0.15f);
                }
            }
        }

        GenerateMesh(map, 1);
    }

    public void GenerateMesh(int[,] map, float squareSize)
    {

        var squareGrid = new MapGrids(map, squareSize);

        squareGrid.Clear();

        for (int i = 0; i < squareGrid.squares.GetLength(0); i++)
        {
            for (int j = 0; j < squareGrid.squares.GetLength(1); j++)
            {
                squareGrid.TriangulateSquare(squareGrid.squares[i, j]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = squareGrid.vertexes.ToArray();
        mesh.triangles = squareGrid.triangles.ToArray();
        mesh.RecalculateNormals();

        squareGrid.CreateWallMesh(walls);

        List<Vector3> topVertices = new List<Vector3>();

        for (int i = 0; i < squareGrid.vertexes.Count; i++)
        {
            topVertices.Add(squareGrid.vertexes[i] + Vector3.up * MapGrids.wallHeight);
        }

        Mesh mesh2 = new Mesh();
        topMesh.mesh = mesh2;
        mesh2.vertices = topVertices.ToArray();
        mesh2.triangles = squareGrid.triangles.ToArray();
        mesh2.RecalculateNormals();

    }
}

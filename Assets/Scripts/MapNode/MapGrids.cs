//Author：GuoYiBo
using System.Collections.Generic;
using UnityEngine;

public class MapGrids

{
    public Square[,] squares;

    List<Vector3> vertexes = new List<Vector3>();
    List<int> triangles = new List<int>();
    Dictionary<int, List<Triangle>> trianglesDictionary = new Dictionary<int, List<Triangle>>();

    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();
    public MapGrids(int[,] map, float squareSize)
    {
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        Vertice[,] controlNodes = new Vertice[nodeCountX, nodeCountY];
        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountY; j++)
            {
                //这里+squareSize/2是为了保证整张图的中点始终在原点
                Vector3 pos = new Vector3(-mapWidth / 2 + i * squareSize + squareSize / 2, 0, -mapHeight / 2 + j * squareSize + squareSize / 2);
                GridType type;
                switch (map[i, j])
                {
                    case 0:
                        type = GridType.GROUND;
                        break;
                    case 1:
                        type = GridType.WALL;
                        break;
                    default:
                        type = GridType.UNKNOWN;
                        break;
                }
                controlNodes[i, j] = new Vertice(pos, type, squareSize);
            }
        }
        squares = new Square[nodeCountX - 1, nodeCountY - 1];
        for (int i = 0; i < nodeCountX - 1; i++)
        {
            for (int j = 0; j < nodeCountY - 1; j++)
            {
                squares[i, j] = new Square(controlNodes[i, j + 1], controlNodes[i + 1, j + 1], controlNodes[i, j], controlNodes[i + 1, j]);
            }
        }
    }

    /// <summary>
    /// 根据square索引值创建三角形
    /// </summary>
    /// <param name="square"></param>
    public void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            //0 point
            case 0:
                break;
            //1 point
            case 1:
                Mesh4Points(square.centerLeft, square.centerBottom, square.bottomLeft);
                break;
            case 2:
                Mesh4Points(square.bottomRight, square.centerBottom, square.centerRight);
                break;
            case 4:
                Mesh4Points(square.topRight, square.centerRight, square.centerTop);
                break;
            case 8:
                Mesh4Points(square.topLeft, square.centerTop, square.centerLeft);
                break;
            //2 points
            case 3:
                Mesh4Points(square.centerRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                break;
            case 6:
                Mesh4Points(square.centerTop, square.topRight, square.bottomRight, square.centerBottom);
                break;
            case 9:
                Mesh4Points(square.topLeft, square.centerTop, square.centerBottom, square.bottomLeft);
                break;
            case 12:
                Mesh4Points(square.topLeft, square.topRight, square.centerRight, square.centerLeft);
                break;
            case 5:
                Mesh4Points(square.centerTop, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft, square.centerLeft);
                break;
            case 10:
                Mesh4Points(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
                break;
            //3 points
            case 7:
                Mesh4Points(square.centerTop, square.topRight, square.bottomRight, square.bottomLeft, square.centerLeft);
                break;
            case 11:
                Mesh4Points(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                Mesh4Points(square.topLeft, square.topRight, square.centerRight, square.centerBottom, square.bottomLeft);
                break;
            case 14:
                Mesh4Points(square.topLeft, square.topRight, square.bottomRight, square.centerBottom, square.centerLeft);
                break;
            //4 points
            case 15:
                Mesh4Points(square.topLeft, square.centerTop, square.centerRight, square.bottomRight, square.centerBottom, square.centerLeft);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 根据顶点数量创建多个三角形，这里传入的参数要求为顺时针
    /// </summary>
    /// <param name="points"></param>
    public void Mesh4Points(params Node[] points)
    {
        InitVertices(points);

        if (points.Length >= 3)
        {
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4)
        {
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5)
        {
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6)
        {
            CreateTriangle(points[0], points[4], points[5]);
        }
    }

    public void InitVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)//default
            {
                //编号
                points[i].vertexIndex = vertexes.Count;
                //加入List
                vertexes.Add(points[i].pos);
            }
        }
    }

    public void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);

        AddTriangle2Dictionary(a.vertexIndex, triangle);
        AddTriangle2Dictionary(b.vertexIndex, triangle);
        AddTriangle2Dictionary(c.vertexIndex, triangle);
    }

    public void AddTriangle2Dictionary(int vertexIndex, Triangle triangle)
    {
        if (trianglesDictionary.ContainsKey(vertexIndex))
        {
            if (!trianglesDictionary[vertexIndex].Contains(triangle))
            {
                trianglesDictionary[vertexIndex].Add(triangle);
            }
        }
        else
        {
            trianglesDictionary.Add(vertexIndex, new List<Triangle> { triangle });
        }
    }

    /// <summary>
    /// 是否是轮廓边
    /// </summary>
    /// <param name="vertexA"></param>
    /// <param name="vertexB"></param>
    /// <returns></returns>
    public bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> triA = trianglesDictionary[vertexA];
        int shareTriangleCount = 0;
        for (int i = 0; i < triA.Count; i++)
        {
            var curTri = triA[i];
            if (curTri.Contains(vertexB))
            {
                shareTriangleCount++;
            }
            if (shareTriangleCount > 1)
            {
                return false;
            }
        }
        return shareTriangleCount == 1;
    }

    /// <summary>
    /// 寻找下一个可以成为轮廓的顶点
    /// </summary>
    /// <param name="vertexIndex">当前顶点index</param>
    /// <returns>下一个顶点index</returns>
    public int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> curTriangles = trianglesDictionary[vertexIndex];
        for (int i = 0; i < curTriangles.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var nextVertice = curTriangles[i].vertices[(j + 1) % 3];
                if (IsOutlineEdge(vertexIndex, nextVertice) && !checkedVertices.Contains(nextVertice))
                {
                    checkedVertices.Add(nextVertice);
                    return nextVertice;
                }
            }
        }
        return -1;
    }

    public void CalculateMeshOutlines()
    {
        for (int curIndex = 0; curIndex < vertexes.Count; curIndex++)
        {
            if (!checkedVertices.Contains(curIndex))
            {
                int nextVertex = GetConnectedOutlineVertex(curIndex);
                if (nextVertex != -1)
                {
                    List<int> outline = new List<int>();
                    outline.Add(curIndex);
                    outline.Add(nextVertex);
                    outlines.Add(outline);
                }
            }
        }
    }
}

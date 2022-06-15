//Author：GuoYiBo
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid
{
    public Square[,] squares;

    List<Vector3> vertexes = new List<Vector3>();
    List<int> triangles = new List<int>();
    public SquareGrid(int[,] map, float squareSize)
    {
        int nodeCountX = map.GetLength(0);
        int nodeCountY = map.GetLength(1);
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountY; j++)
            {
                //这里+squareSize/2是为了保证整张图的中点始终在原点
                Vector3 pos = new Vector3(-mapWidth / 2 + i * squareSize + squareSize / 2, 0, -mapHeight / 2 + j * squareSize + squareSize / 2);
                controlNodes[i, j] = new ControlNode(pos, map[i, j] == 1, squareSize);
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
        AssignVertexIndex(points);

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

    public void AssignVertexIndex(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)//default
            {
                points[i].vertexIndex = vertexes.Count;
                vertexes.Add(points[i].pos);
            }
        }
    }

    public void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
    }
}

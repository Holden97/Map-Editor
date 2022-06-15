//Author：GuoYiBo
using UnityEngine;

public class SquareGrid
{
    public Square[,] squares;
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
        for (int i = 0; i < nodeCountX-1; i++)
        {
            for (int j = 0; j < nodeCountY-1; j++)
            {
                squares[i, j] = new Square(controlNodes[i, j + 1], controlNodes[i + 1, j + 1], controlNodes[i, j], controlNodes[i + 1, j]);
            }
        }
    }
}

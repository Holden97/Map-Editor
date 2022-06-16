//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    public Vertice topLeft, topRight, bottomLeft, bottomRight;
    public Node centerLeft, centerRight, centerTop, centerBottom;
    public int configuration;

    public Square(Vertice topLeft, Vertice topRight, Vertice bottomLeft, Vertice bottomRight)
    {
        configuration = 0;

        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomLeft = bottomLeft;
        this.bottomRight = bottomRight;
        this.centerLeft = bottomLeft.above;
        this.centerRight = bottomRight.above;
        this.centerTop = topLeft.right;
        this.centerBottom = bottomLeft.right;
        if (topLeft.IsWall)
        {
            configuration += 8;
        }
        if (topRight.IsWall)
        {
            configuration += 4;
        }
        if (bottomRight.IsWall)
        {
            configuration += 2;
        }
        if (bottomLeft.IsWall)
        {
            configuration += 1;
        }
    }
}

//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    public ControlNode topLeft, topRight, bottomLeft, bottomRight;
    public Node centerLeft, centerRight, centerTop, centerBottom;
    public int configuration;

    public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
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
        if (topLeft.active)
        {
            configuration += 8;
        }
        if (topRight.active)
        {
            configuration += 4;
        }
        if (bottomRight.active)
        {
            configuration += 2;
        }
        if (bottomLeft.active)
        {
            configuration += 1;
        }
    }
}

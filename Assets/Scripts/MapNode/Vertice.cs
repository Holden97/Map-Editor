//Author：GuoYiBo
using UnityEngine;

/// <summary>
/// 管理四个顶角
/// </summary>
public class Vertice : Node
{
    public bool IsWall
    {
        get
        {
            return type == GridType.WALL;
        }
    }
    public Node above, right;
    private GridType type;
    
    public Vertice(Vector3 pos,GridType type, float squareSize) : base(pos)
    {
        this.type = type;
        above = new Node(pos + Vector3.forward * squareSize / 2);
        right = new Node(pos + Vector3.right * squareSize / 2);
    }
}

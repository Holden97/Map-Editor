//Author：GuoYiBo
using UnityEngine;

/// <summary>
/// 管理四个顶角
/// </summary>
public class ControlNode : Node
{
    public bool active;
    public Node above, right;
    public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos)
    {
        this.active = active;
        above = new Node(pos + Vector3.forward * squareSize / 2);
        right = new Node(pos + Vector3.right * squareSize / 2);
    }
}

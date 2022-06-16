using UnityEngine;

public class Node
{
    public Vector3 pos;
    public int vertexIndex = -1;

    public Node(Vector3 pos)
    {
        this.pos = pos;
    }
}

public enum GridType
{
    /// <summary>
    /// �յ�
    /// </summary>
    GROUND = 0,
    /// <summary>
    /// ǽ
    /// </summary>
    WALL = 1,
    /// <summary>
    /// δ֪
    /// </summary>
    UNKNOWN=-1,
}
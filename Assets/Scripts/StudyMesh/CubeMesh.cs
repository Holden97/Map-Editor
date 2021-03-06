//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeMesh : MonoBehaviour
{
    /// <summary>
    /// 长度顶点数量 >=2
    /// </summary>
    public int length = 3;
    /// <summary>
    /// 宽度顶点数量 >=2
    /// </summary>
    public int width = 3;
    /// <summary>
    /// 高度顶点数量 >=2
    /// </summary>
    public int height = 3;

    public int smoothDegree = 2;

    private int totalVerticesCount;
    private Vector3[] vertices;
    private Vector3[] normals;

    private int curVertexIndex = 0;

    private int[] triangleVerticeIndex;

    private Mesh curMesh;

    private void Awake()
    {
        curMesh = new Mesh();

        int cornerVerticesCount = 8;
        int edgeVerticesCount = ((length - 1) + (width - 1) + (height - 1)) * 4;
        int planesCount = ((height - 1) * (width - 1) + (height - 1) * (length - 1) + (length - 1) * (width - 1)) * 2;

        totalVerticesCount = cornerVerticesCount + edgeVerticesCount + planesCount;
        vertices = new Vector3[totalVerticesCount];
        normals = new Vector3[totalVerticesCount];

        CreateCube();

        GetComponent<MeshFilter>().mesh = curMesh;
    }

    private void SetVertex(ref int curVertexIndex, float x, float y, float z)
    {
        var inner = vertices[curVertexIndex] = new Vector3(x, y, z);
        if (x < smoothDegree)
        {
            inner.x = smoothDegree;
        }
        else if (x + smoothDegree > length)
        {
            inner.x = length - smoothDegree;
        }

        if (y < smoothDegree)
        {
            inner.y = smoothDegree;
        }
        else if (y + smoothDegree > height)
        {
            inner.y = height - smoothDegree;
        }

        if (z < smoothDegree)
        {
            inner.z = smoothDegree;
        }
        else if (z + smoothDegree > width)
        {
            inner.z = width - smoothDegree;
        }

        normals[curVertexIndex] = (vertices[curVertexIndex] - inner).normalized;

        vertices[curVertexIndex] = normals[curVertexIndex] * smoothDegree + inner;

        curVertexIndex++;
    }

    private void CreateCube()
    {
        //计算侧面顶点
        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j <= length; j++)
            {
                SetVertex(ref curVertexIndex, j, i, 0);
            }
            for (int k = 1; k <= width; k++)
            {
                SetVertex(ref curVertexIndex, length, i, k);
            }
            for (int j = 1; j <= length; j++)
            {
                SetVertex(ref curVertexIndex, length - j, i, width);
            }
            for (int k = 1; k < width; k++)
            {
                SetVertex(ref curVertexIndex, 0, i, width - k);
            }
        }

        //计算上面顶点
        VerticeOnHorizontalPlane(height, curVertexIndex);

        //计算下面顶点
        VerticeOnHorizontalPlane(0, curVertexIndex);

        //计算三角形顶点数组元素个数
        var squaresCount = (length * width + width * height + height * length) * 2;
        var totalTriangleVerticesCount = 6 * squaresCount;
        triangleVerticeIndex = new int[totalTriangleVerticesCount];

        //计算侧面三角形
        var perimeter = (width + length) * 2;
        var curTriangleIndexIndex = 0;
        var curV = 0;
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < perimeter; i++, curV++)
            {
                if (i < perimeter - 1)
                {
                    SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curV, curV + 1, curV + perimeter, curV + perimeter + 1);
                }
                else
                {
                    SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curV, curV - perimeter + 1, curV + perimeter, curV + 1);
                }
            }

        }

        //计算上面三角形

        //第一行
        curV = height * perimeter;
        var curVTop = curV + perimeter - 1;

        SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curV, curV + 1, curVTop, curVTop + 1);
        curV++;
        curVTop++;

        for (int i = 1; i < length - 1; i++, curV++, curVTop++)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curV, curV + 1, curVTop, curVTop + 1);
        }
        if (length >= 2)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curV, curV + 1, curVTop, curV + 2);
        }
        //中间
        curV += 2;
        curVTop += 1;
        for (int j = 1; j < width - 1; j++, curV++)
        {
            //左侧
            var leftBottom = height * perimeter + perimeter - 1;
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, leftBottom - j + 1, curVTop + 1 - length, leftBottom - j, curVTop);
            //中间
            for (int i = 1; i < length - 1; i++, curVTop++)
            {
                SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curVTop - length + 1, curVTop - length + 2, curVTop, curVTop + 1);
            }
            curVTop++;
            //右侧
            if (length >= 2)
            {
                SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, curVTop - length, curV, curVTop - 1, curV + 1);
            }
        }
        //最后一行
        var lastOutlineAnchor = height * perimeter + perimeter - width + 1;
        var lastInlineAnchor = curVTop + 1 - length;
        SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, lastOutlineAnchor, lastInlineAnchor, lastOutlineAnchor - 1, lastOutlineAnchor - 2);
        for (int i = 1; i < length - 1; i++, curV++, lastInlineAnchor++, lastOutlineAnchor--)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, lastInlineAnchor, lastInlineAnchor + 1, lastOutlineAnchor - 2, lastOutlineAnchor - 3);
        }
        if (length >= 2)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, lastInlineAnchor, lastOutlineAnchor - 4, lastOutlineAnchor - 2, lastOutlineAnchor - 3);
        }

        //计算下面三角形
        var bottomOutlineStart = 0;
        var bottomOutlineEnd = perimeter - 1;
        var bottomInlineStart = lastInlineAnchor + 1;

        //第一行
        SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomOutlineStart + 1, bottomOutlineStart, bottomInlineStart, bottomOutlineEnd);
        bottomOutlineStart++;
        bottomInlineStart++;

        for (int i = 1; i < length - 1; i++, bottomOutlineStart++, bottomInlineStart++)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomOutlineStart + 1, bottomOutlineStart, bottomInlineStart, bottomInlineStart - 1);
        }
        if (length >= 2)
        {
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomOutlineStart + 1, bottomOutlineStart, bottomOutlineStart + 2, bottomInlineStart - 1);
        }

        //中间
        for (int j = 1; j < width - 1; j++, bottomOutlineEnd--, bottomOutlineStart++)
        {
            //左侧
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomInlineStart - length + 1, bottomOutlineEnd, bottomInlineStart, bottomOutlineEnd - 1);
            //中间
            bottomInlineStart++;
            for (int i = 1; i < length - 1; i++, bottomInlineStart++)
            {
                SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomInlineStart - length + 1, bottomInlineStart - length, bottomInlineStart, bottomInlineStart - 1);
            }
            //右侧
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomOutlineStart + 2, bottomInlineStart - length, bottomOutlineStart + 3, bottomInlineStart - 1);
        }
        //最后一行
        if (width >= 2)
        {
            var bottomLastStart = bottomInlineStart - length + 1;
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomLastStart, bottomOutlineEnd, bottomOutlineEnd - 2, bottomOutlineEnd - 1);
            bottomLastStart++;
            for (int i = 1; i < length - 1; i++, bottomLastStart++, bottomOutlineEnd--)
            {
                SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomLastStart, bottomLastStart - 1, bottomOutlineEnd - 3, bottomOutlineEnd - 2);
            }
            SquareCalculate(triangleVerticeIndex, ref curTriangleIndexIndex, bottomOutlineEnd - 4, bottomLastStart - 1, bottomOutlineEnd - 3, bottomOutlineEnd - 2);
        }

        curMesh.vertices = vertices;
        curMesh.triangles = triangleVerticeIndex;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < totalVerticesCount; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

    /// <summary>
    /// 计算顶层/底层 面上的顶点（不包括边框）
    /// </summary>
    /// <param name="height">水平面高度</param>
    /// <param name="startIndex">开始Index</param>
    private void VerticeOnHorizontalPlane(int height, int startIndex)
    {
        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < length; j++)
            {
                SetVertex(ref startIndex, j, height, i);
            }
        }
        this.curVertexIndex = startIndex;
    }

    private void SquareCalculate(int[] triangle, ref int startIndex, int leftBottomVertex, int rightBottomVertex, int leftTopVertex, int rightTopVertex)
    {
        triangle[startIndex] = leftBottomVertex;
        triangle[startIndex + 1] = triangle[startIndex + 4] = leftTopVertex;
        triangle[startIndex + 2] = triangle[startIndex + 3] = rightBottomVertex;
        triangle[startIndex + 5] = rightTopVertex;
        startIndex += 6;
    }
}

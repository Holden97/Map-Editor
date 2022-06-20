//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeMesh : MonoBehaviour
{
    /// <summary>
    /// 长度顶点数量
    /// </summary>
    private int length = 4;
    /// <summary>
    /// 宽度顶点数量
    /// </summary>
    private int width = 4;
    /// <summary>
    /// 高度顶点数量
    /// </summary>
    private int height = 6;

    private int totalVerticesCount;
    private Vector3[] vertices;
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

        CreateCube();

        GetComponent<MeshFilter>().mesh = curMesh;
    }

    private void CreateCube()
    {
        //计算侧面顶点
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < length; j++)
            {
                vertices[curVertexIndex++] = new Vector3(j, i, 0);
            }
            for (int k = 0; k < width; k++)
            {
                vertices[curVertexIndex++] = new Vector3(length - 1, i, k);
            }
            for (int j = 0; j < length; j++)
            {
                vertices[curVertexIndex++] = new Vector3(length - 1 - j, i, width - 1);
            }
            for (int k = 0; k < width; k++)
            {
                vertices[curVertexIndex++] = new Vector3(0, i, width - 1 - k);
            }
        }

        //计算上面顶点
        VerticeOnHorizontalPlane(height - 1, curVertexIndex);

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


        curMesh.vertices = vertices;
        curMesh.triangles = triangleVerticeIndex;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        Gizmos.color = Color.black;
        for (int i = 0; i < totalVerticesCount; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.2f);
        }
    }

    /// <summary>
    /// 计算顶层/底层 面上的顶点（不包括边框）
    /// </summary>
    /// <param name="height">水平面高度</param>
    /// <param name="startIndex">开始Index</param>
    private void VerticeOnHorizontalPlane(int height, int startIndex)
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                vertices[startIndex++] = new Vector3(j, height, i);
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

//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    GameObject image;
    Vector3 vec = new Vector3(50, 490, 0);
    int index = 0;
    Transform parent;

    private void Start()
    {
        image = Resources.Load<GameObject>("Object/image");
        parent = GameObject.Find("Canvas").transform;
    }

    public void CreateObject()
    {
        GameObject img = Instantiate(image, vec + new Vector3(index * 150, 0, 0), Quaternion.identity);
        img.transform.parent = parent;
        index++;
    }
}

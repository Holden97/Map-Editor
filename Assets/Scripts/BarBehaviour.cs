/// Author：GuoYiBo
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Foo]

public class BarBehaviour : MonoBehaviour
{
    private void Start()
    {
        InjectFoo();
    }

    private void InjectFoo()
    {
        if (Attribute.IsDefined(GetType(), typeof(FooAttribute)))
        {
            gameObject.AddComponent<FooBehaviour>();
        }
    }
}

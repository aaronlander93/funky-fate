﻿// Author:      Joseph
// Purpose:     Move background at a constant speed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScrollBackground : MonoBehaviour
{
    public float speed = 0.5f;

    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);

        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}

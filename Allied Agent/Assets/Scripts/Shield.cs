using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public int hp;
    private float red;
    private void Update()
    {
        if (hp >= 0)
        {
            red = 1f - (hp / 1000f);
        }
        meshRenderer.materials[0].color = new Vector4(red, 0, 0, 1);
        if (hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}

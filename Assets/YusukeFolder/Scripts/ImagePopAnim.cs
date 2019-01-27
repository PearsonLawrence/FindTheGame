using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePopAnim : MonoBehaviour
{

    private float scale = 0.1f;
    private float update = 0.1f;
    private bool isScaleUpdate = false;

    // Update is called once per frame
    void Update()
    {
        if (isScaleUpdate && scale < 1)
        {
            scale += update;
            this.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void ScaleUpdate()
    {
        isScaleUpdate = true;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageState : MonoBehaviour
{
    Image ThisImage;
    public List<Texture2D> imageList;
    [Range(0, 255)] public float albedo_LightUp = 0;
    [Range(0, 255)] public float albedo_LightDown = 0;

    void Start()
    {
        ThisImage = this.GetComponent<Image>();
        LightDown();

        updateTime = DateTime.Now;
        testStart = true;
    }

    //Test
    private DateTime updateTime;
    bool test = false;
    bool testStart = false;
    void Update()
    {
        if(!testStart)
            return;

        DateTime nowTime = DateTime.Now;
        double diff = (nowTime - updateTime).TotalSeconds;
        if (diff > 3)
        {
            Debug.Log("★★★" + diff);
            updateTime = nowTime;
            if (test)
            {
                LightUp();
                test = false;
            }
            else
            {
                LightDown();
                test = true;
            }
        }

    }

    public virtual void LightUp()
    {
        ThisImage.color = new Color(albedo_LightUp / 255f, albedo_LightUp / 255f, albedo_LightUp / 255f);
    }

    public virtual void LightDown()
    {
        ThisImage.color = new Color(albedo_LightDown / 255f, albedo_LightDown / 255f, albedo_LightDown / 255f);
    }

    public void StateUpdate(int imageNo)
    {
        if (imageNo > imageList.Count)
            return;
        ThisImage.material.mainTexture = imageList[imageNo];
    }

}

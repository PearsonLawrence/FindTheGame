using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageState : MonoBehaviour
{
    Image ThisImage;
    public List<Sprite> imageList;
    [Range(0, 255)] public float albedo_LightUp = 0;
    [Range(0, 255)] public float albedo_LightDown = 0;

    void Start()
    {
        ThisImage = this.GetComponent<Image>();
    }
    
    public void LightUp()
    {
        ThisImage.color = new Color(albedo_LightUp / 255f, albedo_LightUp / 255f, albedo_LightUp / 255f);
    }

    public void LightDown()
    {
        ThisImage.color = new Color(albedo_LightDown / 255f, albedo_LightDown / 255f, albedo_LightDown / 255f);
    }

    public virtual void StateUpdate(int imageNo)
    {
        if (imageNo >= imageList.Count)
            return;

        var image = this.GetComponent<Image>();
        image.sprite = imageList[imageNo];
    }

}

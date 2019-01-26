using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEvent : MonoBehaviour
{
    public GameObject EventImageGObj;
    private int keyCount = 0;
    private Image image;
    private float alpha = 255;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Event(keyCount);
            keyCount++;
        }

        if (image != null && alpha > 1)
        {
            alpha -= 2;
            image.color = new Color(1, 1, 1, alpha / 255f);
        }
    }

    private void Event(int eventNo)
    {
        switch (eventNo)
        {
            case 0:
                image = EventImageGObj.GetComponent<Image>();
                break;
            case 1:
                var ST = this.GetComponent<SceneTransition>();
                ST.isUnlock = true;
                break;
            default:
                break;
        }
    }
}

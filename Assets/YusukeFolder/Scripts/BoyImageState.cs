using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoyImageState : MonoBehaviour
{
    Image ThisImage;
    public Texture2D Image1;
    public Texture2D Image2;
    public Texture2D Image3;
    public Texture2D Image4;


    void Start()
    {
        ThisImage = this.GetComponent<Image>();
        LightDown();
    }

    public enum STATE
    {
        AllRight,
        Caution,
        Danger,
        hide
    }

    public void LightUp()
    {
        ThisImage.color = new Color(255, 255, 255);
    }

    public void LightDown()
    {
        ThisImage.color = new Color(200, 200, 200);
    }

    void StateUpdate(STATE state)
    {
        switch (state)
        {
            case STATE.AllRight:
                ThisImage.material.mainTexture = Image1;
                break;
            case STATE.Caution:
                ThisImage.material.mainTexture = Image2;
                break;
            case STATE.Danger:
                ThisImage.material.mainTexture = Image3;
                break;
            case STATE.hide:
                ThisImage.material.mainTexture = Image4;
                break;
            default:
                break;
        }
    }

}

using System;
using UnityEngine;

public class GameClearEvent : MonoBehaviour
{
    private int keyCount = 0;
    public GameObject EventImage;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Event(keyCount);
            keyCount++;
        }
    }

    private void Event(int eventNo)
    {
        switch (eventNo)
        {
            case 0:

                break;
            case 1:
                break;
            default:
                break;
        }
    }
}

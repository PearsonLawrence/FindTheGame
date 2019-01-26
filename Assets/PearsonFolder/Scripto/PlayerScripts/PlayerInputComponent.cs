using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    //horizontal axis and vertical axis stored input
    public Vector3 HorzVertIP;

    public Vector2 CurrentMousePosition;
    public Vector3 CurrentJoyStickPosition;
    // public Vector3 WorldMousePosition;

    public Camera MainCamera;

    public bool MouseLeft;
    public bool MouseRight;

    public bool Sprinting;

    public bool Crawling;

    public void UpdateInput()
    {
        //Sprinting = Input.GetKey(KeyCode.LeftShift);
        Crawling = Input.GetKey(KeyCode.C);


        HorzVertIP.x = Input.GetAxisRaw("Horizontal");
        HorzVertIP.z = Input.GetAxisRaw("Vertical");
        HorzVertIP.y = 0;

        CurrentJoyStickPosition.x = Input.GetAxisRaw("RJXAxis");
        CurrentJoyStickPosition.z = Input.GetAxisRaw("RJYAxis");
        Debug.Log(CurrentJoyStickPosition);

        CurrentMousePosition = Input.mousePosition;
        MouseLeft = Input.GetMouseButton(0);
        MouseRight = Input.GetMouseButton(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }
}

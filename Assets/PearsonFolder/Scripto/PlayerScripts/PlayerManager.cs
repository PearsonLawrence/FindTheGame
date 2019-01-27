﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static bool HasGame;
    public static bool IsInBed;

    public bool stressLevel;

    public GameObject Mother;

    public Vector3 LastPosition;

    public bool isHiding;

    public GameObject viewLight;

    private UIManager uim;

    public void StopHiding()
    {
        transform.position = LastPosition;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        isHiding = false;
        //viewLight.SetActive(true);
        GameObject.Find("UIManager").GetComponent<UIManager>().BoyHeartLevel = 0;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private float distancetomom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}

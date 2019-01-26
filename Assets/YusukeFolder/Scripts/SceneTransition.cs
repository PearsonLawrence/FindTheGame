﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string scene;
    public bool isUnlock;

    //void Start()
    //{
    //    Debug.Log(scene);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && isUnlock)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (scene.Length > 0)
            SceneManager.LoadScene(scene);
    }
}
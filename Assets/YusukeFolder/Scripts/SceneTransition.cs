using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    public string scene;
    public bool isUnlock;

    void Start()
    {
        Debug.Log(scene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && isUnlock)
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiUse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("mainSceneUI", LoadSceneMode.Additive);
        Debug.Log("mainSceneUI");
    }

}

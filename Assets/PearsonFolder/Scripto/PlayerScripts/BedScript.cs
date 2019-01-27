using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BedScript : MonoBehaviour, Iinteractable
{
    public void Activate()
    {

    }

    public bool isGoal;

    public void Interact(GameObject Owner)
    {
        PlayerManager temp = Owner.GetComponent<PlayerManager>();
        if(temp)
        {
            if(isGoal)
            {

            }
            else
            {

            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

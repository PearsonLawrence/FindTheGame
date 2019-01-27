using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConsole : MonoBehaviour, Iinteractable
{
    public virtual void Activate()
    {

    }

    public virtual void Interact(GameObject Owner)
    {
        PlayerManager temp = Owner.GetComponent<PlayerManager>();
        if (temp)
        {

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChestScripts : MonoBehaviour, Iinteractable
{
    public void Activate()
    {

    }

    public void Interact(GameObject Owner)
    {
        PlayerManager temp = Owner.GetComponent<PlayerManager>();
        if (temp)
        {
            if (hasGame)
            {
                temp.HasGame = hasGame;

            }
            if (CanHide)
            {
                PlayerManager Manager = Owner.GetComponent<PlayerManager>();
                Manager.isHiding = true;
                Manager.LastPosition = Owner.transform.position;
                Rigidbody rb = Owner.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                Owner.GetComponent<CapsuleCollider>().enabled = false;
                Owner.transform.position = transform.position;
                Owner.transform.forward = transform.forward;

                if(Manager.HasGame && IsGoal)
                {
                    SceneManager.LoadScene("GameClear");
                }
            }

            
            
        }
    }

    public bool hasGame;
    public bool CanHide;
    public bool IsGoal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

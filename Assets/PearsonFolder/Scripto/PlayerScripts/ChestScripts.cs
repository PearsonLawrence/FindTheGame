using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChestScripts : MonoBehaviour, Iinteractable
{
    public void Activate()
    {

    }

    UIManager Manager;

    public void Interact(GameObject Owner)
    {
        PlayerManager temp = Owner.GetComponent<PlayerManager>();
        if (temp)
        {
            if (hasGame)
            {
                PlayerManager.HasGame = hasGame;
                Manager = GameObject.Find("UIManager").GetComponent<UIManager>();
                Manager.GetedGameBoy = PlayerManager.HasGame;

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
                GameObject.Find("UIManager").GetComponent<UIManager>().BoyHeartLevel = 3;
                if (PlayerManager.HasGame && IsGoal)
                {
                    GameObject.Find("UIManager").GetComponent<UIManager>().GameClear();
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

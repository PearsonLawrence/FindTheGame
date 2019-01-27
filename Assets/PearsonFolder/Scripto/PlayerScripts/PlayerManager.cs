using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public bool HasGame;

    public bool stressLevel;

    public GameObject Mother;

    public Vector3 LastPosition;

    public bool isHiding;

    public GameObject viewLight;

    public void StopHiding()
    {
        transform.position = LastPosition;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        isHiding = false;
        //viewLight.SetActive(true);
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

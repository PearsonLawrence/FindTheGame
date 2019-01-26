using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public float CurrentSpeed;
    public Animator anim;
    public Rigidbody RB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", RB.velocity.magnitude);
    }
}

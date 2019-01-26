using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMotherCamera : MonoBehaviour
{
    public Transform mTarget;
    public float mDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var move_pos = mTarget.position;
        move_pos.y += mDistance;
        transform.position = move_pos;

        transform.LookAt(mTarget.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CameraSpeed;
    public Vector3 Offset;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        Offset = transform.position - Target.transform.position;
    }

    

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = MathLib.LerpToTargetOffset(transform, Target, Offset, CameraSpeed);
    }
}

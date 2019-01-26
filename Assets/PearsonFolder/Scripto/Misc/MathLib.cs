using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathLib : MonoBehaviour
{
    public static Vector3 ClampVector(Vector3 VectorToClamp, float min, float max, bool ClampX = true, bool ClampY = true, bool ClampZ = true)
    {
        Vector3 Result = VectorToClamp;

        if (ClampX)
            Result.x = Mathf.Clamp(VectorToClamp.x, min, max);
        if (ClampY)
            Result.y = Mathf.Clamp(VectorToClamp.y, min, max);
        if (ClampZ)
            Result.z = Mathf.Clamp(VectorToClamp.z, min, max);
        
        return Result;
    }

    public static RaycastHit RaycastFromScreenPoint(Vector2 ScreenPoint, Camera cam)
    {
        RaycastHit Result;

        Ray ray = cam.ScreenPointToRay(ScreenPoint);

        Physics.Raycast(ray, out Result, 10000.0f);

        return Result;

    }
    //public static void ClampVector(out Vector3 OutVectorToClamp, float min, float max, bool ClampX = true, bool ClampY = true, bool ClampZ = true)
    //{
    //    Vector3 Result = Vector3.zero; ;

    //    if (ClampX)
    //        Result.x = Mathf.Clamp(OutVectorToClamp.x, min, max);
    //    if(ClampY)
    //        Result.y = Mathf.Clamp(OutVectorToClamp.y, min, max);
    //    if(ClampZ)
    //        Result.y = Mathf.Clamp(OutVectorToClamp.z, min, max);


    //    OutVectorToClamp = Result;


    //}
    public static Vector3 LerpToTargetOffset(Transform trans, GameObject Target, Vector3 offset, float lerpspeed)
    {
        Vector3 Result;

        Result = Vector3.Lerp(trans.position, Target.transform.position + offset, lerpspeed * Time.deltaTime);

        return Result;
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

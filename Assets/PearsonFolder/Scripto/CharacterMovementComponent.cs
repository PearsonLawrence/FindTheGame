using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementComponent : MonoBehaviour
{
    public float WalkAcceleration = 1000.0f;
    public float CrawlAcceleration = 500.0f;
    public float SprintAcceleration = 3000.0f;
    private float CurrentAcceleration;

    public float MaxWalkSpeed = .5f;
    public float MaxCrawlSpeed = .25f;
    public float MaxSprintSpeed = 1;
    private float CurrentMaxSpeed;

    public float PlayerTurnSpeed = 5.0f;

    private Rigidbody RB;

    public PlayerInputComponent InputComponent;

    public Light VisionLight;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();

    }



    public void DoMovement(PlayerInputComponent IPComp, Rigidbody rb, float acceleration, float maxspeed)
    {
        RB.AddForce(IPComp.HorzVertIP * CurrentAcceleration * Time.deltaTime);
        
        RB.velocity = MathLib.ClampVector(RB.velocity, -maxspeed, maxspeed, true, false);

    }
    
   

    public void UpdateSpeedVariables()
    {
        CurrentAcceleration = (InputComponent.Crawling) ? CrawlAcceleration : (InputComponent.Sprinting) ? SprintAcceleration : WalkAcceleration;
        CurrentMaxSpeed = (InputComponent.Crawling) ? MaxCrawlSpeed : (InputComponent.Sprinting) ? MaxSprintSpeed : MaxWalkSpeed;


    }

    

    public void DoMouseLook(PlayerInputComponent IP)
    {
        if (IP.MainCamera != null)
        {
            if (IP.CurrentJoyStickPosition == Vector3.zero)
            {
                RaycastHit hit = MathLib.RaycastFromScreenPoint(IP.CurrentMousePosition, IP.MainCamera);

                Vector3 Dir = (hit.point - transform.position).normalized;
                Dir.y = 0;

                transform.forward = Vector3.Lerp(transform.forward, Dir, PlayerTurnSpeed * Time.deltaTime);

                VisionLight.spotAngle = 130 - RB.angularVelocity.magnitude; ;
                Debug.Log(RB.angularVelocity);
            }
            else
            {
                Vector3 Dir = Vector3.Normalize(IP.CurrentJoyStickPosition * 100) + transform.position;
                transform.forward = Dir;
            }
        }
        else
        {
            Debug.Log("ERROR: ASSIGN MAIN CAMERA");
        }
    }
    private void Update()
    {
        UpdateSpeedVariables();
    }

    private void FixedUpdate()
    {
        DoMouseLook(InputComponent);
        DoMovement(InputComponent, RB, CurrentAcceleration, CurrentMaxSpeed);
    }
    
}

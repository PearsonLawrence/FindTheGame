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
            Vector3 LastForward = transform.forward ;
            if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                RaycastHit hit = MathLib.RaycastFromScreenPoint(IP.CurrentMousePosition, IP.MainCamera);

                Vector3 Dir = (hit.point - transform.position).normalized;
                Dir.y = 0;

                transform.forward = Vector3.Lerp(transform.forward, Dir, PlayerTurnSpeed * Time.deltaTime);
                LastForward = transform.forward;
                Debug.Log(RB.angularVelocity);
            }
            else if((InputComponent.CurrentJoyStickPosition.x > .1f || InputComponent.CurrentJoyStickPosition.x < -.1f) || (InputComponent.CurrentJoyStickPosition.y > .1f || InputComponent.CurrentJoyStickPosition.y < -.1f))
            {
                Vector3 JoysticPosition = transform.position + (InputComponent.CurrentJoyStickPosition * 20);
                Vector3 Dir = (transform.position - JoysticPosition).normalized;
                Dir.y = 0;
                Dir.Normalize();
                transform.forward = Vector3.Lerp(transform.forward, Dir, PlayerTurnSpeed * Time.deltaTime);
                LastForward = transform.forward;
            }
            else
            {
                 transform.forward = LastForward;
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
        Debug.Log(RB.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        DoMouseLook(InputComponent);
        DoMovement(InputComponent, RB, CurrentAcceleration, CurrentMaxSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("CollideSound"))
        {
            if(RB.velocity.magnitude > .5f)
            {
                GameObject Temp = Instantiate(collision.gameObject.GetComponent<CollisionSoundSpawn>().NoisePrefab, transform.position, Quaternion.identity);
                Temp.GetComponent<NoiseComponent>().NoiseLevel = 4;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FootStepComponent : MonoBehaviour
{

    public enum FloorType
    {
        OldWood,
        Bathroom,
        Tatami,
        Carpet
    }

    public FloorType CurrentFloorType;

    private Rigidbody RB;

    public GameObject StoredNoisePrefab;

    public ParticleSystem PS;

    public GameObject FootPoint1, FootPoint2;
    private GameObject CurrentFootPoint;
    public float walkfrequency = 1;
    private float countdown;
    public AudioClip[] FootStepType;

    NavMeshAgent agent;

    //0 wood 
    //1 tatami
    //2 bathroom
    //3 carpet
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        CurrentFootPoint = FootPoint1;
    }

    public void Step()
    {
        RaycastHit hit;

        
        Physics.Raycast(CurrentFootPoint.transform.position, -transform.up, out hit, 5.0f);

        

        if(hit.collider.CompareTag("Wood"))
        {
            GameObject newNoise = Instantiate(StoredNoisePrefab, CurrentFootPoint.transform.position, CurrentFootPoint.transform.rotation);
            newNoise.transform.localScale = CurrentFootPoint.transform.localScale;
            NoiseComponent CurrentNoise = newNoise.GetComponent<NoiseComponent>();
            CurrentNoise.Owner = gameObject;
            CurrentNoise.NoiseLevel = 4;
            CurrentNoise.decreasespeed = .7f;
            CurrentNoise.MaxSize = 7;
            CurrentNoise.audiosource.PlayOneShot(FootStepType[0]);

        }
        else if (hit.collider.CompareTag("Tatami"))
        {
            GameObject newNoise = Instantiate(StoredNoisePrefab, CurrentFootPoint.transform.position, CurrentFootPoint.transform.rotation);
            newNoise.transform.localScale = CurrentFootPoint.transform.localScale;
            NoiseComponent CurrentNoise = newNoise.GetComponent<NoiseComponent>();
            CurrentNoise.Owner = gameObject;
            CurrentNoise.NoiseLevel = 1.5f;
            CurrentNoise.MaxSize = 7;
            CurrentNoise.audiosource.PlayOneShot(FootStepType[1]);
        }
        else if (hit.collider.CompareTag("Bathroom"))
        {
            GameObject newNoise = Instantiate(StoredNoisePrefab, CurrentFootPoint.transform.position, CurrentFootPoint.transform.rotation);
            newNoise.transform.localScale = CurrentFootPoint.transform.localScale;
            NoiseComponent CurrentNoise = newNoise.GetComponent<NoiseComponent>();
            CurrentNoise.Owner = gameObject;
            CurrentNoise.NoiseLevel = 3;
            CurrentNoise.decreasespeed = .6f;
            CurrentNoise.MaxSize = 7;
            CurrentNoise.audiosource.PlayOneShot(FootStepType[2]);
        }
        else if (hit.collider.CompareTag("Carpet"))
        {
            GameObject newNoise = Instantiate(StoredNoisePrefab, CurrentFootPoint.transform.position, CurrentFootPoint.transform.rotation);
            newNoise.transform.localScale = CurrentFootPoint.transform.localScale;
            NoiseComponent CurrentNoise = newNoise.GetComponent<NoiseComponent>();
            CurrentNoise.Owner = gameObject;
            CurrentNoise.NoiseLevel = 1.5f;
            CurrentNoise.decreasespeed = .8f;
            CurrentNoise.MaxSize = 7;
            CurrentNoise.audiosource.PlayOneShot(FootStepType[3]);
        }
        CurrentFootPoint = (CurrentFootPoint == FootPoint1) ? FootPoint2 : FootPoint1;
    }

    public void SpawnFootstep()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        

        Vector3 CurrentVelocity = (agent != null) ? agent.velocity : RB.velocity;

        if(countdown <= 0 && CurrentVelocity != Vector3.zero)
        {
            Step();
            countdown = walkfrequency;
        }
    }
}

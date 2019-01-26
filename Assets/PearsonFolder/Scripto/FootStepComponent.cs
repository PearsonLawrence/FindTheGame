using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepComponent : MonoBehaviour
{

    public enum FloorType
    {
        Wood,
        OldWood,
        HardTile,
        Carpet
    }

    public FloorType CurrentFloorType;

    public NoiseComponent StoredNoise;

    public ParticleSystem PS;

    public AudioClip[] FootStepType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetFloorType()
    {
        RaycastHit hit;

        
        Physics.Raycast(transform.position, -transform.up, out hit, 10.0f);

        if(hit.collider.CompareTag("Wood"))
        {

        }

    }

    public void SpawnFootstep()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

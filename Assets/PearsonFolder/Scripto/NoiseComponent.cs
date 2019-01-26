using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseComponent : MonoBehaviour
{
    public float NoiseLevel;

    public float decreasespeed;

    public float ExpandSpeed;

    public float MaxSize = 30;

    public GameObject Owner;

    private SphereCollider spherecollider;

    public ParticleSystem SoundWave;

    // Start is called before the first frame update
    void Start()
    {
        spherecollider = GetComponent<SphereCollider>();
        spherecollider.radius = 0;
    }

    //Expands spherecollision and decreases noise level slowly
    public void ExpandCollision()
    {
        spherecollider.radius += Time.deltaTime * ExpandSpeed;
        NoiseLevel -= Time.deltaTime * decreasespeed;

        if(NoiseLevel <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExpandCollision();
    }
}

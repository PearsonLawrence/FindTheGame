using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoiseComponent : MonoBehaviour
{
    public float NoiseLevel;

    public float decreasespeed;

    public float ExpandSpeed;

    public float MaxSize = 30;

    public GameObject Owner;

    private SphereCollider spherecollider;

    public ParticleSystem SoundWave;

    public AudioSource audiosource;

    public  Image PictureSound;
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
        if(PictureSound)
        {
            PictureSound.color -= (PictureSound.color.a <= 0) ? new Color(0,0,0,0) : new Color(0,0,0,Time.deltaTime * .5f);
        }
        if(NoiseLevel <= 0 || spherecollider.radius > MaxSize)
        {
            Destroy(gameObject,2);
            spherecollider.radius = 0;

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SoundDetectionComponent TempSoundDetect = other.GetComponent<SoundDetectionComponent>();
        if (TempSoundDetect)
        {
            if(TempSoundDetect.CurrentNoiseLevel < NoiseLevel)
            {
                TempSoundDetect.CurrentNoiseLevel = NoiseLevel;
                TempSoundDetect.PointofInterest = transform.position;
               
            }
            TempSoundDetect.CurrentNoiseStress += NoiseLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ExpandCollision();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetectionComponent : MonoBehaviour
{

    public bool IsPlayer;

    public float ListenRadius;


    public List<NoiseComponent> CurrentNoises;

    public NoiseComponent LoudestNoise;

    public Vector3 PointofInterest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        NoiseComponent Temp = other.GetComponent<NoiseComponent>();
        if(Temp)
        {
            CurrentNoises.Add(Temp);
        }
    }

    public void CheckNoiseLevel()
    {
        for(int i = 0; i < CurrentNoises.Capacity; i++)
        {
            switch(CurrentNoises[i].NoiseLevel)
            {
                case 0:

                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetectionComponent : MonoBehaviour
{

    public bool IsPlayer = false;

    //public float ListenRadius;

    public float CurrentNoiseLevel;
    //Important

    public float CurrentNoiseStress; //??
    public float StressDecreaseModifier = .2f;

  //  public NoiseComponent LoudestNoise;

    public Vector3 PointofInterest;
    //Important

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!IsPlayer)
        {
            CurrentNoiseStress -= Time.deltaTime * StressDecreaseModifier;
            CurrentNoiseStress = Mathf.Clamp(CurrentNoiseStress, 0, 5);
        }
    }
}

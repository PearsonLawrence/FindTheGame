using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepManager : MonoBehaviour
{

    public float FootStepFrequency;

    private float footStepCounter;

    public GameObject FootStepPrefab;

    float DT;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnFootstep()
    {

        GameObject Temp = Instantiate(FootStepPrefab, transform.position, Quaternion.identity);
        //Temp.GetComponent<
    }

    // Update is called once per frame
    void Update()
    {
        DT = Time.deltaTime;

    }
}

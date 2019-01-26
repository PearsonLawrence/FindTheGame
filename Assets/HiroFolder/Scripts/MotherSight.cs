using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MotherSight : MonoBehaviour
{
    private bool mWatchablePlayer = false;
    private Transform mOwner;

    public bool WatchablePlayer { get{ return mWatchablePlayer; } }

    // Start is called before the first frame update
    void Start()
    {
        mOwner = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //コリジョンとレイキャストでプレイヤーが見えているか判断
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            var test = LayerMask.NameToLayer("Mother");
            int layer_mask = ~(1 << LayerMask.NameToLayer("Mother"));
            RaycastHit hit;
            Physics.Raycast(mOwner.position, (other.transform.position - mOwner.position).normalized, out hit, 100.0f, layer_mask);
            Debug.DrawLine(mOwner.position, other.transform.position, Color.red);
            if(!hit.collider)
            {
                mWatchablePlayer = false;
                return;
            }
            if (hit.collider.tag == "Player")
            {
                mWatchablePlayer = true;
                Debug.Log("Find Player!");
                if(transform.parent.GetComponent<Mother>().mGameOverSceneName == "")
                {
                    Debug.LogError("Set GameOver scene name in Mother AI.");
                }
                SceneManager.LoadScene(transform.parent.GetComponent<Mother>().mGameOverSceneName);
            }
            else
            {
                mWatchablePlayer = false;
            }
        }
        else
        {
            mWatchablePlayer = false;
        }
    }
}

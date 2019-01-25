using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoBehaviour
{
    public float mMoveSpeed = 1.0f;
    private Rigidbody mRigidbody;
    private NavMeshAgent mNavMeshAgent;
    private Vector3 mCurrentTargetPos;

    private const float cReachDistance = 3.0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mRigidbody = gameObject.GetComponent<Rigidbody>();
        if(!mRigidbody)
        {
            Debug.LogError("Can't find rigidbody");
        }
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        if(!mNavMeshAgent)
        {
            Debug.LogError("Can't find nav mesh agent.");
        }

        var rand = Random.Range(0, 6);
        mCurrentTargetPos = GameObject.Find("Goal (" + rand.ToString() + ")").transform.position;
        Debug.Log(rand + "\n" + mCurrentTargetPos);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Vector3.Distance(transform.position, mNavMeshAgent.destination) < cReachDistance)
        {
            var rand = Random.Range(0, 6);
            mCurrentTargetPos = GameObject.Find("Goal (" + rand.ToString() + ")").transform.position;
        }

        mNavMeshAgent.destination = mCurrentTargetPos;
    }

    protected void Move(Vector3 direction)
    {
        direction.y = 0.0f;
        direction.Normalize();

        if(mRigidbody)
        {
            mRigidbody.MovePosition(transform.position + direction * mMoveSpeed * Time.deltaTime);
        }  
        else
        {
            Debug.LogWarning("Can't get Rigidbody. Translate move.");
            gameObject.transform.Translate(direction * mMoveSpeed * Time.deltaTime);
        }
    }

    protected void Rotate(Vector3 direction)
    {
        direction.y = 0.0f;
        direction.Normalize();

        transform.LookAt(transform.position + direction);
    }

    void DrawGizmo()
    {
        Gizmos.DrawSphere(mCurrentTargetPos, 1.0f);
    }
}

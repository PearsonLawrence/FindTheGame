using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoBehaviour
{
    //*********************************************************************************************
    //メンバ変数

    public float mMoveSpeed = 0.4f;
    public float mTargetReachedStopTime = 1.0f;

    private Rigidbody mRigidbody;
    private NavMeshAgent mNavMeshAgent;
    private Vector3 mCurrentTargetPos = Vector3.zero;
    private Dictionary<int, Vector3> mTargetPoints = new Dictionary<int, Vector3>();
    private Counter mTargetReachedStopCounter;

    //State
    private enum MoveState
    {
        cNone,
        cOrderPatrol,        //目標点を順番に
        cRandomPatrol,       //目標点をランダムに
        cToTarget,           //目的の場所へ
        cVigilance           //警戒
    }
    private MoveState mCurrentMoveState;
    private int mOrderIndex = 0;

    private const float cReachDistance = 2.0f;

    //*********************************************************************************************
    //Unityデフォルト関数

    // Start is called before the first frame update
    private void Start()
    {
        mRigidbody = gameObject.GetComponent<Rigidbody>();
        if (!mRigidbody)
        {
            Debug.LogError("Can't find rigidbody");
        }
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        if (!mNavMeshAgent)
        {
            Debug.LogError("Can't find nav mesh agent.");
        }

        //目標点リストの作成
        var points = GameObject.FindGameObjectsWithTag("TargetPoint");
        foreach(var point in points)
        {
            var key = int.Parse(point.name);
            mTargetPoints.Add(key, point.transform.position);
        }
        //*** Stateに応じて初期の目標点をかえるように
        var random_pos = CalcRandomPos();
        mCurrentTargetPos = random_pos;
        mCurrentMoveState = MoveState.cRandomPatrol;

        mTargetReachedStopCounter = new Counter(mTargetReachedStopTime, -1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        WatchNowState();
    }

    //*********************************************************************************************
    //メンバ関数

    /// <summary>
    /// 現在のStateを監視してStateに応じた行動をする
    /// </summary>
    private void WatchNowState()
    {
        switch (mCurrentMoveState)
        {
            case MoveState.cOrderPatrol:
            {
                if(Vector3.Distance(transform.position, mCurrentTargetPos) < cReachDistance)
                {
                    mTargetReachedStopCounter.Update();
                    if(mTargetReachedStopCounter.IsUnderZero())
                    {

                        mCurrentTargetPos = CalcOrderPos();
                    }
                }
                mNavMeshAgent.destination = mCurrentTargetPos;

                break;
            }

            case MoveState.cRandomPatrol:
            {
                //新たな目標点の検索
                if (Vector3.Distance(transform.position, mCurrentTargetPos) < cReachDistance)
                {
                    mTargetReachedStopCounter.Update();
                    if(mTargetReachedStopCounter.IsUnderZero())
                    {
                        mTargetReachedStopCounter.InitCount(mTargetReachedStopTime);
                        mCurrentTargetPos = CalcRandomPos();
                    }
                }

                mNavMeshAgent.destination = mCurrentTargetPos;

                break;
            }

            case MoveState.cVigilance:
            {
                break;
            }

            case MoveState.cToTarget:
            {
                break;
            }

            default:
            {
                break;
            }
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction">移動方向</param>
    private void Move(Vector3 direction)
    {
        direction.y = 0.0f;
        direction.Normalize();

        if (mRigidbody)
        {
            mRigidbody.MovePosition(transform.position + direction * mMoveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Can't get Rigidbody. Translate move.");
            gameObject.transform.Translate(direction * mMoveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 回転
    /// </summary>
    /// <param name="direction">進行方向</param>
    private void Rotate(Vector3 direction)
    {
        direction.y = 0.0f;
        direction.Normalize();

        transform.LookAt(transform.position + direction);
    }

    /// <summary>
    /// 目標点リストから目標点をランダムにとる
    /// </summary>
    /// <returns>次に向かう点</returns>
     private Vector3 CalcRandomPos()
    {
        var random = Random.Range(0, mTargetPoints.Count);
        var target_pos = mTargetPoints[random];
        return target_pos;
    }
    
    private Vector3 CalcOrderPos()
    {
        
    }

    /// <summary>
    /// 時間を計るカウンター
    /// Step: 1.0fだと、1秒間で1進む, 2.0fだと1秒で2進む
    ///       負数にするとCountを減らすことが出来る
    /// </summary>
    class Counter
    {
        private float mCount;
        private float mStep;

        public float Count { get { return mCount; } }
        public float Step { get { return mStep; } }

        public Counter()
            : this(0.0f, 0.0f)
        {
        }
        public Counter(float count, float step)
        {
            mCount = count;
            mStep = step;
        }

        ~Counter()
        {
        }

        public void InitCount(float count)
        {
            mCount = count;
        }

        public void Update()
        {
            mCount += mStep * Time.deltaTime;
        }

        public bool IsUnderZero()
        {
            return mCount < 0.0f;
        }
    }
}

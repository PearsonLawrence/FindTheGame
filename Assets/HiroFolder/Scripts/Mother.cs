﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoBehaviour
{
    //*********************************************************************************************
    //メンバ変数

    public float                        mMoveSpeed = 3.5f;

    [Space(10)]
    public float                        mTargetReachedStopTime = 1.0f;
    public float                        mLookAroundTime = 1.0f;

    [Space(10)]
    public float                        mNoiseReactionStopTime = 1.5f;
    public float                        mNoiseReactionLookAroundTime = 5.0f;
    public float                        mNosieReactionMoveToPointInterstSpeed = 3.5f;
    public float                        mNosieReactionMoveToPointInterstQuicklySpeed = 5.0f;

    private Rigidbody                   mRigidbody;
    private NavMeshAgent                mNavMeshAgent;
    private Animator                    mMomAnimator;
    private SoundDetectionComponent     mSoundDirectionComponent;
    private Vector3                     mCurrentTargetPos = Vector3.zero;
    private Dictionary<int, Vector3>    mTargetPoints = new Dictionary<int, Vector3>();
    private Counter                     mTargetReachedStopCounter;
    private Counter                     mNoiseReactionCounter = new Counter(0.0f, 0.0f);
    private SoundFootStep               mSoundFootStep;

    private Vector3                     mOldPos;
    private Counter                     mLookAroundCounter;
    private Vector3                     mCurrentLookDir;

    //NoiseLevel
    public enum NoiseLevel
    {
        cNone = 0,
        cStop = 1,
        cLookAround = 2,
        cMoveToPointInterst = 3,
        cMoveToPointInterstQuickly = 4
    }
    [Space(10)]
    public NoiseLevel mDoingNoiseLevelAction = NoiseLevel.cNone;

    //State
    public enum MoveState
    {
        cNone,
        cOrderPatrol,        //目標点を順番に
        cRandomPatrol,       //目標点をランダムに
        cNoiseReaction,      //音に応じたリアクション
        cFindPlayer          //プレイヤー発見
    }
    public MoveState mCurrentMoveState = MoveState.cOrderPatrol;
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
        mSoundDirectionComponent = gameObject.GetComponent<SoundDetectionComponent>();
        if(!mSoundDirectionComponent)
        {
            Debug.LogError("Can't find sound direction component.");
        }
        mMomAnimator = transform.GetChild(0).GetComponent<Animator>();
        if(!mMomAnimator)
        {
            Debug.LogError("Can't find Animator component.");
        }
        mSoundFootStep = GetComponent<SoundFootStep>();
        if (!mMomAnimator)
        {
            Debug.LogError("Can't find Sound foot step component.");
        }

        //目標点リストの作成
        var points = GameObject.FindGameObjectsWithTag("TargetPoint");
        foreach(var point in points)
        {
            var key = int.Parse(point.name);
            mTargetPoints.Add(key, point.transform.position);
        }

        InitMove();

        mTargetReachedStopCounter = new Counter(mTargetReachedStopTime, -1.0f);
        mLookAroundCounter = new Counter(mLookAroundTime, -1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Vector3.Distance(transform.position, mOldPos) > 0.01f)
        {
            var move_speed = Vector3.Distance(transform.position, mOldPos);
            mMomAnimator.SetFloat("MoveSpeed", move_speed);

            mSoundFootStep.MoveSpeed = move_speed;
        }
        else
        {
            mMomAnimator.SetFloat("MoveSpeed", 0.0f);
            LookAround();
        }

        CheckNoise();
        WatchNowState();

        mOldPos = transform.position;
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
                        mTargetReachedStopCounter.InitCount(mTargetReachedStopTime);
                        mCurrentTargetPos = CalcOrderPos();
                    }
                }
                SetNavMeshDestination(mCurrentTargetPos, mMoveSpeed);

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
                SetNavMeshDestination(mCurrentTargetPos, mMoveSpeed);

                break;
            }

            //ノイズリアクション
            case MoveState.cNoiseReaction:
            {
                NoiseReaction();
                break;
            }

            case MoveState.cFindPlayer:
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
    /// 移動初期化
    /// </summary>
    private void InitMove()
    {
        switch (mCurrentMoveState)
        {
            case MoveState.cOrderPatrol:
            {
                mCurrentTargetPos = mTargetPoints[mOrderIndex];
                break;
            }

            case MoveState.cRandomPatrol:
            {
                var random_pos = CalcRandomPos();
                mCurrentTargetPos = random_pos;
                break;
            }

            case MoveState.cNoiseReaction:
            {
                NoiseReaction();
                break;
            }

            case MoveState.cFindPlayer:
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

        var current_dir = transform.forward;
        var next_dir = Vector3.Lerp(current_dir, direction, 0.2f);

        transform.LookAt(transform.position + next_dir);
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
        ++mOrderIndex;
        mOrderIndex = mOrderIndex >= mTargetPoints.Count ? 0 : mOrderIndex;

        return mTargetPoints[mOrderIndex];
    }

    /// <summary>
    /// 聴覚チェック
    /// </summary>
    private void CheckNoise()
    {
        if(mSoundDirectionComponent.CurrentNoiseStress > 0.0f)
        {
            mCurrentMoveState = MoveState.cNoiseReaction;
            var noise_stress = mSoundDirectionComponent.CurrentNoiseStress > mSoundDirectionComponent.CurrentNoiseLevel ? mSoundDirectionComponent.CurrentNoiseStress : mSoundDirectionComponent.CurrentNoiseLevel;
            mDoingNoiseLevelAction = (NoiseLevel)noise_stress;
        }
        else
        {
            mCurrentMoveState = MoveState.cOrderPatrol;
        }
    }

    //音によるリアクション
    private void NoiseReaction()
    {
        switch(mDoingNoiseLevelAction)
        {
            case NoiseLevel.cNone:
            {
                break;
            }
            case NoiseLevel.cStop:
            {
                mCurrentTargetPos = transform.position;
                break;
            }
            case NoiseLevel.cLookAround:
            {
                break;
            }
            case NoiseLevel.cMoveToPointInterst:
            {
                SetNavMeshDestination(mSoundDirectionComponent.PointofInterest, mNosieReactionMoveToPointInterstSpeed);
                break;
            }

            case NoiseLevel.cMoveToPointInterstQuickly:
            {
                SetNavMeshDestination(mSoundDirectionComponent.PointofInterest, mNosieReactionMoveToPointInterstQuicklySpeed);
                break;
            }
        }
    }

    /// <summary>
    /// NavMesh移動
    /// </summary>
    /// <param name="target_pos"></param>
    /// <param name="move_speed"></param>
    private void SetNavMeshDestination(Vector3 target_pos, float move_speed)
    {
        mNavMeshAgent.destination = target_pos;
        mNavMeshAgent.speed = move_speed;
    }

    /// <summary>
    /// 周囲を見渡す
    /// </summary>
    private void LookAround()
    {
        if (mLookAroundCounter.IsUnderZero())
        {
            var rand_pos = Random.Range(0.0f, 360.0f);
            var look_pos_offset = new Vector3(Mathf.Cos(rand_pos), 0.0f, Mathf.Sin(rand_pos));

            mCurrentLookDir = transform.position + look_pos_offset * 100;
            mLookAroundCounter = new Counter(mLookAroundTime, -1.0f);
        }
        Rotate(mCurrentLookDir);
        mLookAroundCounter.Update();
    }

    private void CheckFloorType()
    {

    }

    //*********************************************************************************************

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

    //*********************************************************************************************
    //Debug drawing

    private void OnDrawGizmos()
    {
    }
}

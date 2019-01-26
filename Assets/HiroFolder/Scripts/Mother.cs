﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mother : MonoBehaviour
{
    //*********************************************************************************************
    //メンバ変数

    public float                        mMoveSpeed = 0.4f;
    public float                        mTargetReachedStopTime = 1.0f;
    public float                        mNoseReactionStopTime = 1.5f;
    public float                        mNoiseReactionLookAroundTime = 5.0f;
    public float                        mNosieReactionMoveToPointInterstSpeed = 0.4f;
    public float                        mNosieReactionMoveToPointInterstQuicklySpeed = 1.0f;

    private Rigidbody                   mRigidbody;
    private NavMeshAgent                mNavMeshAgent;
    private SoundDetectionComponent     mSoundDirectionComponent;
    private Vector3                     mCurrentTargetPos = Vector3.zero;
    private Dictionary<int, Vector3>    mTargetPoints = new Dictionary<int, Vector3>();
    private Counter                     mTargetReachedStopCounter;
    private Counter                     mNoiseReactionCounter;

    //NoiseLevel
    public enum NoiseLevel
    {
        cNone = 0,
        cStop = 1,
        cLookAround = 2,
        cMoveToPointInterst = 3,
        cMoveToPointInterstQuickly = 4
    }
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
    public MoveState mOldMoveState = MoveState.cOrderPatrol;
    private int mOrderIndex = 0;

    private const float cReachDistance = 2.0f;

    private bool mIsNoiseActionStopCoroutine = false;
    private bool mIsNoiseActionLookAroundCoroutine = false;
    private bool mIsNoiseActionMoveToPointInterstCoroutine = false;
    private bool mIsNoiseActionMoveToPointInterstQuicklyCoroutine = false;

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

        //目標点リストの作成
        var points = GameObject.FindGameObjectsWithTag("TargetPoint");
        foreach(var point in points)
        {
            var key = int.Parse(point.name);
            mTargetPoints.Add(key, point.transform.position);
        }

        InitMove();

        mTargetReachedStopCounter = new Counter(mTargetReachedStopTime, -1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        CheckNoise();
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
                        mTargetReachedStopCounter.InitCount(mTargetReachedStopTime);
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
            mOldMoveState = mCurrentMoveState;
            mCurrentMoveState = MoveState.cNoiseReaction;
            mDoingNoiseLevelAction = (NoiseLevel)mSoundDirectionComponent.CurrentNoiseLevel;
        }
    }

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
                mNoiseReactionCounter = new Counter(mNoseReactionStopTime, -1.0f);
                StartCoroutine(NoiseStopAction());
                break;
            }

            case NoiseLevel.cLookAround:
            {
                mNoiseReactionCounter = new Counter(mNoiseReactionLookAroundTime, -1.0f);
                StartCoroutine(NoiseLookAroundAction());
                break;
            }

            case NoiseLevel.cMoveToPointInterst:
            {
                StartCoroutine(NoiseMoveToPointInterstAction());
                break;
            }

            case NoiseLevel.cMoveToPointInterstQuickly:
            {
                StartCoroutine(NoiseMoveToPointInterstQuicklyAction());
                break;
            }
        }
    }

    /// <summary>
    /// ConeとRayCastでプレイヤーを見つけているかチェック
    /// </summary>
    /// <returns></returns>
    private bool IsFindPlayer()
    {
        return false;
    }

    //*********************************************************************************************
    //NoiseReactionFunctions

    private IEnumerator NoiseStopAction()
    {
        if(mIsNoiseActionStopCoroutine)
        {
            yield break;
        }
        mIsNoiseActionStopCoroutine = true;
        while(!mNoiseReactionCounter.IsUnderZero())
        {
            Debug.Log("Now noise reaction 'Stop' playing.");
            mNoiseReactionCounter.Update();
            yield return null;
        }
        var current_state = mCurrentMoveState;
        mCurrentMoveState = mOldMoveState;
        mOldMoveState = current_state;
        mIsNoiseActionStopCoroutine = false;
        yield break;
    }
    private IEnumerator NoiseLookAroundAction()
    {
        if(mIsNoiseActionLookAroundCoroutine)
        {
            yield break;
        }
        mIsNoiseActionLookAroundCoroutine = true;
        while (!mNoiseReactionCounter.IsUnderZero())
        {
            Debug.Log("Now noise reaction 'LookAround' playing.");
            mNoiseReactionCounter.Update();
            yield return null;
        }
        var current_state = mCurrentMoveState;
        mCurrentMoveState = mOldMoveState;
        mOldMoveState = current_state;

        mIsNoiseActionLookAroundCoroutine = false;
        yield break;
    }

    private IEnumerator NoiseMoveToPointInterstAction()
    {
        if(mIsNoiseActionMoveToPointInterstCoroutine)
        {
            yield break;
        }
        mIsNoiseActionMoveToPointInterstCoroutine = true;
        var infinity_check = 0;
        while (Vector3.Distance(transform.position, mCurrentTargetPos) < cReachDistance)
        {
            Debug.Log("Now noise reaction 'Stop' playing.");
            mNoiseReactionCounter.Update();

            if(infinity_check > 100)
            {
                break;
            }
            ++infinity_check;
            yield return null;
        }
        var current_state = mCurrentMoveState;
        mCurrentMoveState = mOldMoveState;
        mOldMoveState = current_state;

        mIsNoiseActionMoveToPointInterstCoroutine = false;
        yield break;
    }

    private IEnumerator NoiseMoveToPointInterstQuicklyAction()
    {
        if(mIsNoiseActionMoveToPointInterstQuicklyCoroutine)
        {
            yield break;
        }
        mIsNoiseActionMoveToPointInterstQuicklyCoroutine = true;
        while (Vector3.Distance(transform.position, mCurrentTargetPos) < cReachDistance)
        {
            var infinity_check = 0;
            Debug.Log("Now noise reaction 'Stop' playing.");
            mNoiseReactionCounter.Update();

            if(infinity_check > 100)
            {
                break;
            }
            ++infinity_check;
            yield return null;
        }
        var current_state = mCurrentMoveState;
        mCurrentMoveState = mOldMoveState;
        mOldMoveState = current_state;

        mIsNoiseActionMoveToPointInterstQuicklyCoroutine = false;
        yield break;
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

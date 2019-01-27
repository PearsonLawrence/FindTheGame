using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFootStep : MonoBehaviour
{
    public GameObject mNoiseSpeher;

    [HeaderAttribute("TatamiNoise")]
    public int      mNoiseLevelTatami = 1;
    public float    mDecreaseSpeedTatami = 0.5f;
    public float    mExpandSpeedTatami = 5.0f;
    public float    mMaxSizeTatami = 5.0f;
    public AudioClip mTatamiSE;

    [HeaderAttribute("CarpetNoise")]
    public int      mNoiseLevelCarpet = 2;
    public float    mDecreaseSpeedCarpet = 0.5f;
    public float    mExpandSpeedCarpet = 5.0f;
    public float    mMaxSizeCarpet = 10.0f;
    public AudioClip mCarpetSE;

    [HeaderAttribute("FlooringNoise")]
    public int      mNoiseLevelFlooring = 3;
    public float    mDecreaseSpeedFlooring = 0.5f;
    public float    mExpandSpeedFlooring = 5.0f;
    public float    mMaxSizeFlooring = 20.0f;
    public AudioClip mFlooringSE;

    [HeaderAttribute("BathRoomNoise")]
    public int mNoiseLevelBathRoom = 3;
    public float mDecreaseSpeedBathRoom = 0.5f;
    public float mExpandSpeedBathRoom = 5.0f;
    public float mMaxSizeBathRoom = 20.0f;
    public AudioClip mBathRoomSE;

    [HeaderAttribute("OldFlooringNoise")]
    public int      mNoiseLevelOldFlooring = 4;
    public float    mDecreaseSpeedOldFlooring = 0.5f;
    public float    mExpandSpeedOldFlooring = 5.0f;
    public float    mMaxSizeOldFlooring = 30.0f;
    public AudioClip mOldFlooringSE;

    private float mMoveSpeed;
    public float MoveSpeed { set { mMoveSpeed = value; } }

    private Counter mSoundIntarvalCounter = new Counter(0.0f, -1.0f);

    private AudioSource mAudioSource;

    public enum　FloorType
    {
        cNone = 0,
        cTatami = 1,
        cCarpet = 2,
        cFlooring = 3,
        cBathRoom = 4,
        cOldFlooring = 5
    };

    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //SoundFloorTypeSE(1 / mMoveSpeed);
    }

    public void SoundFloorTypeSE(float move_speed)
    {
        RaycastHit hit;
        int layer_mask = ~(1 << LayerMask.NameToLayer("FieldObject"));
        Physics.Raycast(transform.position, Vector3.down, out hit, 10.0f, layer_mask);

        switch (hit.transform.tag)
        {
            case "Tatami":
            {
                SoundFloorTypeSE(FloorType.cTatami, move_speed);
                break;
            }
            case "Carpet":
            {
                SoundFloorTypeSE(FloorType.cCarpet, move_speed);
                break;
            }
            case "Flooring":
            {
                SoundFloorTypeSE(FloorType.cFlooring, move_speed);
                break;
            }
            case "BathRoom":
            {
                SoundFloorTypeSE(FloorType.cBathRoom, move_speed);
                break;
            }
            case "OldFlooring":
            {
                SoundFloorTypeSE(FloorType.cOldFlooring, move_speed);
                break;
            }
            default:
            {
                break;
            }
        }
    }
    public void SoundFloorTypeSE(FloorType floor_type, float move_speed)
    {
        if (mSoundIntarvalCounter.IsUnderZero())
        {
            switch (floor_type)
            {
                case FloorType.cTatami:
                {
                    GameObject noise_sphere = Instantiate<GameObject>(mNoiseSpeher, transform);
                    NoiseComponent new_noise = noise_sphere.GetComponent<NoiseComponent>();
                    new_noise.NoiseLevel = mNoiseLevelTatami;
                    new_noise.decreasespeed = mDecreaseSpeedTatami;
                    new_noise.ExpandSpeed = mExpandSpeedTatami;
                    new_noise.MaxSize = mMaxSizeTatami;
                    mAudioSource.PlayOneShot(mTatamiSE);

                    break;
                }

                case FloorType.cCarpet:
                {
                    GameObject noise_sphere = Instantiate<GameObject>(mNoiseSpeher, transform);
                    NoiseComponent new_noise = noise_sphere.GetComponent<NoiseComponent>();
                    new_noise.NoiseLevel = mDecreaseSpeedCarpet;
                    new_noise.decreasespeed = mDecreaseSpeedCarpet;
                    new_noise.ExpandSpeed = mExpandSpeedCarpet;
                    new_noise.MaxSize = mMaxSizeCarpet;
                    mAudioSource.PlayOneShot(mCarpetSE);

                    break;
                }

                case FloorType.cFlooring:
                {
                    GameObject noise_sphere = Instantiate<GameObject>(mNoiseSpeher, transform);
                    NoiseComponent new_noise = noise_sphere.GetComponent<NoiseComponent>();
                    new_noise.NoiseLevel = mNoiseLevelFlooring;
                    new_noise.decreasespeed = mDecreaseSpeedFlooring;
                    new_noise.ExpandSpeed = mExpandSpeedFlooring;
                    new_noise.MaxSize = mMaxSizeFlooring;
                    mAudioSource.PlayOneShot(mFlooringSE);

                    break;
                }

                case FloorType.cBathRoom:
                {
                    GameObject noise_sphere = Instantiate<GameObject>(mNoiseSpeher, transform);
                    NoiseComponent new_noise = noise_sphere.GetComponent<NoiseComponent>();
                    new_noise.NoiseLevel = mNoiseLevelBathRoom;
                    new_noise.decreasespeed = mDecreaseSpeedBathRoom;
                    new_noise.ExpandSpeed = mExpandSpeedBathRoom;
                    new_noise.MaxSize = mMaxSizeBathRoom;
                    mAudioSource.PlayOneShot(mBathRoomSE);

                    break;
                }

                case FloorType.cOldFlooring:
                {
                    GameObject noise_sphere = Instantiate<GameObject>(mNoiseSpeher, transform);
                    NoiseComponent new_noise = noise_sphere.GetComponent<NoiseComponent>();
                    new_noise.NoiseLevel = mNoiseLevelOldFlooring;
                    new_noise.decreasespeed = mDecreaseSpeedOldFlooring;
                    new_noise.ExpandSpeed = mExpandSpeedOldFlooring;
                    new_noise.MaxSize = mMaxSizeOldFlooring;
                    mAudioSource.PlayOneShot(mOldFlooringSE);

                    break;
                }

                default:
                {
                    break;
                }
            }

            mSoundIntarvalCounter = new Counter(move_speed, -1.0f);
        }
        mSoundIntarvalCounter.Update();
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

}

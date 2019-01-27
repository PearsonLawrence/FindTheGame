using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject LightImage;
    public GameObject BoyImage;
    public GameObject SoundImage;
    public GameObject GameOverImage;
    public GameObject GameClearImage;
    public GameObject GameBoyImageState;


    private ImageState LightImageState;
    private ImageState BoyImageState;
    private ImageState SoundImageState;
    [SerializeField]
    private bool _isInLight = false;
    [SerializeField]
    private int _boyHeartLevel = 0;
    [SerializeField]
    private int _soundLevel = 0;
    [SerializeField]
    private bool _isGetGame = false;

    private PlayerManager Player;

 

    public bool IsInLight
    {
        get { return _isInLight; }
        set
        {
            if (BoyImageState == null)
                BoyImageState = BoyImage.GetComponent<ImageState>();
            if (LightImageState == null)
                LightImageState = LightImage.GetComponent<ImageState>();

            _isInLight = value;
            if (_isInLight)
            {
                BoyImageState.LightUp();
                LightImageState.LightUp();
            }
            else
            {
                BoyImageState.LightDown();
                LightImageState.LightDown();
            }
        }
    }

 

    public bool GetedGameBoy
    {
        get { return _isGetGame; }
        set
        {
            if (PlayerManager.HasGame)
            {
                GameBoyImageState.SetActive(true);
                _isGetGame = value;
            }
        }
    }

    /// <summary>
    /// Level 0 : High tension
    /// Level 1 : A little calm
    /// Level 2 : Mother is near
    /// Level 3 : Hide In Object
    /// </summary>
    public int BoyHeartLevel
    {
        get { return _boyHeartLevel; }
        set
        {
            if (BoyImageState == null)
                BoyImageState = BoyImage.GetComponent<ImageState>();
            _boyHeartLevel = RangePositivInt(value, BoyImageState.imageList.Count);
            BoyImageState.StateUpdate(_boyHeartLevel);
        }
    }

    /// <summary>
    /// Level 0 : Silent
    /// Level 1 : Soft noise
    /// Level 2 : 
    /// Level 3 : Loud noise
    /// </summary>
    public int SoundLevel
    {
        get { return _soundLevel; }
        set
        {
            if (SoundImageState == null)
                SoundImageState = SoundImage.GetComponent<ImageState>();
            _soundLevel = RangePositivInt(value, SoundImageState.imageList.Count);
            SoundImageState.StateUpdate(_soundLevel);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI init
        IsInLight = false;
        BoyHeartLevel = 0;
        SoundLevel = 0;

        Player = GameObject.FindObjectOfType<PlayerManager>();
    }

    private int RangePositivInt(int value, int maxValue)
    {
        if ((uint)value >= (uint)maxValue) return 0;
        return value;
    }

    public void testUpdate()
    {
        IsInLight = _isInLight;
        BoyHeartLevel = _boyHeartLevel;
        SoundLevel = _soundLevel;
    }

    public void GameOver()
    {
        GameOverImage.SetActive(true);
        var anim = GameOverImage.GetComponent<ImagePopAnim>();
        anim.ScaleUpdate();
        var ST = this.gameObject.GetComponent<SceneTransition>();
        ST.scene = "GameOverScene";
        ST.LateLoadScene(2.0f);
    }

   

    public void GameClear()
    {
        //GameClearImage.SetActive(true);
        //var anim = GameClearImage.GetComponent<ImagePopAnim>();
        //anim.ScaleUpdate();
        var ST = this.gameObject.GetComponent<SceneTransition>();
        ST.scene = "GameClearScene";
        ST.LateLoadScene(1.0f);
    }
}
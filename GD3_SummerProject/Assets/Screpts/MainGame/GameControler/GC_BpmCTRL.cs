using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GC_BpmCTRL : MonoBehaviour
{
    // パブリック変数
    [Header("BPM")]
    public float bpm;   // テンポ

    // キャンバス
    [Header("キャンバス")]
    //public Text bpmText;    // BPM表記
    public Image beatImage;
    [SerializeField] Slider _beatSlider;

    // オーディオ関係
    [Header("オーディオ")]
    [SerializeField] AudioCTRL _audioCTRL;
    [SerializeField] AudioSource audioSource;   // オーディオソース
    [SerializeField] AudioClip[] audioClip;     // クリップ

    [Header("デバッグ")]
    [SerializeField] float _maxValue;
    [SerializeField] float _halfValue;
    [SerializeField] float _ping;
    [SerializeField] float _halfPing;

    // プライベート変数
    private float _timing = 0.0f;    // メトロノーム用
    private bool _metronome = false; // メトロノームシグナル
    private bool _metronomeFlap = false;
    private bool _step = false;       // １拍
    private bool _stepFlip = false;   // シグナル半分
    private bool _doSignal = false;   // シグナル送信用
    private bool _perfect = false;    // パーフェクト
    private bool _count = false;      // カウント用
    private float _nowImageSize = 0.6f;
    private float _minImageSize = 0.6f;
    private float _maxImageSize = 1.2f;
    private bool _pause = false;



    void Start()
    {
        // オーディオ初期化
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = _audioCTRL.defVolume;
        audioClip = new AudioClip[_audioCTRL.clips_BPM.Length];
        audioClip = _audioCTRL.clips_BPM;

        beatImage.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        BpmReset();
    }
    
    void Update()
    {
        if (_pause)
        {
            BpmReset();
            return;
        }
        Counter();
    }

    private void FixedUpdate()
    {
        if (_pause) { return; }
        _timing -= Time.deltaTime;
    }


    void Counter()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カウンター
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if (_timing <= _halfValue && _stepFlip == false)
        {
            audioSource.PlayOneShot(audioClip[0]);
            _nowImageSize = _maxImageSize;
            _step = true;
            _stepFlip = true;

            Debug.Log("half");
        }
        else
        {
            _step = false;
        }


        if (_timing <= _ping)
        {
            _doSignal = true;
        }

        if (_timing <= _halfPing)
        {
            _perfect = true;
        }

        if (_timing <= 0.0f && _metronomeFlap == false)
        {
            audioSource.PlayOneShot(audioClip[0]);
            _nowImageSize = _maxImageSize;
            _metronome = true;
            _metronomeFlap = true;

            Debug.Log("full");
        }
        else
        {
            _metronome = false;
        }

        if (_timing <= -_halfPing)
        {
            _perfect = false;
        }

        if (_timing <= -_ping)
        {
            _doSignal = false;
            _metronomeFlap = false;
            _stepFlip = false;

            _count = true;

            BpmReset();
        }
        else
        {
            _count = false;
        }

        
        ImageShrinking();

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    // BPM更新用
    float BpmReset()
    {
        _halfValue = float.Parse((60 / bpm).ToString("N4"));
        _maxValue = float.Parse((_halfValue * 2.0f).ToString("N4"));

        _beatSlider.maxValue = _maxValue;

        _ping = float.Parse((_maxValue * 0.14f).ToString("N4"));

        _halfPing = float.Parse((_ping * 0.2f).ToString("N4"));

        _beatSlider.minValue = -_ping;

        return _timing = _maxValue;
    }

    // シグナル送信関数
    public bool Metronome()
    {
        return _metronome;
    }

    public bool Step()
    {
        return _step;
    }

    public bool Signal()
    {
        return _doSignal;
    }

    public bool Perfect()
    {
        return _perfect;
    }

    public bool Count()
    {
        return _count;
    }

    public void ChangePause(bool flag)
    {
        _timing = 0;
        _pause = flag;
    }

    void ImageShrinking()
    {
        if (_nowImageSize > _minImageSize){ _nowImageSize -= Time.deltaTime * 3.0f; }
        else { _nowImageSize = _minImageSize; }

        beatImage.transform.localScale = new Vector3(_nowImageSize, _nowImageSize, 1.0f);


        _beatSlider.value = _timing;
    }
}

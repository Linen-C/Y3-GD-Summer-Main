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
    [SerializeField] AudioSource audioSource;   // オーディオソース
    [SerializeField] AudioClip[] audioClip;     // クリップ

    // プライベート変数
    private float timing = 0.0f;    // メトロノーム用
    private bool metronome = false; // メトロノームシグナル
    private bool metronomeFlap = false;
    private bool doSignal = false;  // シグナル送信用
    private float nowImageSize = 0.6f;
    private float minImageSize = 0.6f;
    private float maxImageSize = 1.2f;



    void Start()
    {
        beatImage.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        BpmReset();
    }
    
    void Update()
    {
        Counter();
    }


    void Counter()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カウンター
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (timing <= 0.1f)
        {
            doSignal = true;
            //beatImage.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
        }

        if (timing <= 0.0f && metronomeFlap == false)
        {
            //audioSource.PlayOneShot(audioClip[0]);
            nowImageSize = maxImageSize;
            metronome = true;
            metronomeFlap = true;
        }
        else
        {
            metronome = false;
            if (metronomeFlap == false)
            {
               //beatImage.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
            }
        }

        if (timing <= -0.2f)
        {
            doSignal = false;
            metronomeFlap = false;
            BpmReset();
        }

        ImageShrinking();
        timing -= Time.deltaTime;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    // BPM更新用
    float BpmReset()
    {
        //bpmText.text = "BPM:" + bpm;

        _beatSlider.maxValue = 60 / bpm;
        _beatSlider.minValue = -0.2f;

        return timing = 60 / bpm;
    }

    // シグナル送信関数
    public bool Metronome()
    {
        return metronome;
    }
    public bool Signal()
    {
        return doSignal;
    }


    void ImageShrinking()
    {
        if (nowImageSize > minImageSize){ nowImageSize -= Time.deltaTime * 4.0f; }
        else { nowImageSize = minImageSize; }

        _beatSlider.value = timing;

        beatImage.transform.localScale = new Vector3(nowImageSize, nowImageSize, 1.0f);
    }
}

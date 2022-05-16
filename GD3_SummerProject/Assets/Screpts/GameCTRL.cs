using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCTRL : MonoBehaviour
{
    // パブリック変数
    [Header("BPM")]
    public float bpm;   // テンポ

    // キャンバス
    [Header("キャンバス")]
    public Text bpmText;    // BPM表記
    public Image beatImage;

    // プライベート変数
    private float timing = 0.0f;    // メトロノーム用
    private bool metronome = false; // メトロノームシグナル
    private bool metronomeFlap = false;
    private bool doSignal = false;  // シグナル送信用

    // コンポーネント

    void Start()
    {
        BpmReset();
    }

    
    void Update()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カウンター
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (timing <= 0.2f)
        {
            doSignal = true;
            beatImage.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }

        if (timing <= 0.0f && metronomeFlap == false)
        {
            metronome = true;
            metronomeFlap = true;
        }
        else
        {
            metronome = false;
            if (metronomeFlap == false)
            {
            }
        }

        if (timing <= -0.2f)
        {
            doSignal = false;
            metronomeFlap = false;
            BpmReset();
            beatImage.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        timing -= Time.deltaTime;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    }

    float BpmReset()
    {
        bpmText.text = "BPM:" + bpm;
        return timing = 60 / bpm;
    }


    // シグナル送信関数

    public bool Metronome()
    {
        return metronome;
    }

    public bool SendSignal()
    {
        return doSignal;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCTRL : MonoBehaviour
{
    // 変数
    public float bpm;   // テンポ

    // キャンバス
    public Text bpmText;    // BPM表記
    public Text timingText; // 仮タイミング表記
    public Text metronomeText;  // メトロノームシグナル表示

    // 定数
    private float timing = 0.0f;    // メトロノーム用
    private bool metronome = false; // メトロノームシグナル
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
            timingText.text = "true";
        }
        
        if (timing <= 0.0f)
        {
            metronome = true;
            metronomeText.text = "PULSE";
        }
        else
        {
            metronome = false;
            metronomeText.text = "";
        }

        if (timing <= -0.2f)
        {
            doSignal = false;
            BpmReset();
            timingText.text = "false";
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

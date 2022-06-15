using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaCTRL : MonoBehaviour
{

    [Header("最大・現在ウェーブ数")]
    [SerializeField] int max_Wave;
    [SerializeField] public int now_Wave;
    [Header("BPMコントロール(自動取得)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [Header("敵管理")]
    [SerializeField] ArenaEnemyCTRL enemyCtrl;
    [Header("インターバル用")]
    [SerializeField] int maxCount;
    [SerializeField] int nowCount;
    [SerializeField] bool interval;
    [SerializeField] Text interval_text;


    void Start()
    {
        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = gameCtrlObj.GetComponent<GC_BpmCTRL>();

        enabled = false;
    }

    void Update()
    {
        if (enemyCtrl.DoEnemyAllDestroy())
        {
            Debug.Log("ウェーブ進行");
            Interval();
        }
    }


    public void Entry()
    {
        enabled = true;
    }

    void Interval()
    {
        if (nowCount == -1)
        {
            nowCount = 0;
            interval = true;
        }

        Debug.Log("インターバル");
        if (bpmCTRL.Metronome())
        {
            nowCount++;
            interval_text.text = nowCount.ToString();
        }

        if (nowCount > maxCount)
        {
            interval_text.text = " ";
            interval = false;
            WaveProgress();
        }
    }

    void WaveProgress()
    {
        now_Wave++;
        nowCount = -1;

        if (max_Wave >= now_Wave){ enemyCtrl.WavaStart(); }
        else { ArenaClear(); }
    }

    void ArenaClear()
    {
        Debug.Log("殲滅");
        enabled = false;
    }
}

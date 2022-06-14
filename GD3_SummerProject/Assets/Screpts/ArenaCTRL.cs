using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCTRL : MonoBehaviour
{

    [Header("最大・現在ウェーブ数")]
    [SerializeField] int max_Wave;
    [SerializeField] public int now_Wave;
    [Header("BPMコントロール(自動取得)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [Header("敵管理")]
    [SerializeField] ArenaEnemyCTRL enemyCtrl;


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
            WaveProgress();
        }
    }


    public void Entry()
    {
        enabled = true;
    }

    void Interval()
    {

    }

    void WaveProgress()
    {
        now_Wave++;

        if (max_Wave >= now_Wave){ enemyCtrl.WavaStart(); }
        else { ArenaClear(); }
    }

    void ArenaClear()
    {
        Debug.Log("殲滅");
        enabled = false;
    }
}

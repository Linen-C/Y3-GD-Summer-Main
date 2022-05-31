using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoAwake : MonoBehaviour
{
    [Header("エリア")]
    [SerializeField] AreaCTRL area;
    [Header("ゲームコントロール(自動取得)")]
    [SerializeField] GC_GameCTRL gameCtrl;
    [Header("ゲート(自動取得)")]
    [SerializeField] public GateCTRL[] gateCTRLs;
    [Header("エネミースポーン")]
    [SerializeField] EnemysSpawn enemySpawn;

    void Start()
    {
        gateCTRLs = area.gateCTRLs;

        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        gameCtrl = gameCtrlObj.GetComponent<GC_GameCTRL>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (area.DoClearFlag() == true) { return; }
        area.DoAwake();

        enemySpawn.DoAwake();

        for (int i = 0; i < gateCTRLs.Length; i++)
        {
            gateCTRLs[i].GateClose();
        }

        gameCtrl.areaCtrl = area;

        Destroy(gameObject);
    }
}

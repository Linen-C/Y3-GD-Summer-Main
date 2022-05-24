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
    [SerializeField] GateCTRL gateCTRL_U;
    [SerializeField] GateCTRL gateCTRL_D;
    [SerializeField] GateCTRL gateCTRL_L;
    [SerializeField] GateCTRL gateCTRL_R;
    [Header("エネミースポーン")]
    [SerializeField] EnemysSpawn enemySpawn;

    void Start()
    {
        if (area.gateCTRL_U != null) { gateCTRL_U = area.gateCTRL_U; }
        if (area.gateCTRL_D != null) { gateCTRL_D = area.gateCTRL_D; }
        if (area.gateCTRL_L != null) { gateCTRL_L = area.gateCTRL_L; }
        if (area.gateCTRL_R != null) { gateCTRL_R = area.gateCTRL_R; }

        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        gameCtrl = gameCtrlObj.GetComponent<GC_GameCTRL>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (area.DoClearFlag() == true) { return; }
        area.DoAwake();

        enemySpawn.DoAwake();

        if (gateCTRL_U != null) { gateCTRL_U.GateClose(); }
        if (gateCTRL_D != null) { gateCTRL_D.GateClose(); }
        if (gateCTRL_L != null) { gateCTRL_L.GateClose(); }
        if (gateCTRL_R != null) { gateCTRL_R.GateClose(); }

        gameCtrl.areaCtrl = area;

        Destroy(gameObject);
    }
}

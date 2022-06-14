using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEntry : MonoBehaviour
{
    [Header("アリーナ(自動取得)")]
    [SerializeField] ArenaCTRL arenaCtrl;
    [Header("ゲームコントロール(自動取得)")]
    [SerializeField] GC_GameCTRL gameCtrl;

    void Start()
    {
        arenaCtrl = transform.parent.GetComponent<ArenaCTRL>();

        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        gameCtrl = gameCtrlObj.GetComponent<GC_GameCTRL>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }

        // 起動
        arenaCtrl.Entry();

        //gameCtrl.areaCtrl = arenaCtrl;

        // 用済みなので消去
        Destroy(gameObject);
    }

}

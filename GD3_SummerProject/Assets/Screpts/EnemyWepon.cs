using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWepon : MonoBehaviour
{
    // パブリック変数
    public float defTime;   // 攻撃判定の発生時間

    // プライベート変数
    float attakingTime = 0.0f;  // 判定の発生時間

    // スクリプト
    public EnemyCTRL enemyCTRL;

    // コンポーネント
    BoxCollider2D coll;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }


    void Update()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 判定発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if (attakingTime > 0) { attakingTime -= Time.deltaTime; }
        else { coll.enabled = false; }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public void Attacking()
    {
        coll.enabled = true;
        attakingTime = defTime;
        Debug.Log("エネミー：判定発生");
    }
}

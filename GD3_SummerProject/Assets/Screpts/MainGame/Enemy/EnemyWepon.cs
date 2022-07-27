using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWepon : MonoBehaviour
{
    // パブリック変数
    [Header("パブリック変数")]
    public float defTime;   // 攻撃判定の発生時間

    // スクリプト
    [Header("スクリプト")]
    public EnemyCTRL enemyCTRL;
    public SpriteChanger spriteChanger;

    // プライベート変数
    public float attakingTime = 0.0f;  // 判定の発生時間
    float spriteAlpha = 0.0f;

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

        if (spriteAlpha >= 0.0f)
        {
            spriteChanger.ChangeTransparency(spriteAlpha);
            spriteAlpha -= Time.deltaTime * 2.0f;
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public void Attacking()
    {
        coll.enabled = true;
        attakingTime = defTime;
        spriteAlpha = 1.0f;
        //spriteChanger.ChangeTransparency(1.0f);
        // Debug.Log("エネミー：判定発生");
    }
}

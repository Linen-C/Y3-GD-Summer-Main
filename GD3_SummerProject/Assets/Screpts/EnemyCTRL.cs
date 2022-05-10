using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCTRL : MonoBehaviour
{
    // パブリック変数
    public float moveSpeed;        // 移動速度
    //public int needWeponCharge;   // クールダウン
    public GameObject Cursor;   // カーソル取得(多分これが一番早い)
    public GameObject Player;   // プレイヤー

    // プライベート変数
    //private int weponCharge = 1;      // クールダウン仮
    private Vector2 diff;   // プレイヤーの方向

    // コンポーネント
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        TracePlayer();
        CursorRot();
        Move();
    }


    void TracePlayer()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // プレイヤー方向の補足
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // 自分の位置
        Vector2 transPos = transform.position;

        // プレイヤー座標
        Vector2 playerPos = Player.transform.position;

        // ベクトルを計算
        diff = (playerPos - transPos).normalized;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    void CursorRot()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カーソル回転
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // 回転に代入
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // カーソルくんにパス
        Cursor.GetComponent<Transform>().rotation = curRot;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }
    void Move()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 移動
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // プレイヤーに向かって移動するだけのヤツだけど一応関数化
        body.velocity = new Vector2(diff.x * moveSpeed, diff.y * moveSpeed);

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("「いてっ！」");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCTRL : MonoBehaviour
{
    // パブリック変数
    public float moveSpeed;        // 移動速度
    public GameCTRL gameCTRL;      // ゲームコントローラー
    public int needWeponCharge;   // クールダウン仮
    public GameObject Cursor;   // カーソル取得(多分これが一番早い)

    // キャンパス
    public Text cooldownText;   // クールダウン表示用

    // プライベート変数
    private int weponCharge = 1;      // クールダウン仮
    private bool coolDownReset = false; // クールダウンのリセットフラグ

    private float dogeTimer = 0;

    // コンポーネント
    Rigidbody2D body;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        CursorRot();
        Attack();
        Move();
    }


    void Move()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 移動
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        body.velocity = new Vector2(
            Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);




        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("doge");
            dogeTimer = 0.2f;
        }

        if (dogeTimer > 0.0f)
        {
            body.AddForce(new Vector2(30.0f, 0f), ForceMode2D.Impulse);
            dogeTimer -= Time.deltaTime;
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }


    void CursorRot()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カーソル回転
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // 自分の位置
        Vector2 transPos = transform.position;
        //Debug.Log("tX" + transPos.x + "_" + "tY" + transPos.y);

        // スクリーン座標系のマウス座標をワールド座標系に修正
        Vector2 mouseRawPos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseRawPos);
        //Debug.Log("mX" + mouseWorldPos.x + "_"+ "mY" + mouseWorldPos.y);

        // ベクトルを計算
        Vector2 diff = (mouseWorldPos - transPos).normalized;

        // 回転に代入
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // カーソルくんにパス
        Cursor.GetComponent<Transform>().rotation = curRot;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }


    void Attack()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (Input.GetMouseButtonDown(0) && (weponCharge == needWeponCharge) && gameCTRL.SendSignal())
        {
            Debug.Log("ATTACK");
            UseWepon();
            coolDownReset = true;
        }

        if (gameCTRL.Metronome())
        {
            if (coolDownReset == true)
            {
                weponCharge = 1;
                coolDownReset = false;
            }
            else if (weponCharge < needWeponCharge)
            {
                weponCharge++;
            }
        }
        cooldownText.text = "COOL:" + weponCharge;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    void UseWepon()
    {



        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        /*
         *１）関数呼び出し
         *２）武器情報を受け取る
         *３）武器情報を元に攻撃を発生させる
         */

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCTRL : MonoBehaviour
{
    // 変数
    public float moveSpeed;        // 移動速度
    public GameCTRL gameCTRL;      // ゲームコントローラー
    public int needWeponCharge;   // クールダウン仮
    public GameObject Cursor;   // カーソル取得(多分これが一番早い)

    // キャンパス
    public Text cooldownText;   // クールダウン表示用

    // 定数
    private int weponCharge = 1;      // クールダウン仮
    private bool coolDownReset = false; // クールダウンのリセットフラグ

    // コンポーネント
    Rigidbody2D body;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 移動
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        body.velocity = new Vector2(
            Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("doge");
            //var rot = Cursor.transform.rotation;
            body.AddForce(transform.right * 50.0f, ForceMode2D.Impulse);
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //


        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (Input.GetMouseButtonDown(0) && (weponCharge == needWeponCharge) && gameCTRL.SendSignal())
        {
            Debug.Log("ATTACK");
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

}

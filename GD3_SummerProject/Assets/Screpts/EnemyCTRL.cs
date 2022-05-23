using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCTRL : MonoBehaviour
{
    // 変数
    [Header("ステータス")]
    [SerializeField] int helthPoint;  // 体力
    [Header("移動")]
    [SerializeField] float moveSpeed;  // 移動速度
    [Header("武器")]
    [SerializeField] int needWeponCharge;  // 必要クールダウン
    [Header("ノックバックと無敵時間")]
    [SerializeField] float knockBackPower;    // かかるノックバックの強さ
    [SerializeField] float defNonDamageTime;  // デフォルト無敵時間

    // スクリプト
    [Header("スクリプト")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [SerializeField] EnemyWepon ownWepon;  // 所持武器
    [SerializeField] AreaCTRL areaCTRL;    // エリアコンポーネント

    // ゲームオブジェクト
    [Header("ゲームオブジェクト")]
    [SerializeField] GameObject Cursor;    // カーソル取得(多分これが一番早い)
    [SerializeField] GameObject Player;    // プレイヤー
    [SerializeField] GameObject areaObj;   // エリアオブジェクト

    // プライベート変数
    private int weponCharge = 1;         // 現在クールダウン
    private bool coolDownReset = false;  // クールダウンのリセットフラグ
    private Vector2 diff;                // プレイヤーの方向
    private float knockBackCounter = 0;  // ノックバック時間カウンター
    private float NonDamageTime = 0;     // 無敵時間

    // コンポーネント
    Rigidbody2D body;

    void Start()
    {
        // 親エリアコンポーネントの取得
        areaObj = transform.parent.parent.gameObject;
        areaCTRL = areaObj.GetComponent<AreaCTRL>();

        // キレそう
        var bpmCtrl = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = bpmCtrl.GetComponent<GC_BpmCTRL>();

        // ２回も使いとうなかったわい…
        Player = GameObject.FindGameObjectWithTag("Player");

        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!areaCTRL.enabled)
        {
            body.velocity = new Vector2(0, 0);
            return;
        }

        Move();

        if (!IfIsAlive()) { return; }

        Attack();
        TracePlayer();
        CursorRot();

        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }
    }


    void Attack()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if ((weponCharge == needWeponCharge) && bpmCTRL.SendSignal() && coolDownReset == false)
        {
            // Debug.Log("ENEMY_ATTACK");
            ownWepon.Attacking();

            coolDownReset = true;
        }

        if (bpmCTRL.Metronome())
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

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
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

        if (weponCharge == needWeponCharge)
        {
            return;
        }

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

        if(knockBackCounter > 0.0f)
        {
            body.AddForce(new Vector2(
                -diff.x * knockBackPower,
                -diff.y * knockBackPower),
                ForceMode2D.Impulse);

            knockBackCounter -= Time.deltaTime;
        }
        else
        {
            body.velocity = new Vector2(diff.x * moveSpeed, diff.y * moveSpeed);
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    bool IfIsAlive()
    {
        if(helthPoint > 0) { return true; }
        else
        {
            Destroy(gameObject, defNonDamageTime + 0.1f);
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "PlayerAttack") && (NonDamageTime <= 0.0f))
        {
            helthPoint -= 1;
            NonDamageTime = defNonDamageTime;
            knockBackCounter = 0.1f;
        }
        //Debug.Log("「いてっ！」");
    }

}

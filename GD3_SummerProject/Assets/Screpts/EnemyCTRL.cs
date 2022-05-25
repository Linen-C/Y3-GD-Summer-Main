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
    [Header("スクリプト(マニュアル)")]
    [SerializeField] EnemyWepon ownWepon;  // 所持武器
    [Header("スクリプト(自動取得)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [SerializeField] AreaCTRL areaCTRL;    // エリアコンポーネント

    // ゲームオブジェクト
    [Header("ゲームオブジェクト(マニュアル)")]
    [SerializeField] GameObject Cursor;    // カーソル取得(多分これが一番早い)
    [Header("ゲームオブジェクト(自動取得)")]
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

    public enum State
    {
        Stop,
        Alive,
        Dead
    }
    public State state;


    void Start()
    {
        // コンポーネント取得
        body = GetComponent<Rigidbody2D>();

        // 親エリアコンポーネントの取得
        areaObj = transform.parent.parent.gameObject;
        areaCTRL = areaObj.GetComponent<AreaCTRL>();

        // キレそう
        var bpmCtrl = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = bpmCtrl.GetComponent<GC_BpmCTRL>();

        // ２回も使いとうなかったわい…
        Player = GameObject.FindGameObjectWithTag("Player");

        // ステート初期化
        state = State.Stop;
    }


    void Update()
    {
        // 動作判定
        if(areaCTRL.enabled == true){ state = State.Alive; }
        else { state = State.Stop; }

        // 停止
        if (state == State.Stop)
        {
            body.velocity = new Vector2(0, 0);
            return;
        }

        // 死亡判定
        if (state == State.Dead) { return; }
        IsDead();

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // 処理
        TracePlayer();  // プレイヤー追跡
        CursorRot();    // 旋回(カーソル回転)
        Attack();       // 攻撃
        Move();         // 移動

    }


    // プレイヤー追跡
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

    // 旋回(カーソル回転)
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

    // 攻撃
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

    // 移動
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

    // 死亡判定
    void IsDead()
    {
        if(helthPoint <= 0 && state != State.Dead)
        {
            Destroy(gameObject, defNonDamageTime + 0.1f);
            state = State.Dead;
        }
    }

    // 衝突判定
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

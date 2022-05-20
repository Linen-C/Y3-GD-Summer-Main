using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCTRL : MonoBehaviour
{
    // パブリック変数
    [Header("ステータス")]
    public int helthPoint;  // 体力
    [Header("移動")]
    public float moveSpeed;  // 移動速度
    [Header("武器")]
    public int needWeponCharge;  // 必要クールダウン
    [Header("ノックバックと無敵時間")]
    public float knockBackPower;    // かかるノックバックの強さ
    public float defNonDamageTime;  // デフォルト無敵時間
    
    //[Header("スポーン位置")]
    //[SerializeField] public Vector2 spawnPoint;  // スポーン位置

    // スクリプト
    [Header("スクリプト")]
    public GameCTRL gameCTRL;    // メトロノーム受け取り用
    public EnemyWepon ownWepon;  // 所持武器

    // ゲームオブジェクト
    [Header("ゲームオブジェクト")]
    public GameObject Cursor;    // カーソル取得(多分これが一番早い)
    public GameObject Player;    // プレイヤー

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
        // キレそう
        var gcCtrn = GameObject.FindGameObjectWithTag("GameController");
        gameCTRL = gcCtrn.GetComponent<GameCTRL>();

        // ２回も使いとうなかったわい…
        Player = GameObject.FindGameObjectWithTag("Player");

        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
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

        if ((weponCharge == needWeponCharge) && gameCTRL.SendSignal() && coolDownReset == false)
        {
            // Debug.Log("ENEMY_ATTACK");
            ownWepon.Attacking();

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

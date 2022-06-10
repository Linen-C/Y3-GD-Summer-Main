using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCTRL : MonoBehaviour
{
    [Header("遠距離攻撃用の仮組み")]
    [SerializeField] bool shootingType = false; // 遠距離攻撃タイプか
    [SerializeField] GameObject bullet; // 弾丸

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
    [SerializeField] GameObject cursor;    // カーソル取得(多分これが一番早い)
    [SerializeField] GameObject cursorImage;  // カーソルイメージ(TKIH)
    [SerializeField] GameObject flashObj;   // フラッシュ用
    [Header("ゲームオブジェクト(自動取得)")]
    [SerializeField] GameObject player;    // プレイヤー
    [SerializeField] GameObject areaObj;   // エリアオブジェクト

    // プライベート変数
    private int weponCharge = 1;         // 現在クールダウン
    private bool coolDownReset = false;  // クールダウンのリセットフラグ
    private Vector2 diff;                // プレイヤーの方向
    private float knockBackCounter = 0;  // ノックバック時間カウンター
    private float NonDamageTime = 0;     // 無敵時間

    // コンポーネント
    SpriteRenderer sprite;
    Rigidbody2D body;
    Animator anim;
    Animator flashAnim;
    Transform curTrans;

    enum State
    {
        Stop,
        Alive,
        Dead
    }
    State state;


    void Start()
    {
        if (!shootingType) { bullet = null; }

        // 親エリアコンポーネントの取得
        areaObj = transform.parent.parent.gameObject;
        areaCTRL = areaObj.GetComponent<AreaCTRL>();

        // キレそう
        var bpmCtrl = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = bpmCtrl.GetComponent<GC_BpmCTRL>();

        // ２回も使いとうなかったわい…
        player = GameObject.FindGameObjectWithTag("Player");

        // コンポーネント取得
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flashAnim = flashObj.GetComponent<Animator>();
        curTrans = cursor.GetComponent<Transform>();

        // ステート初期化
        state = State.Stop;
    }

    void Update()
    {
        // 停止判定
        if (!areaCTRL.enabled)
        {
            state = State.Stop;
            anim.SetBool("Moving", false);
        }
        else
        {
            state = State.Alive;
            anim.SetBool("Moving", true);
        }

        // 死亡判定
        IfIsAlive();

        // ステート判定
        if (state != State.Alive)
        {
            body.velocity = new Vector2(0, 0);
            return;
        }
        else { anim.SetBool("Alive", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // 処理
        TracePlayer();  // プレイヤー補足・追跡
        CursorRot();    // 旋回
        Attack();       // 攻撃
        //Move();         // 移動
    }

    void FixedUpdate()
    {
        // ステート判定
        if (state != State.Alive)
        {
            body.velocity = new Vector2(0, 0);
            return;
        }

        Move();
    }


    // プレイヤー補足・追跡
    void TracePlayer()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // プレイヤー方向の補足
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // 自分の位置
        Vector2 transPos = transform.position;

        // プレイヤー座標
        Vector2 playerPos = player.transform.position;

        // ベクトルを計算
        diff = (playerPos - transPos).normalized;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    // 旋回
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
        curTrans.rotation = curRot;
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        // スプライト反転
        if (curTrans.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }

    // 攻撃
    void Attack()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if ((weponCharge == needWeponCharge) && bpmCTRL.Signal() && coolDownReset == false)
        {
            // Debug.Log("ENEMY_ATTACK");

            if (shootingType)
            {
                Instantiate(
                    bullet,
                    new Vector3
                    (cursorImage.transform.position.x,
                    cursorImage.transform.position.y,
                    cursorImage.transform.position.z),
                    cursor.transform.rotation);
            }
            else { ownWepon.Attacking(); }

            anim.SetTrigger("Attack");

            coolDownReset = true;
        }

        if (bpmCTRL.Metronome())
        {
            if (coolDownReset == true)
            {
                weponCharge = 1;
                coolDownReset = false;
            }
            else if (weponCharge < needWeponCharge) { weponCharge++; }

            if (weponCharge == (needWeponCharge - 1))
            {
                anim.SetTrigger("Charge");
                flashAnim.SetTrigger("FlashTrigger");
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
    void IfIsAlive()
    {
        if(helthPoint <= 0)
        {
            Destroy(gameObject, defNonDamageTime + 0.1f);
        }
    }



    // ダメージを受ける
    public void TakeDamage(int damage,int knockback)
    {
        helthPoint -= damage;
        knockBackPower = knockback;

        NonDamageTime = defNonDamageTime;
        knockBackCounter = 0.1f;
        
        anim.SetTrigger("Damage");
    }
}

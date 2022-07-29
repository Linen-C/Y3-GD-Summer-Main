using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCTRL : MonoBehaviour
{
    [Header("遠距離攻撃用")]
    [SerializeField] bool shootingType = false; // 遠距離攻撃タイプか
    [SerializeField] GameObject bullet; // 弾丸

    [Header("チュートリアル用")]
    [SerializeField] bool _kockBackResist = false;
    [SerializeField] int _damageCap = 0;

    [Header("ステータス")]
    [SerializeField] int maxHelthPoint;      // 最大体力
    [SerializeField] int nowHelthPoint = 0;  // 現在体力
    [SerializeField] int maxStan = 100;      // 最大スタン値
    [SerializeField] int nowStan;            // 現在スタン値
    [SerializeField] int doStanCount;        // スタン回復までのテンポ数
    [SerializeField] int stanRecoverCount;   // スタン回復までのデフォルトテンポ数

    [Header("移動")]
    [SerializeField] float moveSpeed;  // 移動速度

    [Header("武器")]
    [SerializeField] int needWeaponCharge;  // 必要クールダウン

    [Header("ノックバックと無敵時間")]
    [SerializeField] float knockBackPower;    // かかるノックバックの強さ
    [SerializeField] float defNonDamageTime = 0.3f;  // デフォルト無敵時間

    [Header("テスト")]
    [SerializeField] EnemyMove _enemyMove;

    // スクリプト
    [Header("スクリプト(マニュアル)")]
    [SerializeField] EnemyWepon ownWepon;  // 所持武器
    [Header("スクリプト(自動取得)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [SerializeField] StageManager _stageManager;
    [SerializeField] PlayerAttack_B _playerAttack;

    // ゲームオブジェクト
    [Header("ゲームオブジェクト(マニュアル)")]
    [SerializeField] GameObject cursor;    // カーソル取得(多分これが一番早い)
    [SerializeField] GameObject cursorImage;  // カーソルイメージ(TKIH)
    [SerializeField] GameObject flashObj;   // フラッシュ用
    [Header("ゲームオブジェクト(自動取得)")]
    [SerializeField] GameObject player;    // プレイヤー
    [SerializeField] GameObject areaObj;   // エリアオブジェクト
    [SerializeField] Renderer _renderer;

    // 体力表示
    [Header("体力表示(マニュアル)")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider stanBar;
    [SerializeField] Image stanBarFill;

    // プライベート変数
    private int weaponCharge = 1;         // 現在クールダウン
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

    public enum State
    {
        Stop,
        Alive,
        Stan,
        Dead
    }
    State state;

    void Awake()
    {
        // コンポーネント取得
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flashAnim = flashObj.GetComponent<Animator>();
        curTrans = cursor.GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;
        stanBar.value = 1;

        if (!shootingType) { bullet = null; }

        // 親エリアコンポーネントの取得
        areaObj = transform.parent.parent.parent.parent.gameObject;
        _stageManager = areaObj.GetComponent<StageManager>();

        // キレそう
        var bpmCtrl = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = bpmCtrl.GetComponent<GC_BpmCTRL>();

        // ２回も使いとうなかったわい…
        player = GameObject.FindGameObjectWithTag("Player");
        _playerAttack = player.GetComponent<PlayerAttack_B>();

        // ステート初期化
        state = State.Stop;
    }

    void Update()
    {
        // 停止判定
        if (!_stageManager.enabled)
        {
            state = State.Stop;
            anim.SetBool("Moving", false);
        }
        else if (state != State.Dead)
        {
            state = State.Alive;
            anim.SetBool("Moving", true);
        }

        // 体力表示セット
        SetHP();

        // 死亡判定
        if (!IfIsAlive()) { return; }
        else { anim.SetBool("Alive", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // スタン処理
        if (!Stan()) { return; }


        // 処理
        TracePlayer();  // プレイヤー補足・追跡
        CursorRot();    // 旋回
        Attack();       // 攻撃

    }

    void FixedUpdate()
    {
        // ステート判定
        knockBackCounter = _enemyMove.KnockBack(knockBackCounter, body, diff, knockBackPower);

        if (!CanMove()) { return; }

        _enemyMove.Move(knockBackCounter, doStanCount, body, diff, moveSpeed);
    }



    // スタン
    bool Stan()
    {
        if (maxStan == -1) { return true; }

        if (nowStan >= maxStan)
        {
            doStanCount = stanRecoverCount;
            weaponCharge = 0;
            nowStan = 0;
        }

        if (doStanCount > 0)
        {
            if (bpmCTRL.Count()) { doStanCount--; }
            return false;
        }
        else if (nowStan > 0)
        {
            if (bpmCTRL.Count()) { nowStan -= 5; }
        }

        if (nowStan < 0) { nowStan = 0; }

        return true;
    }

    // 死亡判定
    bool IfIsAlive()
    {
        if (state == State.Dead) { return false; }

        if (nowHelthPoint <= 0 && state != State.Dead)
        {
            state = State.Dead;
            _playerAttack.GetCharge();
            Destroy(gameObject, defNonDamageTime + 0.1f);

            return false;
        }
        return true;
    }

    bool CanMove()
    {
        if (state == State.Dead ||
            state == State.Stop ||
            needWeaponCharge == -1 ||
            weaponCharge >= (needWeaponCharge - 1) )
        {
            body.velocity = new Vector2(0, 0);
            return false;
        }
        return true;
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

        if (weaponCharge >= (needWeaponCharge - 1) ||
            (needWeaponCharge == -1) ||
            ownWepon.attakingTime > 0)
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

        if ((weaponCharge == needWeaponCharge) && bpmCTRL.Signal() && coolDownReset == false)
        {
            // Debug.Log("ENEMY_ATTACK");

            if (shootingType)
            {
                if (_renderer.isVisible)
                {
                    Instantiate(
                    bullet,
                    new Vector3
                    (cursorImage.transform.position.x,
                    cursorImage.transform.position.y,
                    cursorImage.transform.position.z),
                    cursor.transform.rotation);
                }
            }
            else { ownWepon.Attacking(); }

            anim.SetTrigger("Attack");
            coolDownReset = true;
        }

        if (bpmCTRL.Metronome())
        {
            if (coolDownReset == true)
            {
                //state = State.Alive;
                weaponCharge = 1;
                coolDownReset = false;
            }
            else if (weaponCharge < needWeaponCharge) { weaponCharge++; }

            if (weaponCharge == (needWeaponCharge - 1))
            {
                anim.SetTrigger("Charge");
                flashAnim.SetTrigger("FlashTrigger");
            }
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    // 体力表示
    void SetHP()
    {
        hpSlider.value = (float)nowHelthPoint / (float)maxHelthPoint;
        if (doStanCount > 0)
        {
            stanBar.value = stanBar.maxValue;
            stanBarFill.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else
        {
            stanBar.value = (float)nowStan / (float)maxStan;
            stanBarFill.color = new Color(1.0f, 1.0f, 0.0f);
        }
    }


    // ダメージを受ける
    public void TakeDamage(int damage,int knockback, int stanPower,int typeNum)
    {
        if (NonDamageTime > 0) { return; }
        if (_damageCap >= damage) { damage = 0; }

        nowHelthPoint -= damage;
        knockBackPower = knockback;
        if (_kockBackResist) { knockBackPower = 0; }

        if (doStanCount <= 0) { nowStan += stanPower; }

        NonDamageTime = defNonDamageTime;
        knockBackCounter = 0.1f;
        
        anim.SetTrigger("Damage");
    }
}

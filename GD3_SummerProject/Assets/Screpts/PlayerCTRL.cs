using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerCTRL : MonoBehaviour
{
    // 変数
    [Header("ステータス")]
    [SerializeField] int helthPoint;  // 体力
    [Header("移動")]
    [SerializeField] float moveSpeed;  // 移動速度
    [SerializeField] float dashPower;  // ダッシュパワー
    [Header("遠距離攻撃")]
    [SerializeField] int needCharge;  // 遠距離攻撃に必要なチャージ
    [SerializeField] int nowCharge;   // 現在のチャージ
    [Header("ノックバックと無敵時間")]
    [SerializeField] float knockBackPower;    // かかるノックバックの強さ
    [SerializeField] float defNonDamageTime;  // デフォルト無敵時間

    // ゲームオブジェクト
    [Header("ゲームオブジェクト")]
    [SerializeField] GameObject cursor;  // カーソル取得(多分これが一番早い)
    [SerializeField] GameObject cursorImage;  // カーソルイメージ(TKIH)
    [SerializeField] GameObject bullet;  // 遠距離攻撃用の弾
    [SerializeField] GameObject flashObj;   // フラッシュ用

    // スクリプト
    [Header("スクリプト")]
    [SerializeField] GC_BpmCTRL bpmCTRL;      // BPMコントローラー
    [SerializeField] GC_jsonInput inputList;  // jsonファイルからの取得
    [SerializeField] PlayerWeapon trail;  // 攻撃用

    // キャンパス
    [Header("キャンバスUI")]
    [SerializeField] Text hpText;         // 体力表示用
    [SerializeField] Text cooldownText;   // クールダウン表示用
    [SerializeField] Text bulletText;     // 射撃チャージ

    // プライベート変数
    private int maxWeponCharge = 0;     // 必要クールダウン
    private int nowWeponCharge = 1;         // 現在クールダウン
    private int weponNo = 0;             // 所持している武器番号(0～1)
    private bool coolDownReset = false;  // クールダウンのリセットフラグ

    private int comboCountLeft = 0;
    private bool doComboMode = false;

    private float dogeTimer = 0;         // 回避用のタイマー
    private float knockBackCounter = 0;  // ノックバック時間カウンター
    private float NonDamageTime = 0;     // 無敵時間
    private Vector2 moveDir;    // 移動用ベクトル

    // コンポーネント
    SpriteRenderer sprite;
    Rigidbody2D body;
    Animator anim;
    Animator flashAnim;
    PlayerControls plCtrls;
    JsonData getList;

    public enum State
    {
        Stop,
        Alive,
        Dead
    }
    public State state;


    void Awake()
    {
        // コンポーネント取得
        TryGetComponent(out body);
        TryGetComponent(out sprite);
        TryGetComponent(out anim);
        //TryGetComponent(out flashAnim);

        flashAnim = flashObj.GetComponent<Animator>();
        plCtrls = new PlayerControls();
        
    }

    void Start()
    {
        // 武器初期化
        getList = inputList.SendList();
        nowCharge = 0;  // あえて0で初期化
        maxWeponCharge = trail.SwapWeapon(getList, 0);

        // UI系初期化
        UIUpdate();

        // ステート初期化
        state = State.Stop;
        anim.SetBool("Alive", true);

        // インプットシステム有効化
        plCtrls.Enable();
    }

    void Update()
    {
        // 移動入力の取得
        moveDir = plCtrls.Player.Move.ReadValue<Vector2>();


        // UI更新
        UIUpdate();

        // 死亡判定
        IsDead();

        // ステート判定
        if (state != State.Alive)
        {
            anim.SetBool("Moving", false);
            body.velocity = new Vector2(0,0);
            return;
        }
        else { anim.SetBool("Moving", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // 処理
        Rotation();   // 旋回系
        Attack();     // 攻撃
        Dash();       // ダッシュ入力
        SwapWepon();  // 武器交換
        Shooting();   // 遠距離攻撃

    }

    void FixedUpdate()
    {
        // ステート判定
        if (state != State.Alive) { return; }

        Move();
    }




    // UI更新
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void UIUpdate()
    {
        hpText.text = "HP：" + helthPoint.ToString();
        cooldownText.text = "Wepon : " + nowWeponCharge + "/" + maxWeponCharge;
        bulletText.text = "Shot : " + nowCharge + "/" + needCharge;
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //



    // 旋回
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // 旋回系
    void Rotation()
    {
        var padName = Gamepad.current;
        if (padName != null) { CursorRotStick(); }
        else { CursorRotMouse(); }

        if (cursor.transform.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }

    }

    // 旋回（キーボード・マウス）
    void CursorRotMouse()
    {
        // 自分の位置
        Vector2 transPos = transform.position;

        // スクリーン座標系のマウス座標をワールド座標系に修正
        var rawDir = plCtrls.Player.MouseDir.ReadValue<Vector2>();
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(rawDir);

        // ベクトルを計算
        Vector2 diff = (mouseDir - transPos).normalized;

        // 回転に代入
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // カーソルにパス
        cursor.GetComponent<Transform>().rotation = curRot;
    }

    // 旋回（スティック）
    void CursorRotStick()
    {
        // スティック方向取得
        var stickDir = plCtrls.Player.StickDir.ReadValue<Vector2>();

        // 入力が無ければ更新しない
        if (stickDir == new Vector2(0,0)) { return; }

        // ベクトルを計算
        float radian = Mathf.Atan2(stickDir.x, stickDir.y) * Mathf.Rad2Deg;

        // 回転に代入
        if (radian < 0) { radian += 360; }

        // カーソルにパス
        cursor.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, radian);
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //



    // 攻撃
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void Attack()
    {
        

        if (plCtrls.Player.Attack.triggered
            && bpmCTRL.Signal()
            && coolDownReset == false)
        {
            anim.SetTrigger("Attack");
            trail.Attacking(nowWeponCharge);
            coolDownReset = true;
        }

        if (trail.Combo())
        {
            nowWeponCharge = maxWeponCharge;
            doComboMode = true;
            coolDownReset = false;
            comboCountLeft = 2;
        }

        
        if (bpmCTRL.Metronome())
        {
            if (coolDownReset == true && doComboMode == false)
            {
                nowWeponCharge = 1;
                coolDownReset = false;
            }
            else if (nowWeponCharge < maxWeponCharge) { nowWeponCharge++; }

            if (comboCountLeft > 0)
            {
                comboCountLeft--;
            }
            if (comboCountLeft == 0 && doComboMode == true)
            {
                nowWeponCharge = 1;
                doComboMode = false;
                coolDownReset = true;
            }


            if (nowWeponCharge == (maxWeponCharge - 1))
            {
                anim.SetTrigger("Charge");
                flashAnim.SetTrigger("FlashTrigger");
            }
        }

    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //



    // ダッシュ入力
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void Dash()
    {
        if (plCtrls.Player.Dash.triggered && bpmCTRL.Signal()) { dogeTimer = 0.1f; }
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //



    // 移動
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // 移動系
    void Move()
    {
        // 移動
        if (knockBackCounter > 0.0f)
        {
            KnockBack();

            knockBackCounter -= Time.deltaTime;
        }
        else
        {
            body.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
        }

        // 回避
        if (dogeTimer > 0.0f)
        {
            body.AddForce(new Vector2(moveDir.x * dashPower, moveDir.y * dashPower), ForceMode2D.Impulse);

            dogeTimer -= Time.deltaTime;
        }
    }



    // 武器変更
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void SwapWepon()
    {
        var valueW = plCtrls.Player.WeponSwapWhile.ReadValue<float>();
        var valueUp = plCtrls.Player.WeponSwapButtonUp.triggered;
        var valueDwon = plCtrls.Player.WeponSwapButtonDown.triggered;

        if (valueW != 0 || (valueUp || valueDwon))
        {
            if (valueW > 0 || valueUp)
            {
                switch (weponNo)
                {
                    case 0:
                        weponNo = 1;
                        break;
                    case 1:
                        weponNo = 2;
                        break;
                    case 2:
                        weponNo = 0;
                        break;
                    default:
                        weponNo = 0;
                        break;
                }
            }

            if (valueW < 0 || valueDwon)
            {
                switch (weponNo)
                {
                    case 0:
                        weponNo = 2;
                        break;
                    case 1:
                        weponNo = 0;
                        break;
                    case 2:
                        weponNo = 1;
                        break;
                    default:
                        weponNo = 0;
                        break;
                }
            }

            maxWeponCharge = trail.SwapWeapon(getList, weponNo); // 必要クールダウン上書き   
            nowWeponCharge = 0;    // 現クールダウンを上書き
        }
    }



    // 遠距離攻撃
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // 遠距離攻撃チャージ
    public void GetCharge()
    {
        if (nowCharge < needCharge) { nowCharge += 1; }
    }

    // 遠距離攻撃
    void Shooting()
    {
        if (bpmCTRL.Signal())
        {
            if (nowCharge == needCharge && plCtrls.Player.Shot.triggered)
            {
                // 遠距離攻撃発生
                Debug.Log("遠距離攻撃");

                Instantiate(
                    bullet,
                    new Vector3
                    (cursorImage.transform.position.x,
                    cursorImage.transform.position.y,
                    cursorImage.transform.position.z),
                    cursor.transform.rotation);

                nowCharge = 0;
            }
        }
    }




    // 死亡判定
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void IsDead()
    {
        if (helthPoint <= 0)
        {
            hpText.text = "HP：" + helthPoint.ToString();
            state = State.Dead;
        }
    }




    // その他
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // ノックバック
    void KnockBack()
    {
        var diff = FetchNearObjectWithTag("Enemy");

        body.AddForce(new Vector2(
                -diff.x * knockBackPower,
                -diff.y * knockBackPower),
                ForceMode2D.Impulse);
    }

    // 最も近い敵オブジェクトの取得
    private Vector2 FetchNearObjectWithTag(string tagName)
    {
        GameObject nearEnemy = null;

        var targets = GameObject.FindGameObjectsWithTag(tagName);
        var minTargetDist = float.MaxValue;

        if (targets != null) { return new Vector2(0, 0); }

        foreach (var target in targets)
        {
            var targetDist = Vector2.Distance(
                transform.position,
                target.transform.position);

            if (!(targetDist < minTargetDist)) { continue; }

            minTargetDist = targetDist;
            nearEnemy = target.transform.gameObject;
        }


        // 自分の位置
        Vector2 transPos = transform.position;

        // 最も近い座標
        Vector2 enemyPos = nearEnemy.transform.position;

        // ベクトルを計算
        Vector2 diff = (enemyPos - transPos).normalized;

        return diff;
    }

    // 被ダメージ判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && (NonDamageTime <= 0.0f))
        {
            NonDamageTime = defNonDamageTime;
            knockBackCounter = 0.2f;
            helthPoint -= 1;
            anim.SetTrigger("Damage");
        }
    }


}

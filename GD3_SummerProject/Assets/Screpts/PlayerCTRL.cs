using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCTRL : MonoBehaviour
{
    // パブリック変数
    public int helthPoint;  // 体力
    public float moveSpeed;        // 移動速度
    public float dashPower; // ダッシュパワー

    public int needCharge;  // 遠距離攻撃に必要なチャージ
    public int nowCharge;   // 現在のチャージ

    // ゲームオブジェクト
    public GameObject Cursor;   // カーソル取得(多分これが一番早い)

    // スクリプト
    public GameCTRL gameCTRL;      // ゲームコントローラー
    public jsonInput inputList; // jsonファイルからの取得
    public PlayerWeapon ownWeapon;   // 攻撃のテスト用

    // キャンパス
    public Text hpText;         // 体力表示用
    public Text cooldownText;   // クールダウン表示用

    // プライベート変数
    private int needWeponCharge = 0;   // 必要クールダウン
    private int weponCharge = 1;      // 現在クールダウン
    private bool coolDownReset = false; // クールダウンのリセットフラグ
    private float dogeTimer = 0;    // 回避用のタイマー
    WeponList getList;

    private int weponNo = 0; // 所持している武器番号(0～1)

    // コンポーネント
    Rigidbody2D body;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        getList = inputList.SendList();
        nowCharge = 0;  // 仮で初期化

        Debug.Log("Pl_Get_WeponList：" + getList.weponList.Length);
        needWeponCharge = ownWeapon.SwapWeapon(getList, 0);
    }


    void Update()
    {

        var padName = Input.GetJoystickNames();
        if (padName.Length > 0) { CursorRotStick(); }
        else { CursorRotMouse(); }

        Attack();
        Move();
        SwapWepon();
        Shooting();

        hpText.text = "HP：" + helthPoint.ToString();
    }


    void Attack()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 攻撃・クールダウン
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.JoystickButton5))
            && (weponCharge == needWeponCharge) && gameCTRL.SendSignal())
        {
            Debug.Log("ATTACK");
            ownWeapon.Attacking();

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

    void Move()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 移動
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        body.velocity = new Vector2(
            Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 回避
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton4)) && gameCTRL.SendSignal())
        {
            Debug.Log("doge");
            dogeTimer = 0.1f;
        }

        if (dogeTimer > 0.0f)
        {
            body.AddForce(new Vector2(Input.GetAxis("Horizontal") * dashPower, Input.GetAxis("Vertical") * dashPower), ForceMode2D.Impulse);
            dogeTimer -= Time.deltaTime;
        }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    void CursorRotMouse()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カーソル回転（マウス）
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

    void CursorRotStick()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // カーソル回転（スティック）
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        var h = Input.GetAxisRaw("Horizontal2");
        var v = Input.GetAxisRaw("Vertical2");

        if (h == 0 && v == 0)
            return;

        float radian = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

        if (radian < 0){ radian += 360; }

        Cursor.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, radian);

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    void SwapWepon()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
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

            needWeponCharge = ownWeapon.SwapWeapon(getList, weponNo); // 必要クールダウン上書き   
            weponCharge = 0;    // 現クールダウンを上書き
        }
    }

    void Shooting()
    {
        if (gameCTRL.SendSignal())
        {
            if (nowCharge == needCharge && Input.GetMouseButton(1))
            {
                // 遠距離攻撃発生
                Debug.Log("遠距離攻撃");
                nowCharge = 0;
            }
        }
    }

    public void GetCharge()
    {
        if (nowCharge < needCharge) { nowCharge += 1; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            helthPoint -= 1;
        }
    }
}

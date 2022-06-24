using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerCTRL : MonoBehaviour
{
    // 変数
    [Header("ステータス")]
    [SerializeField] int maxHelthPoint;      // 最大体力
    [SerializeField] int nowHelthPoint = 0;  // 現在体力

    [Header("無敵時間")]
    [SerializeField] public float defNonDamageTime;  // デフォルト無敵時間

    // ゲームオブジェクト
    [Header("ゲームオブジェクト(マニュアル)")]
    [SerializeField] GameObject flashObj;   // フラッシュ用

    // スクリプト
    [Header("コンポーネント(マニュアル)")]
    [SerializeField] GC_GameCTRL gamectrl;  // いろいろ取ってくる用
    

    [Header("コンポーネント(オート)")]
    [SerializeField] GC_BpmCTRL _bpmCTRL;        // BPMコントローラー
    [SerializeField] PlayerWeapon playerWeapon;  // 攻撃用
    [SerializeField] EquipLoad equipLoad;       // 装備武器取得

    [Header("コンポーネント(マニュアル)")]
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] PlayerRotation _playerRotation;
    [SerializeField] PlayerAttack _playerAttack;

    // エンジン依存コンポーネント
    [Header("コンポーネント(オート)")]
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Rigidbody2D _body;
    [SerializeField] public Animator _anim;
    [SerializeField] public Animator _flashAnim;
    [SerializeField] PlayerControls _playerControls;

    public JsonData equipList; // 自動取得

    // キャンパス
    [Header("キャンバスUI(マニュアル)")]
    //[SerializeField] Text hpText;         // 体力表示用
    [SerializeField] Text text_Wepon;   // クールダウン表示用
    [SerializeField] Slider slider_Wepon;
    [SerializeField] Text text_Gun;     // 射撃チャージ
    [SerializeField] Slider slider_Gun;

    // 体力表示
    [Header("体力表示(マニュアル)")]
    [SerializeField] Slider hpSlider;

    // プライベート変数
    [Header("プライベート変数だったもの")]
    public int maxWeponCharge = 0;      // 必要クールダウン
    public int nowWeponCharge = 1;      // 現在クールダウン
    public int equipNo = 0;            // 所持している武器番号(0～2)
    public bool coolDownReset = false;  // クールダウンのリセットフラグ

    public int comboTimeLeft = 0;     // コンボ継続カウンター
    public bool doComboMode = false;  // コンボモード
    //private int comboCount = 0;        // コンボ回数カウンター

    public float knockBackCounter = 0;  // ノックバック時間カウンター
    private float NonDamageTime = 0;     // 無敵時間
    private Vector2 _moveDir;             // 移動用ベクトル

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
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        _bpmCTRL = gamectrl.transform.GetComponent<GC_BpmCTRL>();
        equipLoad = gamectrl.transform.GetComponent<EquipLoad>();

        _playerMove = GetComponent<PlayerMove>();

        TryGetComponent(out _body);
        TryGetComponent(out _sprite);
        TryGetComponent(out _anim);
        _flashAnim = flashObj.GetComponent<Animator>();
        _playerControls = new PlayerControls();
        
    }

    void Start()
    {
        // 体力(と表示)初期化
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;

        // 武器初期化
        equipList = equipLoad.GetList();
        maxWeponCharge = playerWeapon.SwapWeapon(equipList.weaponList, 0);
        _playerAttack.nowGunCharge = 0;

        // UI系初期化
        UIUpdate();

        // ステート初期化
        state = State.Stop;
        _anim.SetBool("Alive", true);

        // インプットシステム有効化
        _playerControls.Enable();
    }

    void Update()
    {
        // 移動入力の取得
        _moveDir = _playerControls.Player.Move.ReadValue<Vector2>();


        // UI更新
        UIUpdate();

        SetHP();

        // 死亡判定
        IsDead();

        // ステート判定
        if (state != State.Alive)
        {
            _anim.SetBool("Moving", false);
            _body.velocity = new Vector2(0,0);
            return;
        }
        else { _anim.SetBool("Moving", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // 処理
        _playerRotation.Rotation(_playerControls, _sprite);             // 旋回系
        _playerAttack.Attack(_playerControls, _bpmCTRL, playerWeapon);  // 攻撃
        _playerAttack.SwapWepon(_playerControls, playerWeapon);         // 武器交換
        _playerAttack.Shooting(_playerControls, _bpmCTRL);              // 遠距離攻撃
        _playerMove.Dash(_playerControls, _bpmCTRL);                    // ダッシュ入力

    }

    void FixedUpdate()
    {
        // ステート判定
        if (state != State.Alive) { return; }

        _playerMove.Move(_moveDir, _body);
        //Move();
    }



    // UI更新
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void UIUpdate()
    {
        //hpText.text = "HP：" + nowHelthPoint.ToString();
        text_Wepon.text = nowWeponCharge + " / " + maxWeponCharge;
        slider_Wepon.value = (float)nowWeponCharge / (float)maxWeponCharge;
        text_Gun.text = _playerAttack.nowGunCharge + " / " + _playerAttack.needGunCharge;
        slider_Gun.value = (float)_playerAttack.nowGunCharge / (float)_playerAttack.needGunCharge;
    }



    // 体力表示
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void SetHP()
    {
        hpSlider.value = (float)nowHelthPoint / (float)maxHelthPoint;
    }



    // 遠距離攻撃チャージ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void GetCharge()
    {
        if (_playerAttack.nowGunCharge < _playerAttack.needGunCharge)
        {
            _playerAttack.nowGunCharge += 1;
        }
    }



    // 死亡判定
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void IsDead()
    {
        if (nowHelthPoint <= 0)
        {
            //hpText.text = "HP：" + nowHelthPoint.ToString();
            state = State.Dead;
        }
    }



    // その他
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // 被ダメージ判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && (NonDamageTime <= 0.0f))
        {
            NonDamageTime = defNonDamageTime;
            knockBackCounter = 0.2f;
            nowHelthPoint -= 1;
            _anim.SetTrigger("Damage");
        }
    }

}

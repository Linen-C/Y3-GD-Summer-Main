using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

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
    [SerializeField] PlayerWeapon_B playerWeapon;  // 攻撃用
    [SerializeField] EquipLoad equipLoad;       // 装備武器取得

    [Header("コンポーネント(マニュアル)")]
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] PlayerRotation _playerRotation;
    [SerializeField] PlayerAttack_B _playerAttack;

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
    [SerializeField] Image image_DamagePanel;
    [SerializeField] float image_DamagePanel_defalpha = 1.0f;
    [SerializeField] float image_DamagePanel_nowalpha = 0.0f;
    [SerializeField] TextMeshProUGUI _comboText;
    [SerializeField] public TextMeshProUGUI _resultText;

    // 体力表示
    [Header("体力表示(マニュアル)")]
    [SerializeField] Slider hpSlider;

    // プライベート変数
    [Header("プライベート変数だったもの")]
    public int equipNo = 0;            // 所持している武器番号(0～2)

    public int comboTimeLeft = 0;     // コンボ継続カウンター
    public bool doComboMode = false;  // コンボモード
    public int comboCount = 0;        // コンボ回数カウンター

    public float knockBackCounter = 0;  // ノックバック時間カウンター
    float NonDamageTime = 0;     // 無敵時間
    Vector2 _moveDir;             // 移動用ベクトル

    public bool _orFaild = false;
    public int _orFaildCount = 0;


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
        playerWeapon = GetComponentInChildren<PlayerWeapon_B>();
        _bpmCTRL = gamectrl.transform.GetComponent<GC_BpmCTRL>();
        equipLoad = gamectrl.transform.GetComponent<EquipLoad>();

        _playerMove = GetComponent<PlayerMove>();

        TryGetComponent(out _body);
        TryGetComponent(out _sprite);
        TryGetComponent(out _anim);
        _flashAnim = flashObj.GetComponent<Animator>();
        _playerControls = new PlayerControls();


        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void Start()
    {
        // 体力(と表示)初期化
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;

        // 武器初期化
        equipList = equipLoad.GetList();
        //maxWeaponCharge = playerWeapon.SwapWeapon(equipList.weaponList, 0);
        playerWeapon.SwapWeapon(equipList.weaponList, 0);
        //_playerAttack.nowGunCharge = 0;

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
        _playerAttack.SwapingWeapon(_playerControls, playerWeapon);     // 武器交換
        _playerAttack.Shooting(_playerControls, _bpmCTRL);              // 遠距離攻撃
        _playerMove.Dash(_playerControls, _bpmCTRL);                    // ダッシュ入力

    }

    void FixedUpdate()
    {
        // ステート判定
        if (state != State.Alive) { return; }

        _playerMove.Move(_moveDir, _body);
    }



    // UI更新
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void UIUpdate()
    {
        // 体力表示
        hpSlider.value = (float)nowHelthPoint / (float)maxHelthPoint;

        // ダメージ表示
        if (image_DamagePanel_nowalpha > 0.0f) { image_DamagePanel_nowalpha -= Time.deltaTime; }
        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, image_DamagePanel_nowalpha);


        // コンボ表示
        if (comboCount == 0)
        {
            if (_comboText.alpha > 0.0f) { _comboText.alpha -= Time.deltaTime; }
            if (_resultText.alpha > 0.0f) { _resultText.alpha -= Time.deltaTime; }
        }
        else
        {
            _comboText.alpha = 1.0f;
            _comboText.text = "x" + comboCount;
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



    // 被ダメージ判定
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack" && (NonDamageTime <= 0.0f))
        {
            image_DamagePanel_nowalpha = image_DamagePanel_defalpha;
            NonDamageTime = defNonDamageTime;
            knockBackCounter = 0.2f;
            nowHelthPoint -= 1;
            _anim.SetTrigger("Damage");
        }
    }

}

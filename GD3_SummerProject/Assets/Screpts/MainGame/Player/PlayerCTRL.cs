using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCTRL : MonoBehaviour
{
    [Header("ステータス")]
    [SerializeField] int _maxHelthPoint;      // 最大体力
    [SerializeField] int _nowHelthPoint = 0;  // 現在体力
    [SerializeField] public float _defNonDamageTime = 0.5f;  // デフォルト無敵時間

    [Header("ゲームオブジェクト(マニュアル)")]
    [SerializeField] GameObject _flashObj;  // フラッシュ用
    [SerializeField] GameObject _gameCTRL;  // いろいろ取ってくる用

    [Header("コンポーネント(オート)")]
    // 外部
    [SerializeField] GC_BpmCTRL _bpmCTRL;             // BPMコントローラー
    [SerializeField] EquipLoad _equipLoad;            // 装備武器取得
    [SerializeField] PlayerWeapon_B _playerWeapon;    // 武器
    [SerializeField] public Animator _flashAnim;      // フラッシュアニメーション
    [SerializeField] PlayerControls _playerControls;  // コントローラー
    // 内部
    [SerializeField] PlayerMove _playerMove;          // 移動
    [SerializeField] PlayerRotation _playerRotation;  // 旋回
    [SerializeField] PlayerAttack_B _playerAttack;    // 攻撃
    [SerializeField] PlayerUI _playerUI;              // UI表示
    [SerializeField] SpriteRenderer _sprite;          // スプライト
    [SerializeField] Rigidbody2D _body;               // 2Rリジッドボディ
    [SerializeField] public Animator _anim;           // アニメーション

    [Header("その他変数")]
    public JsonData equipList;  // 武器リスト
    public int equipNo = 0;    // 所持している武器番号(0～2)

    public int comboTimeLeft = 0;     // コンボ継続カウンター
    public int comboCount = 0;        // コンボ回数カウンター
    public bool doComboMode = false;  // コンボモード

    public float knockBackCounter = 0;  // ノックバック時間カウンター
    float NonDamageTime = 0;            // 無敵時間
    Vector2 _moveDir;                   // 移動用ベクトル

    public int _orFaildCount = 0;  // ロック時間
    public bool _orFaild = false;  // ロック中かどうか
    
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
        // 外部
        _bpmCTRL = _gameCTRL.transform.GetComponent<GC_BpmCTRL>();
        _equipLoad = _gameCTRL.transform.GetComponent<EquipLoad>();
        _playerWeapon = GetComponentInChildren<PlayerWeapon_B>();
        _flashAnim = _flashObj.GetComponent<Animator>();
        _playerControls = new PlayerControls();
        // 内部
        TryGetComponent(out _playerMove);
        TryGetComponent(out _playerRotation);
        TryGetComponent(out _playerAttack);
        TryGetComponent(out _playerUI);
        TryGetComponent(out _sprite);
        TryGetComponent(out _body);
        TryGetComponent(out _anim);
    }

    void Start()
    {
        // 体力(と表示)初期化
        _nowHelthPoint = _maxHelthPoint;
        _playerUI.HelthSetStart();

        // 武器初期化
        equipList = _equipLoad.GetList();
        _playerWeapon.SwapWeapon(equipList.weaponList, 0);

        // UI系初期化
        _playerUI.UIUpdate(_nowHelthPoint, _maxHelthPoint, comboCount, _orFaild, _orFaildCount, _playerWeapon);

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
        _playerUI.UIUpdate(_nowHelthPoint, _maxHelthPoint, comboCount, _orFaild, _orFaildCount, _playerWeapon);

        // 死亡判定
        IsDead();

        // ステート判定
        if (state != State.Alive)
        {
            _anim.SetBool("Moving", false);
            _anim.SetBool("Alive", false);
            _body.velocity = new Vector2(0,0);
            return;
        }
        else { _anim.SetBool("Alive", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }


        // 旋回系
        _playerRotation.Rotation(_playerControls, _sprite);             // 旋回
        // 攻撃系
        _playerAttack.Attack(_playerControls, _bpmCTRL, _playerWeapon);  // 攻撃
        _playerAttack.SwapingWeapon(_playerControls, _playerWeapon);     // 武器交換
        _playerAttack.Shooting(_playerControls, _bpmCTRL);              // 遠距離攻撃
        // 移動系
        _playerMove.Dash(_playerControls, _bpmCTRL);                    // ダッシュ入力

    }

    void FixedUpdate()
    {
        // ステート判定
        if (state != State.Alive) { return; }

        // 移動反映
        _playerMove.Move(_moveDir, _body);
    }


    void IsDead()
    {
        if (_nowHelthPoint <= 0) { state = State.Dead; }
    }


    // 被ダメージ判定
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    // 攻撃
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "EnemyAttack" ||
            collision.gameObject.tag == "EnemyBullet") &&
            (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    // 接触
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    // ダメージ反映
    private void Damage()
    {
        _playerUI.DamagePanelAlphaSet();

        NonDamageTime = _defNonDamageTime;
        knockBackCounter = 0.2f;
        _nowHelthPoint -= 1;
        _anim.SetTrigger("Damage");
    }

}

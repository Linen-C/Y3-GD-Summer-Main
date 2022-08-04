using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCTRL : MonoBehaviour
{
    [Header("チュートリアル用")]
    [SerializeField] int _damageCap = 0;
    [SerializeField] bool _kockBackResist = false;

    [Header("得点")]
    [SerializeField] int _havePoint = 100;

    [Header("ステータス")]
    [SerializeField] int maxHelthPoint;      // 最大体力
    [SerializeField] int nowHelthPoint = 0;  // 現在体力
    [SerializeField] int maxStan = 100;      // 最大スタン値
    [SerializeField] int nowStan;            // 現在スタン値
    [SerializeField] int doStanCount;        // スタン回復までのテンポ数
    [SerializeField] int stanRecoverCount;   // スタン回復までのデフォルトテンポ数

    [Header("武器")]
    [SerializeField] public int _nowWeaponCharge = 1;  // 現在クールダウン
    [SerializeField] int _needWeaponCharge;            // 必要クールダウン

    [Header("遠距離攻撃の有無")]
    [SerializeField] bool shootingType = false; // 遠距離攻撃タイプか

    [Header("ノックバックと無敵時間")]
    [SerializeField] float knockBackPower;    // かかるノックバックの強さ
    [SerializeField] float defNonDamageTime = 0.2f;  // デフォルト無敵時間4

    [Header("ゲームオブジェクト(自動取得)")]
    [SerializeField] GameObject _player;    // プレイヤー

    [Header("コンポーネント")]
    // 外部
    [SerializeField] EnemyWepon _ownWepon;  // 所持武器
    [SerializeField] PlayerAttack_B _playerAttack;
    [SerializeField] StageManager _stageManager;  // 親のエリア
    [SerializeField] GC_GameCTRL _gameCTRL;       // ゲームコントローラー
    [SerializeField] GC_BpmCTRL _bpmCTRL;         // メトロノーム
    // 内部
    [SerializeField] EnemyMove _enemyMove;
    [SerializeField] EnemyRotation _enemyRotation;
    [SerializeField] EnemyAttack _enemyAttack;
    [SerializeField] Renderer _renderer;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Rigidbody2D _body;
    [SerializeField] Animator _anim;

    [Header("体力表示(マニュアル)")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider stanBar;
    [SerializeField] Image stanBarFill;

    [Header("オーディオ(マニュアル)")]
    [SerializeField] MainAudioCTRL _audioCTRL;
    [SerializeField] AudioSource audioSource;   // オーディオソース
    [SerializeField] AudioClip audioClip;

    // プライベート変数
    float knockBackCounter = 0;  // ノックバック時間カウンター
    float NonDamageTime = 0;     // 無敵時間
    Vector2 _diff;                // プレイヤーの方向

    public enum State
    {
        Stop,
        Alive,
        Stan,
        Dead
    }
    State _state;


    void Awake()
    {
        // コンポーネント取得
        // 外部
        _ownWepon = GetComponentInChildren<EnemyWepon>();
        // 内部
        TryGetComponent(out _enemyMove);
        TryGetComponent(out _enemyRotation);
        TryGetComponent(out _enemyAttack);
        TryGetComponent(out _renderer);
        TryGetComponent(out _sprite);
        TryGetComponent(out _body);
        TryGetComponent(out _anim);
    }

    void Start()
    {
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;
        stanBar.value = 1;

        if (!shootingType) { _enemyAttack.SetBulletNull(); }

        // 親エリアコンポーネントの取得
        var areaObj = transform.parent.parent.parent.parent.gameObject;
        _stageManager = areaObj.GetComponent<StageManager>();

        // タグ検索取得
        // _gameCTRLと_bpmCTRL
        _gameCTRL = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC_GameCTRL>();
        _bpmCTRL = _gameCTRL.GetComponent<GC_BpmCTRL>();
        // _playerと_PlayerAttack
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttack = _player.GetComponent<PlayerAttack_B>();

        // オーディオ初期化
        audioSource = GetComponent<AudioSource>();
        _audioCTRL = GameObject.FindGameObjectWithTag("AudioController").GetComponent<MainAudioCTRL>();
        audioSource.volume = _audioCTRL.nowVolume;
        audioClip = _audioCTRL.clips_Damage[0];

        // ステート初期化
        _state = State.Stop;
    }

    void Update()
    {
        // 停止判定
        if (!_stageManager.enabled)
        {
            _state = State.Stop;
            _anim.SetBool("Moving", false);
        }
        else if (_state != State.Dead)
        {
            _state = State.Alive;
            _anim.SetBool("Moving", true);
        }

        // 体力表示セット
        SetHP();

        // 死亡判定
        if (!IfIsAlive()) { return; }
        else { _anim.SetBool("Alive", true); }

        // 無敵時間
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // スタン処理
        if (!Stan()) { return; }


        // 処理
        _diff = _enemyRotation.TracePlayer(_player);  // プレイヤー補足・追跡
        _enemyRotation.CursorRot(_nowWeaponCharge, _needWeaponCharge, _ownWepon.attakingTime, _diff, _sprite);    // 旋回
        _enemyAttack.Attack(_needWeaponCharge, shootingType, _bpmCTRL, _renderer, _ownWepon, _anim);       // 攻撃
    }

    void FixedUpdate()
    {
        // ステート判定
        knockBackCounter = _enemyMove.KnockBack(knockBackCounter, _body, _diff, knockBackPower);

        if (!_enemyMove.CanMove(_state, _nowWeaponCharge, _needWeaponCharge,_body)) { return; }

        _enemyMove.Move(knockBackCounter, doStanCount, _body, _diff);
    }



    // スタン
    bool Stan()
    {
        if (maxStan == -1) { return true; }

        if (nowStan >= maxStan)
        {
            doStanCount = stanRecoverCount;
            _nowWeaponCharge = 0;
            nowStan = 0;
        }

        if (doStanCount > 0)
        {
            if (_bpmCTRL.Count()) { doStanCount--; }
            return false;
        }
        else if (nowStan > 0)
        {
            if (_bpmCTRL.Count()) { nowStan -= 5; }
        }

        if (nowStan < 0) { nowStan = 0; }

        return true;
    }

    // 死亡判定
    bool IfIsAlive()
    {
        if (_state == State.Dead) { return false; }

        if (nowHelthPoint <= 0 && _state != State.Dead)
        {
            _state = State.Dead;
            _playerAttack.GetCharge();
            Destroy(gameObject, defNonDamageTime + 0.1f);

            _gameCTRL.AddPoint(_havePoint);

            return false;
        }
        return true;
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

        audioSource.PlayOneShot(audioClip);
        _anim.SetTrigger("Damage");
    }
}

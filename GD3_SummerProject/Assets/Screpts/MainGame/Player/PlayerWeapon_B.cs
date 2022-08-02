using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon_B : MonoBehaviour
{
    // 変数
    [Header("変数")]
    [SerializeField] float defAttackingTime = 0.3f;   // 攻撃判定の基礎発生時間
    [SerializeField] public float nowAttakingTime = 0.0f;  // 判定の発生時間

    // スクリプト
    [Header("スクリプト(マニュアル)")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] PlayerAttack_B _playerAttack;
    [SerializeField] SpriteChanger _spriteChanger;
    [SerializeField] GC_GameCTRL _gameCTRL;

    [Header("パラメータ")]
    [SerializeField] int _damage = 0;
    [SerializeField] int _knockBack = 0;
    [SerializeField] int _stanPower = 0;
    [SerializeField] int _typeNum = 0;

    // プライベート変数
    float _spriteAlpha = 0.0f;
    [SerializeField] float _hitCoolDown = 0.0f;
    bool _comboFlag = false;
    bool _isPerfect = false;

    // コンポーネント
    BoxCollider2D coll;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;

        _spriteChanger.ChangeTransparency(_spriteAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        // 判定発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (nowAttakingTime >= 0)
        {
            nowAttakingTime -= Time.deltaTime;
        }
        else { coll.enabled = false; }

        if (_spriteAlpha > 0.0f)
        {
            _spriteChanger.ChangeTransparency(_spriteAlpha);
            _spriteAlpha -= Time.deltaTime * 2.0f;
        }

        if (_hitCoolDown > 0.0f)
        {
            _hitCoolDown -= Time.deltaTime;
        }
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    // 武器切り替え
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void SwapWeapon(WeaponList[] wepon, int no)
    {
        // バグってたら強制的に0を突っ込む
        if (no + 1 > wepon.Length) { no = 0; }


        // Tags
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // テキスト変更
        //weponNameText.text = wepon[no].name;

        // スプライト切り替えのためパス
        Sprite inImage = Resources.Load<Sprite>(wepon[no].trail);
        _spriteChanger.ChangeSprite(inImage, wepon[no].offset);

        // ここにアイコンも追加するかも
        // (Empty)

        // タイプ
        _typeNum = wepon[no].typeNum;


        // Status
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 最大ダメージ
        _damage = wepon[no].damage;

        // 最大ノックバック量
        _knockBack = wepon[no].maxknockback;

        // スタン値
        _stanPower = wepon[no].stanpower;


        // Sprites
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // スケール
        transform.localScale = new Vector3(
            wepon[no].wideth, wepon[no].height, 1.0f);

        // 座標
        transform.localPosition = new Vector3(
            0.0f, wepon[no].offset, 0.0f);


        // plAttack
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // ペナルティ長さ
        if (_typeNum == 2) { _playerAttack._defPenalty = 2; }
        else { _playerAttack._defPenalty = 1; }

        // UI更新
        _playerAttack._image_Wepon.sprite = Resources.Load<Sprite>(wepon[no].icon);
    }

    public bool Combo()
    {
        if (!_comboFlag) { return false; }

        _comboFlag = false;
        return true;
    }

    public void Attacking(bool timing)
    {
        coll.enabled = true;
        nowAttakingTime = defAttackingTime;

        if (timing) { _isPerfect = true; }

        _spriteAlpha = 1.0f;
    }

    public bool IsPerfect()
    {
        bool retBool;

        retBool = _isPerfect;
        _isPerfect = false;

        return retBool;
    }

    public int GetTypeNum()
    {
        return _typeNum;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (_hitCoolDown <= 0)
            {
                _playerCTRL.comboCount++;
                _comboFlag = true;
                _hitCoolDown = defAttackingTime;
            }

            int damage = _damage;
            if (_typeNum != 2)
            {
                damage += _playerCTRL.comboCount;
                if (_isPerfect) { damage += 2; }
            }

            int scoreValue = 5;
            if (_isPerfect) { scoreValue *= (_playerCTRL.comboCount + 2); }
            else { scoreValue *= _playerCTRL.comboCount; }
            _gameCTRL.AddPoint(scoreValue);

            collision.gameObject.GetComponent<EnemyCTRL>().TakeDamage(
                damage,
                _knockBack,
                _stanPower,
                _typeNum);
        }

        if (collision.tag == "EnemyBullet")
        {
            _playerCTRL.comboCount++;
            _comboFlag = true;
            _hitCoolDown = defAttackingTime;

            _gameCTRL.AddPoint(50);
        }
    }
}

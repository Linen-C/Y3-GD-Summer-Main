using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    // 変数
    [Header("変数")]
    [SerializeField] float defAttackingTime = 0.3f;   // 攻撃判定の基礎発生時間
    [SerializeField] public float nowAttakingTime = 0.0f;  // 判定の発生時間

    // スクリプト
    [Header("スクリプト")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] SpriteChanger _spriteChanger;

    // キャンパス
    //[Header("キャンバス")]
    //[SerializeField] Text weponNameText;  // 武器名表示用
    [Header("UI用(マニュアル)")]
    [SerializeField] Image image_Wepon;
    //[SerializeField] Image image_Gun;

    // プライベート変数
    float spriteAlpha = 0.0f;
    float chargeCool = 0.0f;

    [Header("パラメータ")]
    [SerializeField] int defDamage = 1;      // 通常ダメージ
    [SerializeField] int maxDamage = 0;      // 最大ダメージ
    [SerializeField] int defKnockBack = 0;   // ノックバックパワー
    [SerializeField] int maxKnockBack = 0;   // ノックバックパワー
    [SerializeField] int maxCharge = 0;      // 必要最大チャージ
    [SerializeField] int maxStanPower = 0;      // スタン値

    int nowDamage = 0;
    int nowKockBack = 0;
    int nowStanPower = 0;
    bool comboFlag = false;

    // コンポーネント
    BoxCollider2D coll;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;

        _spriteChanger.ChangeTransparency(spriteAlpha);
    }

    void Update()
    {
        // 判定発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (nowAttakingTime >= 0)
        {
            nowAttakingTime -= Time.deltaTime;
        }
        else{ coll.enabled = false; }

        if (spriteAlpha > 0.0f)
        {
            _spriteChanger.ChangeTransparency(spriteAlpha);
            spriteAlpha -= Time.deltaTime * 2.0f;
        }

        if (chargeCool >= 0.0f)
        {
            chargeCool -= Time.deltaTime;
        }
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }



    // 武器切り替え
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public int SwapWeapon(WeaponList[] wepon,int no)
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


        // Status
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 最大ダメージ
        maxDamage = wepon[no].damage;

        // 基礎ノックバック量
        defKnockBack = wepon[no].defknockback;

        // 最大ノックバック量
        maxKnockBack = wepon[no].maxknockback;

        // 最大チャージ量
        maxCharge = wepon[no].maxcharge;

        // スタン値
        maxStanPower = wepon[no].stanpower;


        // Sprites
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // スケール
        transform.localScale = new Vector3(
            wepon[no].wideth, wepon[no].height, 1.0f);

        // 座標
        transform.localPosition = new Vector3(
            0.0f, wepon[no].offset, 0.0f);


        // UI
        image_Wepon.sprite = Resources.Load<Sprite>(wepon[no].icon);


        // プレイヤーに必要クールダウンを渡してリターン
        return wepon[no].maxcharge;
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //



    // 攻撃発生
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Attacking(int nowCharge)
    {
        if (maxCharge == nowCharge)
        {
            nowDamage = maxDamage;
            nowKockBack = maxKnockBack;
            nowStanPower = maxStanPower;
        }
        else
        {
            nowDamage = defDamage;
            nowKockBack = defKnockBack;
            nowStanPower = 0;
        }

        coll.enabled = true;
        nowAttakingTime = defAttackingTime;

        spriteAlpha = 1.0f;
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //


    public bool Combo()
    {
        if (!comboFlag) { return false; }

        comboFlag = false;
        return true;
    }


    // 攻撃の命中判定
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyCTRL>().TakeDamage(nowDamage, nowKockBack, nowStanPower);
            
            if (nowDamage == maxDamage)
            {
                if (chargeCool <= 0)
                {
                    _playerCTRL.GetCharge();
                    chargeCool = defAttackingTime;
                    _playerCTRL.comboCount++;
                }
                comboFlag = true;
            }
        }
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
}

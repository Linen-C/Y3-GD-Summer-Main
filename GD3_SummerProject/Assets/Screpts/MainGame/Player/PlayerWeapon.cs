using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    // 変数
    [Header("変数")]
    [SerializeField] float defTime;   // 攻撃判定の発生時間
    [SerializeField] public float attakingTime = 0.0f;  // 判定の発生時間

    // スクリプト
    [Header("スクリプト")]
    [SerializeField] PlayerCTRL playerctrl;
    [SerializeField] SpriteChanger spriteChanger;

    // キャンパス
    [Header("キャンバス")]
    [SerializeField] Text weponNameText;  // 武器名表示用

    // プライベート変数
    float spriteAlpha = 0.0f;
    float chargeCool = 0.0f;

    [SerializeField] int defDamage = 1;      // 通常ダメージ
    [SerializeField] int maxDamage = 0;      // 最大ダメージ
    [SerializeField] int defKnockBack = 0;   // ノックバックパワー
    [SerializeField] int maxKnockBack = 0;   // ノックバックパワー
    [SerializeField] int maxCharge = 0;      // 必要最大チャージ

    int nowDamage = 0;
    int nowKockBack = 0;
    bool comboFlag = false;

    // コンポーネント
    BoxCollider2D coll;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;

        spriteChanger.ChangeTransparency(spriteAlpha);
    }

    void Update()
    {
        // 判定発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        if (attakingTime >= 0)
        {
            attakingTime -= Time.deltaTime;
        }
        else{ coll.enabled = false; }

        if (spriteAlpha > 0.0f)
        {
            spriteChanger.ChangeTransparency(spriteAlpha);
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
    public int SwapWeapon(JsonData wepon,int no)
    {
        // バグってたら強制的に0を突っ込む
        if (no + 1 > wepon.weaponList.Length) { no = 0; }


        // 最大ダメージセット
        maxDamage = wepon.weaponList[no].maxcharge;

        // 基礎ノックバック量セット
        defKnockBack = wepon.weaponList[no].defknockback;

        // 最大ノックバック量セット
        maxKnockBack = wepon.weaponList[no].maxknockback;

        // 最大チャージ量セット
        maxCharge = wepon.weaponList[no].maxcharge;


        // 座標セット
        transform.localPosition = new Vector3(
            0.0f, wepon.weaponList[no].offset, 0.0f);

        // スケールセット
        transform.localScale = new Vector3(
            wepon.weaponList[no].wideth, wepon.weaponList[no].height, 1.0f);

        // スプライト切り替えのためパス
        Sprite inImage = Resources.Load<Sprite>(wepon.weaponList[no].image.ToString());
        spriteChanger.ChangeSprite(inImage, wepon.weaponList[no].offset);


        // テキスト変更
        weponNameText.text = wepon.weaponList[no].name;

        // プレイヤーに必要クールダウンを渡してリターン
        return wepon.weaponList[no].maxcharge;
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
        }
        else
        {
            nowDamage = defDamage;
            nowKockBack = defKnockBack;
        }

        coll.enabled = true;
        attakingTime = defTime;

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
        if(collision.tag == "Enemy" && chargeCool <= 0)
        {
            collision.gameObject.GetComponent<EnemyCTRL>().TakeDamage(nowDamage, nowKockBack);
            playerctrl.GetCharge();
            if (nowDamage == maxDamage) { comboFlag = true; }
        }
    }

    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
}

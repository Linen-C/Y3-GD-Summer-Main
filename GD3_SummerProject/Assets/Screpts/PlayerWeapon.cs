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
    [SerializeField] float defChargeCool;

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

    // コンポーネント
    BoxCollider2D coll;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 判定発生
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        
        if (attakingTime >= 0){ attakingTime -= Time.deltaTime; }
        else{ coll.enabled = false; }

        if (spriteAlpha >= 0.0f)
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

    public int SwapWeapon(WeponList wepon,int no)
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 武器切り替え
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        //Debug.Log("武器変更");

        // バグってた時用の処理
        if (no + 1 > wepon.weponList.Length)
        {
            // 座標セット
            transform.localPosition = new Vector3(
                0.0f, wepon.weponList[0].offset, 0.0f);

            // スケールセット
            transform.localScale = new Vector3(
                wepon.weponList[0].wideth, wepon.weponList[0].height, 1.0f);

            // スプライト切り替えのためパス
            Sprite defImage = Resources.Load<Sprite>(wepon.weponList[0].image.ToString());
            spriteChanger.ChangeSprite(defImage, wepon.weponList[0].offset);

            // テキスト変更
            weponNameText.text = wepon.weponList[0].name;

            // プレイヤーに必要クールダウンを渡してリターン
            return wepon.weponList[0].cool;
        }


        // 座標セット
        transform.localPosition = new Vector3(
            0.0f, wepon.weponList[no].offset, 0.0f);

        // スケールセット
        transform.localScale = new Vector3(
            wepon.weponList[no].wideth, wepon.weponList[no].height, 1.0f);

        // スプライト切り替えのためパス
        Sprite inImage = Resources.Load<Sprite>(wepon.weponList[no].image.ToString());
        spriteChanger.ChangeSprite(inImage, wepon.weponList[no].offset);

        // テキスト変更
        weponNameText.text = wepon.weponList[no].name;

        // プレイヤーに必要クールダウンを渡してリターン
        return wepon.weponList[no].cool;

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public void Attacking()
    {
        coll.enabled = true;
        attakingTime = defTime;

        spriteAlpha = 1.0f;

        //spriteChanger.ChangeTransparency(1.0f);
        //Debug.Log("プレイヤー：判定発生");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && chargeCool <= 0)
        {
            playerctrl.GetCharge();
        }
        //Debug.Log("プレイヤー：命中");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    // パブリック変数
    public float defTime;   // 攻撃判定の発生時間

    // キャンパス
    public Text weponNameText;  // 武器名表示用

    // プライベート変数
    float attakingTime = 0.0f;  // 判定の発生時間

    // スクリプト
    public PlayerCTRL playerctrl;

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
        
        if (attakingTime > 0) { attakingTime -= Time.deltaTime; }
        else { coll.enabled = false; }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public int SwapWeapon(WeponList wepon,int no)
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 武器切り替え
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        Debug.Log("Swap_Wepon");

        if (no + 1 > wepon.weponList.Length)
        {
            transform.localPosition = new Vector3(
                0.0f, wepon.weponList[0].offset, 0.0f);

            transform.localScale = new Vector3(
                wepon.weponList[0].wideth, wepon.weponList[0].height, 1.0f);

            weponNameText.text = wepon.weponList[0].name;

            return wepon.weponList[0].cool;
        }


        transform.localPosition = new Vector3(
            0.0f, wepon.weponList[no].offset, 0.0f);

        transform.localScale = new Vector3(
            wepon.weponList[no].wideth, wepon.weponList[no].height, 1.0f);

        weponNameText.text = wepon.weponList[no].name;

        return wepon.weponList[no].cool;

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public void Attacking()
    {
        coll.enabled = true;
        attakingTime = defTime;
        Debug.Log("プレイヤー：判定発生");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerctrl.GetCharge();
        Debug.Log("プレイヤー：命中");
    }
}

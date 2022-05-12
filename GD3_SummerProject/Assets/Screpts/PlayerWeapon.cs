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
    float time = 0.0f;

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
        
        if (time > 0) { time -= Time.deltaTime; }
        else { coll.enabled = false; }

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public int SwapoWeapon(WeponList wepon,int no)
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 武器切り替え
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
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
        time = defTime;
        Debug.Log("判定発生");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerctrl.GetCharge();
        Debug.Log("命中");
    }
}

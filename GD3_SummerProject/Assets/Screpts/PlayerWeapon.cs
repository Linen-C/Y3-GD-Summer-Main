using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // パブリック変数
    public float defTime;   // 攻撃判定の発生時間

    // プライベート変数
    float time = 0.0f;

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

    public int SwapoWeapon()
    {
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
        // 武器切り替え
        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

        /*
        if (Input.GetMouseButtonDown(1))
        {
            transform.localPosition = new Vector3(0.0f, 3.0f, 0.0f);
            transform.localScale = new Vector3(1.0f, 3.0f, 1.0f);
        }
        */

        transform.localPosition = new Vector3(0.0f, 3.0f, 0.0f);
        transform.localScale = new Vector3(1.0f, 3.0f, 1.0f);

        return 2;

        // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    }

    public void Attacking()
    {
        coll.enabled = true;
        time = defTime;
        Debug.Log("判定発生");
    }
}

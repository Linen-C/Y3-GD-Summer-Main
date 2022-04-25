using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCTRL : MonoBehaviour
{
    // 変数
    public float moveSpeed;     // 移動速度
    public GameCTRL gameCTRL;   // ゲームコントローラー
    public int defWeponCooldown;   // クールダウン仮

    // キャンパス
    public Text cooldownText;   // クールダウン表示用

    // 定数
    private float cashTime = 0; // 先行入力用のキャッシュタイム
    private int weponCooldown = 0;  // クールダウン仮

    // コンポーネント
    Rigidbody2D body;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        weponCooldown = defWeponCooldown;
    }


    void Update()
    {
        // 移動
        body.velocity = new Vector2(
            Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);


        if (gameCTRL.Metronome() && weponCooldown > 0)
        {
            weponCooldown--;
        }
        cooldownText.text = "COOL:" + weponCooldown;


        if (gameCTRL.SendSignal())
        {
            if (Input.GetMouseButtonDown(0))
            {
                cashTime = 0.2f;
            }
        }

        if (weponCooldown == 0 && gameCTRL.Metronome())
        {
            if (cashTime >= 0.0f)
            {
                Debug.Log("ATTACK");
                weponCooldown = defWeponCooldown;
            }
        }

        if (cashTime >= 0.0f) { cashTime -= Time.deltaTime; }
    }
}

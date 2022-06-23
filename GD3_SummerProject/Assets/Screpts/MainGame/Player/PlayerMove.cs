using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("PlayerCTRL")]
    [SerializeField] PlayerCTRL _plCTRL;

    [Header("移動")]
    [SerializeField] float moveSpeed;  // 移動速度
    [SerializeField] float dashPower;  // ダッシュパワー

    [Header("ノックバック強さ")]
    [SerializeField] float knockBackPower;    // かかるノックバックの強さ

    private float dogeTimer = 0;         // 回避用のタイマー


    public void Move(Vector2 moveDir, Rigidbody2D body)
    {
        // 移動
        if (_plCTRL.knockBackCounter > 0.0f)
        {
            KnockBack(body);

            _plCTRL.knockBackCounter -= Time.deltaTime;
        }
        else
        {
            body.velocity = new Vector2(
                moveDir.x * moveSpeed,
                moveDir.y * moveSpeed);
        }

        // 回避
        if (dogeTimer > 0.0f)
        {
            body.AddForce(new Vector2(
                moveDir.x * dashPower,
                moveDir.y * dashPower),
                ForceMode2D.Impulse);

            dogeTimer -= Time.deltaTime;
        }
    }

    void KnockBack(Rigidbody2D body)
    {
        var diff = FetchNearObjectWithTag("Enemy");

        body.AddForce(new Vector2(
                -diff.x * knockBackPower,
                -diff.y * knockBackPower),
                ForceMode2D.Impulse);
    }

    Vector2 FetchNearObjectWithTag(string tagName)
    {
        GameObject nearEnemy = null;

        var targets = GameObject.FindGameObjectsWithTag(tagName);
        var minTargetDist = float.MaxValue;

        if (targets == null) { return new Vector2(0, 0); }

        foreach (var target in targets)
        {
            var targetDist = Vector2.Distance(
                transform.position,
                target.transform.position);

            if (!(targetDist < minTargetDist)) { continue; }

            minTargetDist = targetDist;
            nearEnemy = target.transform.gameObject;
        }


        // 自分の位置
        Vector2 transPos = transform.position;

        // 最も近い座標
        Vector2 enemyPos = nearEnemy.transform.position;

        // ベクトルを計算
        Vector2 diff = (enemyPos - transPos).normalized;

        return diff;
    }


    public void Dash(PlayerControls playerControls, GC_BpmCTRL bpmCTRL)
    {
        if (playerControls.Player.Dash.triggered &&
            bpmCTRL.Signal())
        {
            dogeTimer = 0.1f;
        }
    }
}

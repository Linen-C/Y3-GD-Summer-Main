using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] GameObject _cursor;


    public Vector2 TracePlayer(GameObject player)
    {
        // プレイヤー方向の補足
        // 自分の位置
        Vector2 transPos = transform.position;

        // プレイヤー座標
        Vector2 playerPos = player.transform.position;

        // ベクトルを計算
        return (playerPos - transPos).normalized;
    }

    // 旋回
    public void CursorRot(int nowCharge, int needCharge, float attakingTime, Vector2 diff, SpriteRenderer sprite)
    {
        // カーソル回転
        if (nowCharge >= (needCharge - 1) ||
            (needCharge == -1) ||
            attakingTime > 0)
        {
            return;
        }

        // 回転に代入
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // カーソルくんにパス
        _cursor.transform.rotation = curRot;

        // スプライト反転
        if (_cursor.transform.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }
}

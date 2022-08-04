using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [Header("カーソルオブジェクト")]
    [SerializeField] GameObject cursor;       // カーソル取得(多分これが一番早い)


    public void Rotation(PlayerControls playerControls, SpriteRenderer sprite)
    {
        var padName = Gamepad.current;
        if (padName != null) { CursorRotStick(playerControls); }
        else { CursorRotMouse(playerControls); }

        if (cursor.transform.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }


    // 旋回（キーボード・マウス）
    void CursorRotMouse(PlayerControls playerControls)
    {
        // 自分の位置
        Vector2 transPos = transform.position;

        // スクリーン座標系のマウス座標をワールド座標系に修正
        var rawDir = playerControls.Player.MouseDir.ReadValue<Vector2>();
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(rawDir);

        // ベクトルを計算
        Vector2 diff = (mouseDir - transPos).normalized;

        // 回転に代入
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // カーソルにパス
        cursor.transform.rotation = curRot;
    }


    // 旋回（スティック）
    void CursorRotStick(PlayerControls playerControls)
    {
        // スティック方向取得
        var stickDir = playerControls.Player.StickDir.ReadValue<Vector2>();

        // 入力が無ければ更新しない
        if (stickDir == new Vector2(0, 0)) { return; }

        // ベクトルを計算
        float radian = Mathf.Atan2(stickDir.x, stickDir.y) * Mathf.Rad2Deg;

        // 回転に代入
        if (radian < 0) { radian += 360; }

        // カーソルにパス
        cursor.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, radian);
    }
}

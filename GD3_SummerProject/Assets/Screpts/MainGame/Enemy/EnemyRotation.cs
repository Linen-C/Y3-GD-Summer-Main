using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] GameObject _cursor;


    public Vector2 TracePlayer(GameObject player)
    {
        // �v���C���[�����̕⑫
        // �����̈ʒu
        Vector2 transPos = transform.position;

        // �v���C���[���W
        Vector2 playerPos = player.transform.position;

        // �x�N�g�����v�Z
        return (playerPos - transPos).normalized;
    }

    // ����
    public void CursorRot(int nowCharge, int needCharge, float attakingTime, Vector2 diff, SpriteRenderer sprite)
    {
        // �J�[�\����]
        if (nowCharge >= (needCharge - 1) ||
            (needCharge == -1) ||
            attakingTime > 0)
        {
            return;
        }

        // ��]�ɑ��
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // �J�[�\������Ƀp�X
        _cursor.transform.rotation = curRot;

        // �X�v���C�g���]
        if (_cursor.transform.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }
}

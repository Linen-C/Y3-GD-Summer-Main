using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    [Header("�J�[�\���I�u�W�F�N�g")]
    [SerializeField] GameObject cursor;       // �J�[�\���擾(�������ꂪ��ԑ���)


    public void Rotation(PlayerControls playerControls, SpriteRenderer sprite)
    {
        var padName = Gamepad.current;
        if (padName != null) { CursorRotStick(playerControls); }
        else { CursorRotMouse(playerControls); }

        if (cursor.transform.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }


    // ����i�L�[�{�[�h�E�}�E�X�j
    void CursorRotMouse(PlayerControls playerControls)
    {
        // �����̈ʒu
        Vector2 transPos = transform.position;

        // �X�N���[�����W�n�̃}�E�X���W�����[���h���W�n�ɏC��
        var rawDir = playerControls.Player.MouseDir.ReadValue<Vector2>();
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(rawDir);

        // �x�N�g�����v�Z
        Vector2 diff = (mouseDir - transPos).normalized;

        // ��]�ɑ��
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // �J�[�\���Ƀp�X
        cursor.transform.rotation = curRot;
    }


    // ����i�X�e�B�b�N�j
    void CursorRotStick(PlayerControls playerControls)
    {
        // �X�e�B�b�N�����擾
        var stickDir = playerControls.Player.StickDir.ReadValue<Vector2>();

        // ���͂�������΍X�V���Ȃ�
        if (stickDir == new Vector2(0, 0)) { return; }

        // �x�N�g�����v�Z
        float radian = Mathf.Atan2(stickDir.x, stickDir.y) * Mathf.Rad2Deg;

        // ��]�ɑ��
        if (radian < 0) { radian += 360; }

        // �J�[�\���Ƀp�X
        cursor.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, radian);
    }
}

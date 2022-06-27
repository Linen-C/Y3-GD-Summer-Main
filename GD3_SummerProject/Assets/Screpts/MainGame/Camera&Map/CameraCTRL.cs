using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
    // ゲームオブジェクト
    [Header("プレイヤートランスフォーム")]
    [SerializeField] Transform playerTr;
    [Header("カメラ初期位置")]
    [SerializeField] Vector3 cameraDefPos;         // カメラ初期位置
    [Header("カメラ限界位置：右上")]
    [SerializeField] public Vector2 cameraMaxPos;  // カメラ右上限界位置
    [Header("カメラ限界位置：左下")]
    [SerializeField] public Vector2 cameraMinPos;  // カメラ左下限界位置
    [Header("カメラ遷移用の中心位置")]
    [SerializeField] public Vector3 cameraCenter;
    [Header("トランジション")]
    [SerializeField] public Vector2 cameraNextMaxPos;
    [SerializeField] public Vector2 cameraNextMinPos;

    public enum State
    {
        Nomal,
        Trans
    }
    State _state;

    private void Start()
    {
        _state = State.Nomal;
    }

    void LateUpdate()
    {
        if (_state == State.Nomal)
        {
            Camera_Normal();
        }

        if (_state == State.Trans)
        {
            Camera_Trans();
        }

    }


    void Camera_Normal()
    {
        // 滑らかにプレイヤーへ追従
        Vector3 camPos = Vector3.Lerp(
            transform.position,
            playerTr.position + cameraDefPos,
            3.0f * Time.deltaTime);

        // カメラ位置制限
        camPos.x = Mathf.Clamp(camPos.x, cameraMinPos.x, cameraMaxPos.x);
        camPos.y = Mathf.Clamp(camPos.y, cameraMinPos.y, cameraMaxPos.y);
        camPos.z = cameraDefPos.z;

        // 位置代入
        transform.position = camPos;
    }

    void Camera_Trans()
    {
        Vector3 camPos = Vector3.Lerp(
            transform.position,
            cameraCenter,
            5.0f * Time.deltaTime);

        transform.position = camPos;

        if (Mathf.Abs(cameraCenter.y - transform.position.y) < 0.1f)
        {
            cameraMaxPos = cameraNextMaxPos;
            cameraMinPos = cameraNextMinPos;

            cameraNextMaxPos = new Vector2(0, 0);
            cameraNextMinPos = new Vector2(0, 0);

            _state = State.Nomal;
        }
    }

    public void SetNewCamPoint(Vector2 maxPin,Vector2 minPin,bool downToUp)
    {
        if (cameraMaxPos == maxPin && cameraMinPos == minPin) { return; }

        _state = State.Trans;

        cameraNextMaxPos = maxPin;
        cameraNextMinPos = minPin;

        //Debug.Log("Max" + cameraNextMaxPos);
        //Debug.Log("Min" + cameraNextMinPos);

        if (downToUp)
        {
            cameraCenter = new Vector3(0.0f, cameraNextMinPos.y, -10.0f);
        }
        else
        {
            cameraCenter = new Vector3(0.0f, cameraNextMaxPos.y, -10.0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
    // ゲームオブジェクト
    [Header("初期座標")]
    [SerializeField] Vector3 startPos;
    [Header("プレイヤートランスフォーム")]
    [SerializeField] Transform playerTr;
    [Header("カメラ初期位置")]
    [SerializeField] Vector3 cameraDefPos;         // カメラ中心位置
    [Header("カメラ限界位置：右上")]
    [SerializeField] public Vector2 cameraMaxPos;  // カメラ右上限界位置
    [Header("カメラ限界位置：左下")]
    [SerializeField] public Vector2 cameraMinPos;  // カメラ左下限界位置


    private void Awake()
    {
        transform.position = startPos;
    }

    void LateUpdate()
    {
        Camera_Normal();
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


    public void SetNewCamPoint(Vector2 maxPin, Vector2 minPin/*,bool downToUp*/)
    {
        if (cameraMaxPos == maxPin && cameraMinPos == minPin) { return; }

        //_state = State.Trans;

        cameraMaxPos = maxPin;
        cameraMinPos = minPin;
    }
}

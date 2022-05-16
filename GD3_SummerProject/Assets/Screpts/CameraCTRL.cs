using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
    // ゲームオブジェクト
    // public GameObject player;
    [SerializeField] Transform playerTr;
    [SerializeField] Vector3 cameraDefPos;  // カメラ初期位置
    [SerializeField] Vector2 cameraMaxPos;  // カメラ右上限界位置
    [SerializeField] Vector2 cameraMinPos;  // カメラ左下限界位置

    void LateUpdate()
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
}

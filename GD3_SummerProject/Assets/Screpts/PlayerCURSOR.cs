using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCURSOR : MonoBehaviour
{


    void Start()
    {
        
    }


    void Update()
    {
        // 自分の位置
        Vector2 transPos = transform.position;
        //Debug.Log("tX" + transPos.x + "_" + "tY" + transPos.y);

        // スクリーン座標系のマウス座標をワールド座標系に修正
        Vector2 mouseRawPos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseRawPos);
        //Debug.Log("mX" + mouseWorldPos.x + "_"+ "mY" + mouseWorldPos.y);

        // ベクトルを計算
        Vector2 diff = (mouseWorldPos - transPos).normalized;

        // 回転に代入
        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
    }
}

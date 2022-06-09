using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCTRL : MonoBehaviour
{
    [Header("プレイヤートランスフォーム")]
    [SerializeField] Transform playerTr;
    [Header("マップ(キャンバス)")]
    [SerializeField] Canvas filedMap;
    [Header("マップ移動速度")]
    [SerializeField] int moveSpeed;


    void Start()
    {
        filedMap.enabled = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M)) { SwitchEnable(); }

        /*
        if (filedMap.enabled == true)
        {
            transform.position = new Vector3(
            Input.GetAxis("Horizontal") * moveSpeed,
            Input.GetAxis("Vertical") * moveSpeed,
            -10.0f);
        }
        */
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            playerTr.position.x,
            playerTr.position.y,
            -10.0f);
    }

    void SwitchEnable()
    {
        if (filedMap.enabled == false) { filedMap.enabled = true; }
        else { filedMap.enabled = false; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysSpawn : MonoBehaviour
{
    [Header("エリアオブジェクトをここに")]
    [SerializeField] GameObject areaObject;

    [Header("生成する敵のリスト")]
    [SerializeField] GameObject[] spawnEnemyList;

    [Header("生成する敵の位置")]
    [SerializeField] Vector2[] spwanEnemyPos;

    [Header("エリアオブジェクトの座標")]
    [SerializeField] Vector2 ereaPos;

    void Start()
    {
        ereaPos = new Vector2(areaObject.transform.position.x,
            areaObject.transform.position.y);
    }

    public void DoAwake()
    {
        if (spawnEnemyList.Length < 0) { return; }

        // 座標を　親の位置+ ってすればいける

        for (int i = 0; spawnEnemyList.Length > i; i++)
        {
            float pointx = ereaPos.x + spwanEnemyPos[i].x;
            float pointy = ereaPos.y + spwanEnemyPos[i].y;

            Instantiate(
                spawnEnemyList[i],
                new Vector3(pointx, pointy, 0.0f),
                Quaternion.identity,
                transform);

            //Debug.Log("生成");
        }

    }
}

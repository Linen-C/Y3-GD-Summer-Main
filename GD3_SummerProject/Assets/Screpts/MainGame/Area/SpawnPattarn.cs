using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPattarn : MonoBehaviour
{
    [Header("生成する敵のリスト")]
    [SerializeField] GameObject[] spawnEnemyList;
    [Header("生成する敵の位置")]
    [SerializeField] Vector2[] spwanEnemyPos;
    [Header("アリーナ(自動取得)")]
    [SerializeField] ArenaCTRL arenaCtrl;
    [SerializeField] TutoArenaCTRL tutoArenaCtrl;
    [Header("中心座標")]
    [SerializeField] Vector2 arenaPos;

    void Start()
    {
        arenaCtrl = transform.parent.parent.gameObject.GetComponent<ArenaCTRL>();

        if(arenaCtrl != null)
        {
            arenaPos = new Vector2(arenaCtrl.transform.position.x,
            arenaCtrl.transform.position.y);
        }
        else
        {
            tutoArenaCtrl = transform.parent.parent.gameObject.GetComponent<TutoArenaCTRL>();
            arenaPos = new Vector2(tutoArenaCtrl.transform.position.x,
            tutoArenaCtrl.transform.position.y);
        }
    }

    public void Spawn()
    {
        if (spawnEnemyList.Length < 0) { return; }

        for (int i = 0; spawnEnemyList.Length > i; i++)
        {
            float pointx = arenaPos.x + spwanEnemyPos[i].x;
            float pointy = arenaPos.y + spwanEnemyPos[i].y;

            Instantiate(
                spawnEnemyList[i],
                new Vector3(pointx, pointy, 0.0f),
                Quaternion.identity,
                transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoArenaEntry : MonoBehaviour
{
    [Header("アリーナ(自動取得)")]
    [SerializeField] TutoArenaCTRL arenaCtrl;

    void Start()
    {
        arenaCtrl = transform.parent.GetComponent<TutoArenaCTRL>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }

        // 起動
        arenaCtrl.Entry();

        // 用済みなので消去
        Destroy(gameObject);
    }

}

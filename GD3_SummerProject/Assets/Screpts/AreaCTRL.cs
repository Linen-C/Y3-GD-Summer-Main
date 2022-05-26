using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCTRL : MonoBehaviour
{
    [Header("クリアフラグ")]
    [SerializeField] bool clearFlag = false;
    [Header("各ゲート位置")]
    [SerializeField] public GateCTRL gateCTRL_U;
    [SerializeField] public GateCTRL gateCTRL_D;
    [SerializeField] public GateCTRL gateCTRL_L;
    [SerializeField] public GateCTRL gateCTRL_R;
    [Header("エネミー(自動取得)")]
    [SerializeField] Transform enemys;
    [SerializeField] Transform[] enemyList;
    [Header("エンドポイントフラグ")]
    [SerializeField] bool endPoint = false;
    [Header("ゲームコントロール(自動取得)")]
    [SerializeField] GameObject gameCtrl;


    void Start()
    {
        if (endPoint) { gameCtrl = GameObject.FindGameObjectWithTag("GameController"); }
        enabled = false;
    }

    void Update()
    {
        enemyList = GetEnemyList(enemys);

        if (enemyList.Length <= 0)
        {
            if (endPoint)
            {
                SendGameEND();
                return;
            }

            if (gateCTRL_U != null) { gateCTRL_U.GateOpen(); }
            if (gateCTRL_D != null) { gateCTRL_D.GateOpen(); }
            if (gateCTRL_L != null) { gateCTRL_L.GateOpen(); }
            if (gateCTRL_R != null) { gateCTRL_R.GateOpen(); }
            Debug.Log("殲滅");
            enabled = false;
        }
    }

    private Transform[] GetEnemyList(Transform parent)
    {
        var children = new Transform[parent.childCount];

        for (int i = 0; i < children.Length; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }

    public bool DoClearFlag()
    {
        return clearFlag;
    }

    public void DoAwake()
    {
        enabled = true;
    }

    void SendGameEND()
    {
        gameCtrl.SendMessage("S_GameClear");
    }
}

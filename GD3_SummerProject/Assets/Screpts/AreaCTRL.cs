using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCTRL : MonoBehaviour
{
    [SerializeField] bool clearFlag = false;
    [SerializeField] public GateCTRL gateCTRL_U;
    [SerializeField] public GateCTRL gateCTRL_D;
    [SerializeField] public GateCTRL gateCTRL_L;
    [SerializeField] public GateCTRL gateCTRL_R;
    [SerializeField] Transform enemys;
    [SerializeField] Transform[] enemyList;

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        enemyList = GetEnemyList(enemys);

        if (enemyList.Length <= 0)
        {
            if (gateCTRL_U != null) { gateCTRL_U.GateOpen(); }
            if (gateCTRL_D != null) { gateCTRL_D.GateOpen(); }
            if (gateCTRL_L != null) { gateCTRL_L.GateOpen(); }
            if (gateCTRL_R != null) { gateCTRL_R.GateOpen(); }
            Debug.Log("Ÿr–Å");
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCTRL : MonoBehaviour
{
    [SerializeField] bool clearFlag = false;
    [SerializeField] GameObject gate;
    [SerializeField] Transform enemys;
    [SerializeField] Transform[] enemyList;

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        enemyList = GetEnemyList(enemys);

        if (enemyList.Length > 0)
        {
            //Debug.Log("‚¢‚«‚Ä‚é");
        }
        else { Debug.Log("Ÿr–Å"); }
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

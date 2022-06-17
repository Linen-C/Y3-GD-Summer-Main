using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemyCTRL : MonoBehaviour
{
    [Header("�A���[�i(�����擾)")]
    [SerializeField] ArenaCTRL arenaCtrl;
    [Header("�G�̔z�u�f�[�^")]
    [SerializeField] GameObject[] spawn_pattern_obj;
    [Header("�G�X�|�[���X�N���v�g(�����擾)")]
    [SerializeField] SpawnPattarn[] spawn_pattarn;
    [Header("�G�f�[�^���X�g(�����擾)")]
    [SerializeField] Transform[] enemyList;
    
    [SerializeField] bool running = false;


    void Start()
    {
        arenaCtrl = transform.parent.GetComponent<ArenaCTRL>();

        spawn_pattarn = new SpawnPattarn[spawn_pattern_obj.Length];
        for (int i = 0; spawn_pattern_obj.Length > i; i++)
        {
            spawn_pattarn[i] = spawn_pattern_obj[i].transform.GetComponent<SpawnPattarn>();
        }
    }

    void Update()
    {
        if (running)
        {
            enemyList = GetEnemyList(transform.GetChild(arenaCtrl.now_Wave - 1));
        }
    }


    public void WavaStart()
    {
        spawn_pattarn[arenaCtrl.now_Wave - 1].Spawn();
        enemyList = GetEnemyList(transform.GetChild(arenaCtrl.now_Wave - 1));
        running = true;
    }

    private Transform[] GetEnemyList(Transform parent)
    {
        var children = new Transform[parent.childCount];
        enemyList = new Transform[children.Length];

        for (int i = 0; i < children.Length; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }

    public bool DoEnemyAllDestroy()
    {
        if (enemyList.Length <= 0)
        {
            running = false;
            return true;
        }

        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    [SerializeField] ArenaCTRL _arenaCTRL;
    [SerializeField] GameObject[] _panels;
    [SerializeField] ArenaEnemyCTRL _arenaEnemyCTRL;
    [SerializeField] Transform _enemyTransform;
    //[SerializeField] EnemyCTRL _enemyCTRL;

    void Start()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(false);
        }
    }


    void Update()
    {
        var nowNomber = _arenaCTRL.now_Wave - 1;
        var oldNomber = nowNomber - 1;

        if (nowNomber < 0) { nowNomber = 0; }
        if (oldNomber < 0) { oldNomber = 0; }

        if (_arenaCTRL.inWave)
        {
            _panels[nowNomber].SetActive(true);
        }
        else
        {
            _panels[oldNomber].SetActive(false);
        }

        if(_arenaCTRL.enabled == false)
        {
            _panels[nowNomber].SetActive(false);
        }
    }

    void EnemyOverride()
    {

    }

}

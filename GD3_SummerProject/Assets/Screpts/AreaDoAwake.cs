using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoAwake : MonoBehaviour
{
    public AreaCTRL area;

    [SerializeField] GateCTRL gateCTRL;
    [SerializeField] EnemysSpawn enemySpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (area.DoClearFlag() == true) { return; }
        area.DoAwake();
        enemySpawn.DoAwake();
        gateCTRL.GateClose();
        Destroy(gameObject);
    }
}

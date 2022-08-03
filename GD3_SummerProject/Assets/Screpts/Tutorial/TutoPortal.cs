using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoPortal : MonoBehaviour
{
    [SerializeField] TutoStageManager _stageManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        _stageManager.GetSignal();
    }
}

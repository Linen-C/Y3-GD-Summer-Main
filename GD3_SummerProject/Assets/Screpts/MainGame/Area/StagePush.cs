using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePush : MonoBehaviour
{
    [SerializeField] float _defEventTimer;
    [SerializeField] Transform _player;
    [SerializeField] PlayerMove _playerMove;

    private void Start()
    {
        _playerMove = _player.GetComponent<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        _playerMove.EventMove(_defEventTimer);
    }

}

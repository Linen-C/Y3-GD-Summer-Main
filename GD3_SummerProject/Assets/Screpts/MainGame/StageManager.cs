using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    
    [SerializeField] Portal _portal;
    [SerializeField] GameObject[] _arenas;
    [SerializeField] int _nowArenaNo;
    [SerializeField] GameObject _cloned;
    [SerializeField] GC_GameCTRL _gameCTRL;
    [SerializeField] Transform _entryPoint;
    [SerializeField] Transform _player;


    void Start()
    {
        SetArena();
    }

    void SetArena()
    {
        _cloned = Instantiate(_arenas[_nowArenaNo], transform);
    }

    public void GetSignal()
    {
        Destroy(_cloned);

        _nowArenaNo++;

        if (_nowArenaNo == _arenas.Length)
        {
            _gameCTRL.S_GameClear();
            return;
        }

        _player.position = _entryPoint.position;
        SetArena();
    }
}

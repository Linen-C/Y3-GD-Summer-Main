using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    
    [SerializeField] Portal _portal;
    [SerializeField] GameObject[] _arenas;
    [SerializeField] Transform _entryPoint;
    [SerializeField] Transform _player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void GetSignal()
    {
        _player.position = _entryPoint.position;
    }
}

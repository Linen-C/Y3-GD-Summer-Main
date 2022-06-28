using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    [SerializeField] Canvas _Main;
    [SerializeField] Canvas _Option;

    public void Option_to_Main()
    {
        _Option.enabled = false;
        _Main.enabled = true;
    }
}

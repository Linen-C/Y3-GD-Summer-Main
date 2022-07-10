using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Option : MonoBehaviour
{
    [SerializeField] Canvas _Main;
    [SerializeField] Canvas _Option;

    [SerializeField] AudioCTRL _AudioCTRL;
    [SerializeField] float _volume;
    [SerializeField] Slider _SEslider;

    public void Option_to_Main()
    {
        _Option.enabled = false;
        _Main.enabled = true;
    }

    public void CloseOption()
    {
        if (_AudioCTRL = null)
        {
            _AudioCTRL.defVolume = _volume;
        }

        Destroy(gameObject);
    }
}

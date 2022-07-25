using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Option : MonoBehaviour
{
    [Header("キャンバス")]
    [SerializeField] Canvas _Option;

    [Header("セーブデータ")]
    [SerializeField] OptionData _OptionData;

    [Header("オーディオ")]
    [SerializeField] AudioCTRL _AudioCTRL;
    [SerializeField] float _volume;
    [SerializeField] Slider _slider_SE;

    [Header("画面サイズ")]
    //[SerializeField] ToggleGroup _ToggleGroup;
    [SerializeField] Toggle _toggle_Window;
    [SerializeField] Toggle _toggle_FullSC;


    void Start()
    {


        if (Screen.fullScreen) { _toggle_FullSC.isOn = true; }
        else { _toggle_Window.isOn = true; }
    }


    public void Toggle_SCmode_Window()
    {
        if (_toggle_Window.isOn && Screen.fullScreen)
        {
            Screen.fullScreen = false;
            Debug.Log("Swaped_WindowMode");
        }
    }

    public void Toggle_SCmode_FullSC()
    {
        if (_toggle_FullSC.isOn && !Screen.fullScreen)
        {
            Screen.fullScreen = true;
            Debug.Log("Swaped_FullSC");
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class Option : MonoBehaviour
{
    [Header("キャンバス")]
    [SerializeField] Canvas _Option;

    [Header("セーブデータ")]
    [SerializeField] OptionData _OptionData;

    [Header("コントローラー用の設定")]
    [SerializeField] GameObject _firstButton;
    [SerializeField] GameObject _returnButton;

    [Header("オーディオ")]
    [SerializeField] MainAudioCTRL _Main_AudioCTRL;
    [SerializeField] MenuAudioCTRL _Menu_AudioCTRL;
    [SerializeField] float _volume;
    [SerializeField] Slider _slider_SE;
    [SerializeField] GameObject _audio;

    [Header("画面サイズ")]
    [SerializeField] bool _isFullSC;
    [SerializeField] Toggle _toggle_Window;
    [SerializeField] Toggle _toggle_FullSC;
    

    string _location;   // オプションファイルの場所


    void Start()
    {
        OptionLoad();

        PadCheck();

        _audio = GameObject.FindGameObjectWithTag("AudioController");
        if (_audio != null)
        {
            if (_audio.GetComponent<MainAudioCTRL>())
            {
                _Main_AudioCTRL = _audio.GetComponent<MainAudioCTRL>();
            }
            if (_audio.GetComponent<MenuAudioCTRL>())
            {
                _Menu_AudioCTRL = _audio.GetComponent<MenuAudioCTRL>();
            }
        }

    }

    void PadCheck()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_firstButton);

        _returnButton = GameObject.FindGameObjectWithTag("returnButton");
    }



    void OptionLoad()
    {
        // セーブデータ読み込み
        _location = Application.streamingAssetsPath + "/jsons/OptionSave.json";
        string inputJson = File.ReadAllText(_location).ToString();
        _OptionData = JsonUtility.FromJson<OptionData>(inputJson);

        // SE音量
        _volume = _OptionData.SEvolume;
        _slider_SE.value = _volume;
        //Debug.Log("volume：" + _volume);

        // スクリーンサイズ
        _isFullSC = _OptionData.fullScreen;
        //Debug.Log("FullSC：" + _isFullSC);

        Screen.fullScreen = _isFullSC;
        
        if (_isFullSC){ _toggle_FullSC.isOn = true; }
        else{ _toggle_Window.isOn = true; }

    }

    void OptionSave()
    {
        // SE音量
        _OptionData.SEvolume = _volume;
        //Debug.Log("volume：" + _OptionData.SEvolume);

        // スクリーンサイズ
        _OptionData.fullScreen = _isFullSC;
        //Debug.Log("FullSC：" + _OptionData.fullScreen);

        // セーブデータ書き込み
        var datas = JsonUtility.ToJson(_OptionData, true);
        File.WriteAllText(_location, datas);
    }



    public void SetVolume()
    {
        _volume = _slider_SE.value;
    }


    public void Toggle_SCmode_Window()
    {
        if (_toggle_Window.isOn && Screen.fullScreen)
        {
            _isFullSC = false;
            Debug.Log("Swaped_WindowMode");
        }
    }

    public void Toggle_SCmode_FullSC()
    {
        if (_toggle_FullSC.isOn && !Screen.fullScreen)
        {
            _isFullSC = true;
            Debug.Log("Swaped_FullSC");
        }
    }



    public void CloseOption()
    {
        OptionSave();

        if (_Main_AudioCTRL != null) { _Main_AudioCTRL.VolumeSet(_volume); }
        if (_Menu_AudioCTRL != null) { _Menu_AudioCTRL.VolumeSet(_volume); }

        Screen.fullScreen = _isFullSC;

        if (_returnButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_returnButton);
        }

        Destroy(gameObject);
    }
}

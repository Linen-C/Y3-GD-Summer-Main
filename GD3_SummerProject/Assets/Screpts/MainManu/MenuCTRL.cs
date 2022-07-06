using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCTRL : MonoBehaviour
{
    [SerializeField] Canvas _Main; 
    [SerializeField] Canvas _Customize;
    [SerializeField] Menu_Customize _menu_Customize;
    [SerializeField] Canvas _Select; 
    //[SerializeField] Canvas _Standby;
    [SerializeField] Canvas _Option;

    void Awake()
    {
        _Customize.enabled = false;
        _Select.enabled = false;
        //_Standby.enabled = false;
        _Option.enabled = false;
    }


    // カスタマイズ画面へ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Customize()
    {
        _Main.enabled = false;
        _menu_Customize.EnableMenu();
        _Customize.enabled = true;
    }


    // セレクト画面へ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Select()
    {
        _Main.enabled = false;
        _Select.enabled = true;
    }


    // タイトル画面へ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Title()
    {
        SceneManager.LoadScene("TitleScene");
    }


    // オプションメニュー起動
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void OptionEnable()
    {
        _Main.enabled = false;
        _Option.enabled = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCTRL : MonoBehaviour
{
    [Header("各メニュー情報")]
    [SerializeField] Canvas _Main; 
    [SerializeField] Canvas _Customize;
    [SerializeField] Menu_Customize _menu_Customize;
    [SerializeField] Canvas _Select; 
    [SerializeField] Canvas _StandbyMaingame;
    [SerializeField] Canvas _Option;

    [Header("ボタン")]
    [SerializeField] Menu_Button _menu_Button;

    void Awake()
    {
        _Customize.enabled = false;
        _Select.enabled = false;
        _StandbyMaingame.enabled = false;
        _Option.enabled = false;
    }

    // カスタマイズ画面へ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Customize()
    {
        _Main.enabled = false;
        _menu_Customize.EnableMenu(_Main);

        _menu_Button.B_Custom();
        Debug.Log("Main→Custom");
    }


    // セレクト画面へ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Select()
    {
        _Main.enabled = false;
        _Select.enabled = true;

        _menu_Button.B_select();
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

        _menu_Button.B_Option();
    }

}

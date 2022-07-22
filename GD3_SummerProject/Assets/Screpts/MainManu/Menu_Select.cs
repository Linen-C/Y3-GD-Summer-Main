using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Select : MonoBehaviour
{
    [Header("キャンバス")]
    [SerializeField] Canvas _Main;
    [SerializeField] Canvas _Select;
    [SerializeField] Canvas _StundbyMaingame;

    [Header("ボタン")]
    [SerializeField] Menu_Button _menu_Button;

    // 各ステージへ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_MainGame()
    {
        _Select.enabled = false;
        _StundbyMaingame.enabled = true;

        _menu_Button.B_StundbyMG();
    }

    /*
    public void Select_Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    */

    // メインメニューへ戻る
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_Main()
    {
        _Select.enabled = false;
        _Main.enabled = true;

        _menu_Button.B_MainMenu();
    }

}

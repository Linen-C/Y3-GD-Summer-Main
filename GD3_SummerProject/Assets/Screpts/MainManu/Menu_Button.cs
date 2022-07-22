using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu_Button : MonoBehaviour
{
    [Header("メインメニュー")]
    [SerializeField] Button _Button_mainmenu;
    [Header("カスタマイズ")]
    [SerializeField] Button _Button_custom;
    [Header("ステージセレクト")]
    [SerializeField] Button _Button_select;
    [Header("スタンバイ")]
    [SerializeField] Button _Button_standbyMG;
    [Header("オプション")]
    [SerializeField] Button _Button_option;


    void Awake()
    {
        var padName = Gamepad.current;
        if (padName == null)  { return; }

        B_MainMenu();
    }

    public void B_MainMenu()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        _Button_mainmenu.Select();
    }

    public void B_Custom()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        _Button_custom.Select();
    }

    public void B_select()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        _Button_select.Select();
    }

    public void B_StundbyMG()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        _Button_standbyMG.Select();
    }
    
    public void B_Option()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        _Button_option.Select();
    }

}

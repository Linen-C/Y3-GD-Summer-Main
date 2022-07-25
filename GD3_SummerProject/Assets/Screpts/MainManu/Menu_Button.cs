using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Menu_Button : MonoBehaviour
{
    [Header("メインメニュー")]
    [SerializeField] GameObject _Button_mainmenu;
    [Header("カスタマイズ")]
    [SerializeField] GameObject _Button_custom;
    [Header("ステージセレクト")]
    [SerializeField] GameObject _Button_select;
    [Header("スタンバイ")]
    [SerializeField] GameObject _Button_standbyMG;
    //[Header("オプション")]
    //[SerializeField] GameObject _Button_option;


    void Awake()
    {
        B_MainMenu();
    }

    public void B_MainMenu()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_mainmenu);
        //_Button_mainmenu.Select();
    }

    public void B_Custom()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_custom);
    }

    public void B_select()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_select);
    }

    public void B_StundbyMG()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_standbyMG);
    }
    
    /*
    public void B_Option()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_option);
    }
    */
}

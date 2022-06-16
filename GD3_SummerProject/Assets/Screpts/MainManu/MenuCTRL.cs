using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCTRL : MonoBehaviour
{
    [SerializeField] Canvas _Main; 
    [SerializeField] Canvas _Customize; 
    [SerializeField] Canvas _Select; 
    [SerializeField] Canvas _Standby; 

    void Awake()
    {
        _Customize.enabled = false;
        _Select.enabled = false;
        _Standby.enabled = false;
    }


    // メインメニュー
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Customize()
    {
        _Main.enabled = false;
        _Customize.enabled = true;
    }

    public void Main_to_Select()
    {
        _Main.enabled = false;
        _Select.enabled = true;
    }

    // カスタマイズ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Customize_to_Main()
    {
        _Customize.enabled = false;
        _Main.enabled = true;
    }

    // セレクト
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_Main()
    {
        _Select.enabled = false;
        _Main.enabled = true;
    }


    // スタンバイ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //


}

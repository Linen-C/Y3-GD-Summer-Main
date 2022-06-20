using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Select : MonoBehaviour
{
    [SerializeField] Canvas _Main;
    [SerializeField] Canvas _Select;

    void Start()
    {
        
    }

    // 各ステージへ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_DebugStage()
    {
        SceneManager.LoadScene("BetaTest");
    }


    // メインメニューへ戻る
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_Main()
    {
        _Select.enabled = false;
        _Main.enabled = true;
    }

}

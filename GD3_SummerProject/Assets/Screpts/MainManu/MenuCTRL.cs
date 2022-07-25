using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuCTRL : MonoBehaviour
{
    [Header("各メニュー情報")]
    [SerializeField] GameObject _Main; 
    [SerializeField] GameObject _Customize;
    [SerializeField] Menu_Customize _menu_Customize;
    [SerializeField] GameObject _Select; 
    [SerializeField] GameObject _StandbyMaingame;
    [SerializeField] Menu_StundbyMaingame _stMG;
    [SerializeField] GameObject _Option;

    [Header("アニメーター")]
    [SerializeField] Animator _animator;

    [Header("ボタン")]
    [SerializeField] Menu_Button _menu_Button;

    void Awake()
    {
        //_Option.SetActive(false);
        _animator.SetBool("Main_Bool", true);
    }

    // Any→カスタマイズ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Any_to_Customize()
    {
        _menu_Customize.EnableMenu();
        _animator.SetBool("Custom_Bool", true);
    }

    // メイン→セレクト
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Select()
    {
        _animator.SetBool("Main_Bool", false);
        _animator.SetBool("Select_Bool", true);
    }


    // セレクト→メイン
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_Main()
    {
        _animator.SetBool("Select_Bool", false);
        _animator.SetBool("Main_Bool", true);
    }

    // セレクト→メインゲーム
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_StMG()
    {
        _stMG.SetWeaponImage();

        _animator.SetBool("Select_Bool", false);
        _animator.SetBool("StMG_Bool", true);
    }


    // メインゲーム→セレクト
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void StMG_to_Select()
    {
        _animator.SetBool("StMG_Bool", false);
        _animator.SetBool("Select_Bool", true);
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
        EventSystem.current.SetSelectedGameObject(null);
        Instantiate(_Option);
    }
}

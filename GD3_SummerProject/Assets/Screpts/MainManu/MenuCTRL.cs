using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("オーディオ")]
    [SerializeField] MenuAudioCTRL _menu_AudioCTRL;  // オーディオコントロール
    [SerializeField] AudioSource audioSource;        // オーディオソース
    [SerializeField] AudioClip[] audioClip;          // クリップ


    void Start()
    {
        // オーディオ初期化
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = _menu_AudioCTRL.nowVolume;

        // こんな具合でいけそう
        //audioSource.PlayOneShot(_menu_AudioCTRL._clips[0]);
    }

        void Awake()
    {
        _menu_Button.B_MainMenu();

        _animator.SetBool("Main_Bool", true);
    }

    // Any→カスタマイズ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Any_to_Customize()
    {
        _menu_Customize.EnableMenu();
        _animator.SetBool("Custom_Bool", true);

        _menu_Button.B_Custom();
    }

    // メイン→セレクト
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Main_to_Select()
    {
        _menu_Button.B_select();
        _animator.SetBool("Main_Bool", false);
        _animator.SetBool("Select_Bool", true);
    }


    // セレクト→メイン
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_Main()
    {
        _animator.SetBool("Select_Bool", false);
        _animator.SetBool("Main_Bool", true);
        _menu_Button.B_MainMenu();
    }

    // セレクト→メインゲーム
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Select_to_StMG()
    {
        _stMG.Select_StMG();

        _animator.SetBool("Select_Bool", false);
        _animator.SetBool("StMG_Bool", true);

        _menu_Button.B_StundbyMG();
    }


    // メインゲーム→セレクト
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void StMG_to_Select()
    {
        _animator.SetBool("StMG_Bool", false);
        _animator.SetBool("Select_Bool", true);

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
        EventSystem.current.SetSelectedGameObject(null);
        Instantiate(_Option);
    }
}

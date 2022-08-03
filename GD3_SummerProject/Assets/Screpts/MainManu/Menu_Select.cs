using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Select : MonoBehaviour
{
    [Header("ボタン")]
    [SerializeField] Menu_Button _menu_Button;

    // 各ステージへ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    public void Select_Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}

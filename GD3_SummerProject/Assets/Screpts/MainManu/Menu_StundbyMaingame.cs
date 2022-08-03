using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Menu_StundbyMaingame : MonoBehaviour
{
    [Header("装備表示用")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] WeaponList[] equipList;
    [SerializeField] Image _gunImage;
    [SerializeField] Image _weaponImage_A;
    [SerializeField] Image _weaponImage_B;

    [Header("ハイスコア表示")]
    [SerializeField] TextMeshProUGUI _hiScoreText;

    //[Header("難易度設定")]
    //[SerializeField] public int _difficulty = 0;
    //[SerializeField] int _setBPM = 120;
    //[SerializeField] int _stageQuantity = 3;

    [Header("表示")]
    [SerializeField] TextMeshProUGUI _displayText;

    [Header("ボタン")]
    [SerializeField] Menu_Button _menu_Button;


    public void Select_StMG()
    {
        SetWeaponImage();

        ShowHiScore();
        //DiffSet_Eazy();
    }


    public void SetWeaponImage()
    {
        equipList = new WeaponList[2];
        equipList = saveManager.EquipLoad();

        _weaponImage_A.sprite = Resources.Load<Sprite>(equipList[0].icon);
        _weaponImage_B.sprite = Resources.Load<Sprite>(equipList[1].icon);
    }

    //public void DiffSet_Eazy()
    //{
    //    _difficulty = 0;
    //    _setBPM = 120;
    //    _stageQuantity = 3;

    //    DisplayUpdate();
    //}

    //public void DiffSet_Nomal()
    //{
    //    _difficulty = 1;
    //    _setBPM = 140;
    //    _stageQuantity = 5;

    //    DisplayUpdate();
    //}

    //public void DiffSet_Hard()
    //{
    //    _difficulty = 2;
    //    _setBPM = 180;
    //    _stageQuantity = 7;

    //    DisplayUpdate();
    //}

    void DisplayUpdate()
    {
        //_displayText.text = "ステージ数\t：" + _stageQuantity + "\nBPM\t：" + _setBPM;
    }

    void ShowHiScore()
    {
        var location = Application.streamingAssetsPath + "/jsons/HiScore.json";
        string inputJson = File.ReadAllText(location).ToString();
        HiScore hiScore = JsonUtility.FromJson<HiScore>(inputJson);

        _hiScoreText.text = "ハイスコア\n・Point：" + hiScore.point + "\n・Floor：" + hiScore.floor;
    }

    public void GameStart_Nomal()
    {
        SceneManager.LoadScene("MainGame");
    }

}

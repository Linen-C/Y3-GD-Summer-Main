using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Menu_StundbyMaingame : MonoBehaviour
{
    [Header("�����\���p")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] WeaponList[] equipList;
    [SerializeField] Image _gunImage;
    [SerializeField] Image _weaponImage_A;
    [SerializeField] Image _weaponImage_B;

    [Header("�n�C�X�R�A�\��")]
    [SerializeField] TextMeshProUGUI _hiScoreText;

    //[Header("��Փx�ݒ�")]
    //[SerializeField] public int _difficulty = 0;
    //[SerializeField] int _setBPM = 120;
    //[SerializeField] int _stageQuantity = 3;

    [Header("�\��")]
    [SerializeField] TextMeshProUGUI _displayText;

    [Header("�{�^��")]
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
        //_displayText.text = "�X�e�[�W��\t�F" + _stageQuantity + "\nBPM\t�F" + _setBPM;
    }

    void ShowHiScore()
    {
        var location = Application.streamingAssetsPath + "/jsons/HiScore.json";
        string inputJson = File.ReadAllText(location).ToString();
        HiScore hiScore = JsonUtility.FromJson<HiScore>(inputJson);

        _hiScoreText.text = "�n�C�X�R�A\n�EPoint�F" + hiScore.point + "\n�EFloor�F" + hiScore.floor;
    }

    public void GameStart_Nomal()
    {
        SceneManager.LoadScene("MainGame");
    }

}

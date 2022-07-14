using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_StundbyMaingame : MonoBehaviour
{
    [Header("‰æ–Ê‘JˆÚ—p")]
    [SerializeField] Canvas _StundbyMaingame;
    [SerializeField] Canvas _Select;
    [SerializeField] Canvas _Customize;
    [SerializeField] Menu_Customize _menu_Customize;

    [Header("‘•”õ•\Ž¦—p")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] WeaponList[] equipList;
    [SerializeField] Image _gunImage;
    [SerializeField] Image _weaponImage_A;
    [SerializeField] Image _weaponImage_B;

    private bool _selected = false;

    private void Update()
    {
        if (!_selected && _StundbyMaingame.enabled)
        {
            equipList = new WeaponList[2];
            equipList = saveManager.EquipLoad();

            _weaponImage_A.sprite = Resources.Load<Sprite>(equipList[0].icon);
            _weaponImage_B.sprite = Resources.Load<Sprite>(equipList[1].icon);

            _selected = true;
        }
    }

    public void StMaingame_to_Select()
    {
        _StundbyMaingame.enabled = false;
        _selected = false;
        _Select.enabled = true;
    }

    public void StMaingame_to_Customize()
    {
        _StundbyMaingame.enabled = false;
        _selected = false;
        _menu_Customize.EnableMenu(_StundbyMaingame);
    }

    public void GameStart_Nomal()
    {
        SceneManager.LoadScene("MainGame");
    }

}

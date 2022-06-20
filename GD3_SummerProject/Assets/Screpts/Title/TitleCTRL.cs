using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCTRL : MonoBehaviour
{
    [SerializeField] Text weapon1_tex;
    [SerializeField] Text weapon2_tex;
    [SerializeField] Text weapon3_tex;

    [SerializeField] WeaponListLoad weaponList;
    JsonData jsonData;
    public WeaponList[] equipList;

    [SerializeField] SaveManager saveManager;

    private void Start()
    {
        weaponList = GetComponent<WeaponListLoad>();

        equipList = new WeaponList[3];

        jsonData = weaponList.GetList();

        //saveManager.EquipLoad();
        TestEquip();

        Debug.Log(equipList[0].name);
        Debug.Log(equipList[1].name);
        Debug.Log(equipList[2].name);
    }


    void TestEquip()
    {
        equipList[0] = jsonData.weaponList[0];
        equipList[1] = jsonData.weaponList[1];
        equipList[2] = jsonData.weaponList[4];
    }

    public void WeaponSelectSwap()
    {
        
    }

    public void UIM_ClockToGame()
    {
        SceneManager.LoadScene("BetaTest");
    }

    public void UIM_Shutdown()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] Menu_Customize cutomize;
    [SerializeField] EquipLoad equipLoad;

    string filePath;
    JsonData saveDatas;
    JsonData getDatas;

    private void Awake()
    {
        equipLoad.GetComponent<EquipLoad>();

        saveDatas = new JsonData();
        saveDatas.weaponList = new WeaponList[3];

        filePath = Application.dataPath + "/Resources/jsons/EquipSave.json";
        //Debug.Log(filePath);
    }


    // セーブ
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void EquipSave()
    {
        for (int i = 0; i < saveDatas.weaponList.Length; i++)
        {
            saveDatas.weaponList[i] = cutomize.equipList[i];
        }

        string datas = JsonUtility.ToJson(saveDatas, true);
        
        Debug.Log(datas);

        File.WriteAllText(filePath, datas);
    }


    // ロード
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public WeaponList[] EquipLoad()
    {
        getDatas = equipLoad.GetList();
        for (int i = 0; i < saveDatas.weaponList.Length; i++)
        {
            saveDatas.weaponList[i] = getDatas.weaponList[i];
        }

        return saveDatas.weaponList;
    }
}

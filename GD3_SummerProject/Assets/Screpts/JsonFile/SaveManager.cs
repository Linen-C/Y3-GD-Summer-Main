using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] Menu_Customize cutomize;
    [SerializeField] EquipLoad equipLoad;

    string _equipSavePath;
    JsonData saveDatas;
    JsonData getDatas;

    private void Awake()
    {
        equipLoad.GetComponent<EquipLoad>();

        saveDatas = new JsonData();
        saveDatas.weaponList = new WeaponList[2];

        //filePath = Application.dataPath + "/Resources/jsons/EquipSave.json";
        _equipSavePath = Application.streamingAssetsPath + "/jsons/EquipSave.json";
        //Debug.Log("Path\n" + filePath);
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
        
        //Debug.Log("SaveData\n" + datas);

        File.WriteAllText(_equipSavePath, datas);
    }


    // ロード
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public WeaponList[] EquipLoad()
    {
        getDatas = equipLoad.GetList();
        for (int i = 0; i < saveDatas.weaponList.Length; i++)
        {
            saveDatas.weaponList[i] = getDatas.weaponList[i];
            //Debug.Log("LoadData" + i + "：" + saveDatas.weaponList[i].name);
        }

        return saveDatas.weaponList;
    }
}

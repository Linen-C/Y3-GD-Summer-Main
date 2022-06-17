using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] TitleCTRL titleCTRL;
    //[SerializeField] EquipLoad equipLoad;

    string filePath;
    JsonData saveDatas;
    JsonData getDatas;

    private void Awake()
    {
        //equipLoad.GetComponent<EquipLoad>();

        saveDatas = new JsonData();
        saveDatas.weaponList = new WeaponList[3];

        filePath = Application.dataPath + "/Resources/jsons/EquipSave.json";
        //Debug.Log(filePath);
    }


    public void EquipSave()
    {
        for (int i = 0; i < saveDatas.weaponList.Length; i++)
        {
            saveDatas.weaponList[i] = new WeaponList();
            saveDatas.weaponList[i] = titleCTRL.equipList[i];
        }

        string datas = JsonUtility.ToJson(saveDatas, true);
        
        Debug.Log(datas);

        File.WriteAllText(filePath, datas);
    }

    public void EquipLoad()
    {
        //getDatas = equipLoad.GetList();
        /*
        for (int i = 0; i < saveDatas.weaponList.Length; i++)
        {
            getDatas.weaponList[i] = new WeaponList();
            titleCTRL.equipList[i] = getDatas.weaponList[i];
        }
        */
    }
}

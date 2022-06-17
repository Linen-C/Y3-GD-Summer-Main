using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipLoad : MonoBehaviour
{
    JsonData inputData;

    void Awake()
    {
        inputData = new JsonData();
        inputData.weaponList = new WeaponList[5];

        string inputString = Resources.Load<TextAsset>("jsons/EquipSave").ToString();
        inputData = JsonUtility.FromJson<JsonData>(inputString);
    }

    public JsonData GetList()
    {
        return inputData;
    }
}

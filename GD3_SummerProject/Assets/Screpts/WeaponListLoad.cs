using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponListLoad : MonoBehaviour
{
    JsonData inputData;

    void Awake()
    {
        inputData = new JsonData();
        inputData.weaponList = new WeaponList[3];

        string inputString = Resources.Load<TextAsset>("jsons/WeaponList").ToString();
        inputData = JsonUtility.FromJson<JsonData>(inputString);
    }

    public JsonData GetList()
    {
        return inputData;
    }
}

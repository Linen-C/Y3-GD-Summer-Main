using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipLoad : MonoBehaviour
{
    JsonData inputData;

    void Awake()
    {
        string inputString = Resources.Load<TextAsset>("jsons/EquipSave").ToString();
        inputData = JsonUtility.FromJson<JsonData>(inputString);

        //Debug.Log(inputString);
    }

    public JsonData GetList()
    {
        return inputData;
    }
}

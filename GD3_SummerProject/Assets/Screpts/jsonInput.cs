using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeponList
{
    public InputData[] weponList;
}

[System.Serializable]
public class InputData
{
    public string name;
    public int cool;
    public int wideth;
    public int height;
    public float offset;
}

public class jsonInput : MonoBehaviour
{

    WeponList inputData;

    void Awake()
    {
        string inputString = Resources.Load<TextAsset>("TestWeponList").ToString();
        inputData = JsonUtility.FromJson<WeponList>(inputString);

        /*
        Debug.Log(inputData.weponList[0].name);
        Debug.Log(inputData.weponList[0].cool);
        Debug.Log(inputData.weponList[0].height);
        Debug.Log(inputData.weponList[0].wideth);

        Debug.Log(inputData.weponList[1].name);
        Debug.Log(inputData.weponList[1].cool);
        Debug.Log(inputData.weponList[1].height);
        Debug.Log(inputData.weponList[1].wideth);

        var test = inputData.weponList[0].height + inputData.weponList[1].height;
        Debug.Log(test);
        */

        Debug.Log("jI_WeponList_LoadÅF" + inputData.weponList.Length);
    }

    public WeponList SendList()
    {
        return inputData;
    }
}

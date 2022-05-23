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
    public string image;
    public int cool;
    public int wideth;
    public int height;
    public float offset;
}

public class GC_jsonInput : MonoBehaviour
{

    WeponList inputData;

    void Awake()
    {
        string inputString = Resources.Load<TextAsset>("jsons/TestWeponList").ToString();
        inputData = JsonUtility.FromJson<WeponList>(inputString);

        Debug.Log("jsonインプット：" + inputData.weponList.Length);
    }

    public WeponList SendList()
    {
        return inputData;
    }
}

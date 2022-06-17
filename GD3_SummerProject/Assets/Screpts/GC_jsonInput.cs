using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JsonData
{
    public WeponList[] weponList;
}

[System.Serializable]
public class WeponList
{
    //public Tags tags;
    public string name;
    public string image;
    //public Status status;
    public int damage;
    public int defknockback;
    public int maxknockback;
    public int maxcharge;
    //public Sprites sprites;
    public int wideth;
    public int height;
    public float offset;
}


public class GC_jsonInput : MonoBehaviour
{

    JsonData inputData;

    void Awake()
    {
        string inputString = Resources.Load<TextAsset>("jsons/WeponList").ToString();
        inputData = JsonUtility.FromJson<JsonData>(inputString);

        /*
        Debug.Log("jsonインプット：" + inputData.weponList.Length);
        Debug.Log("Tags");
        Debug.Log("Name：" + inputData.weponList[0].name);
        Debug.Log("Image：" + inputData.weponList[0].image);
        Debug.Log("Status");
        Debug.Log("Damage：" + inputData.weponList[0].damage);
        Debug.Log("Knockback：" + inputData.weponList[0].defknockback);
        Debug.Log("Knockback：" + inputData.weponList[0].maxknockback);
        Debug.Log("Maxcharge：" + inputData.weponList[0].maxcharge);
        Debug.Log("Sprites");
        Debug.Log("Wideth：" + inputData.weponList[0].wideth);
        Debug.Log("Height：" + inputData.weponList[0].height);
        Debug.Log("Offset：" + inputData.weponList[0].offset);
        */
    }

    public JsonData GetList()
    {
        return inputData;
    }
}

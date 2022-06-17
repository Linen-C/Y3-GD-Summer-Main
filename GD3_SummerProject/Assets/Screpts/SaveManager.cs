using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [SerializeField] TitleCTRL titleCTRL;

    WeponList saveData1;
    string filePath;

    private void Awake()
    {
        //filePath = Path.Combine(Application.persistentDataPath, "testoutput.json");
        filePath = Application.persistentDataPath + "/test.json";
    }

    public void DataSaveTest()
    {
        saveData1 = titleCTRL.equipData1;

        var data1 = JsonUtility.ToJson(saveData1);

        Debug.Log(data1);

        File.WriteAllText(filePath, data1);
    }
}

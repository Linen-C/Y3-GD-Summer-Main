using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCTRL : MonoBehaviour
{
    [SerializeField] Text wepon1_tex;
    [SerializeField] Text wepon2_tex;
    [SerializeField] Text wepon3_tex;

    [SerializeField] GC_jsonInput jsonInput;
    public JsonData jsonData;

    public WeponList equipData1;
    public WeponList equipData2;
    public WeponList equipData3;

    private void Start()
    {
        jsonInput = GetComponent<GC_jsonInput>();

        jsonData = jsonInput.GetList();

        TestEquip();

        Debug.Log(equipData1.name);
    }




    void TestEquip()
    {
        equipData1 = jsonData.weponList[0];
        //equipData2 = jsonData.weponList[1];
        //equipData3 = jsonData.weponList[2];
    }

    public void WeponSelectSwap()
    {
        
    }

    public void UIM_ClockToGame()
    {
        SceneManager.LoadScene("BetaTest");
    }

    public void UIM_Shutdown()
    {
        Application.Quit();
    }
}

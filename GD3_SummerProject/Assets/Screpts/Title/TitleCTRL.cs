using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCTRL : MonoBehaviour
{
    private void Start()
    {

    }


    public void UIM_ClockToGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UIM_Shutdown()
    {
        Application.Quit();
    }
}

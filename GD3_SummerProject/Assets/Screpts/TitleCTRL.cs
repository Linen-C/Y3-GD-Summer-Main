using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCTRL : MonoBehaviour
{
    public void ClockToGame()
    {
        SceneManager.LoadScene("DebugScene");
    }
}

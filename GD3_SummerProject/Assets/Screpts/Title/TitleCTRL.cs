using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TitleCTRL : MonoBehaviour
{
    [SerializeField] GameObject _button;

    private void Start()
    {
        var padName = Gamepad.current;
        if (padName == null) { return; }
        EventSystem.current.SetSelectedGameObject(_button);
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

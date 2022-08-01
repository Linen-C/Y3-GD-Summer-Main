using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Menu_Button : MonoBehaviour
{
    [Header("���C�����j���[")]
    [SerializeField] GameObject _Button_mainmenu;
    [Header("�J�X�^�}�C�Y")]
    [SerializeField] GameObject _Button_custom;
    [Header("�X�e�[�W�Z���N�g")]
    [SerializeField] GameObject _Button_select;
    [Header("�X�^���o�C")]
    [SerializeField] GameObject _Button_standbyMG;


    void Awake()
    {
        B_MainMenu();
    }

    private void Update()
    {
        if (!PadCheck())
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        }
    }

    public void B_MainMenu()
    {
        if (PadCheck()) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_mainmenu);
    }

    public void B_Custom()
    {
        if (PadCheck()) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_custom);
    }

    public void B_select()
    {
        if (PadCheck()) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_select);
    }

    public void B_StundbyMG()
    {
        if (PadCheck()) { return; }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_Button_standbyMG);
    }

    bool PadCheck()
    {
        var padName = Gamepad.current;
        if (padName == null) { return true; }

        return false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Select : MonoBehaviour
{
    [Header("�{�^��")]
    [SerializeField] Menu_Button _menu_Button;

    // �e�X�e�[�W��
    // ���������� ���������� ���������� ���������� ���������� //

    public void Select_Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}

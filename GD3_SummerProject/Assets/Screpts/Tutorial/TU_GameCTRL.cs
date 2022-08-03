using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TU_GameCTRL : MonoBehaviour
{
    [Header("BPM�R���g���[��")]
    [SerializeField] GC_BpmCTRL bpmCtrl;

    [Header("�v���C���[���")]
    [SerializeField] PlayerCTRL playerCtrl;

    [Header("�G���A���")]
    [SerializeField] TutoStageManager _stage;

    [Header("UI")]
    [SerializeField] GameObject upperPanel;
    [SerializeField] Image panelImage;
    [SerializeField] GameObject _centerTextObj;
    [SerializeField] TextMeshProUGUI centerText;
    [SerializeField] public TextMeshProUGUI prog_text;
    [SerializeField] TextMeshProUGUI underText;

    [Header("�|�[�Y���j���[")]
    [SerializeField] Canvas pauseCanvas;

    [Header("�I�v�V����")]
    [SerializeField] Option _option;
    [SerializeField] GameObject _firstButton;

    [Header("�X�^�[�g���p�^�C�}�[")]
    [SerializeField] float countDown;

    float panelAlpha = 0.0f;
    float waitTimer = 2.0f;
    Animator _animator;

    string _fileLocation;
    int _hiScore_Point;
    int _hiScore_Floor;
    HiScore _hiScore;


    enum State
    {
        Ready,
        Play,
        GameOver,
        Pause
    }
    State state;

    UIControls uiCtrl;

    private void Awake()
    {
        uiCtrl = new UIControls();

        _animator = _centerTextObj.GetComponent<Animator>();
    }

    void Start()
    {
        // �|�[�YUI���B��
        pauseCanvas.enabled = false;

        // �X�e�[�g������
        S_Ready();
        // �̂��ɂ������邩�A�e�L�����̃X�e�[�g��M��悤�ɂ��邩�B
        // state = State.Ready;

        uiCtrl.Enable();
    }

    void Update()
    {
        switch (state)
        {
            case State.Ready:
                S_Ready_CountDown();
                break;

            case State.Play:
                if (uiCtrl.InGameUI.Pause.triggered) { S_Pause(); }
                if (playerCtrl.state == PlayerCTRL.State.Dead) { S_GameOver(); }
                break;

            case State.GameOver:
                GameOverUpdate();
                break;

            case State.Pause:
                if (uiCtrl.InGameUI.Pause.triggered) { S_Pause_End(); }
                break;
        }
    }



    // �X�e�[�g�p
    // ���������� ���������� ���������� ���������� ���������� //

    // ���f�B
    void S_Ready()
    {
        state = State.Ready;
        DoEnableFalse();
    }

    // �J�E���g�_�E��
    void S_Ready_CountDown()
    {
        if (countDown >= 0)
        {
            upperPanel.SetActive(true);
            centerText.text = "Ready...";

            countDown -= Time.deltaTime;
        }
        else
        {
            upperPanel.SetActive(false);
            centerText.text = "";

            countDown = 0;

            S_Play();
        }
    }

    // �v���C
    void S_Play()
    {
        playerCtrl.state = PlayerCTRL.State.Alive;

        state = State.Play;

        DoEnableTrue();
    }

    // �Q�[���I�[�o�[
    public void S_GameOver()
    {
        upperPanel.SetActive(true);
        centerText.text = "GameOver";
        centerText.fontStyle = FontStyles.Underline;

        state = State.GameOver;

        _animator.SetBool("GameOver", true);

        DoEnableFalse();
    }

    void GameOverUpdate()
    {
        if (waitTimer > 0) { waitTimer -= Time.deltaTime; }
        else
        {
            if (panelAlpha < 1.0f) { panelAlpha += Time.deltaTime; }
            panelImage.color = new Color(0, 0, 0, panelAlpha);
            underText.alpha = panelAlpha;


            if (uiCtrl.InGameUI.Retry.triggered) { ReLoad(); }
            if (uiCtrl.InGameUI.ToMenu.triggered) { ReturnMainMenu(); }
        }
    }

    // �|�[�Y
    void S_Pause()
    {
        pauseCanvas.enabled = true;

        state = State.Pause;
        playerCtrl.state = PlayerCTRL.State.Stop;

        var padName = Gamepad.current;
        if (padName != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_firstButton);
        }

        DoEnableFalse();
    }

    // �|�[�Y����
    void S_Pause_End()
    {
        pauseCanvas.enabled = false;

        playerCtrl.state = PlayerCTRL.State.Alive;

        S_Play();
    }



    // ���̑����� //
    // ���������� ���������� ���������� ���������� ���������� //

    // �����[�h
    void ReLoad()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // ���C�����j���[�֖߂�
    void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // �̂��ɏ���
    void DoEnableTrue()
    {
        bpmCtrl.enabled = true;
        _stage.enabled = true;
    }
    void DoEnableFalse()
    {
        bpmCtrl.enabled = false;
        _stage.enabled = false;
    }



    // �|�[�YUI�p
    // ���������� ���������� ���������� ���������� ���������� //

    // UI�F�Q�[���ɖ߂�
    public void Pause_Close()
    {
        S_Pause_End();
    }

    // UI�F���X�^�[�g
    public void Pause_RestartGame()
    {
        ReLoad();
    }

    // UI�F�^�C�g���֖߂�
    public void Pause_ReturnToMainMenu()
    {
        ReturnMainMenu();
    }

    public void Pause_Option()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Instantiate(_option);
    }
}

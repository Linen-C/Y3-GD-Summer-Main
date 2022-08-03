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
    [Header("BPMコントロール")]
    [SerializeField] GC_BpmCTRL bpmCtrl;

    [Header("プレイヤー情報")]
    [SerializeField] PlayerCTRL playerCtrl;

    [Header("エリア情報")]
    [SerializeField] TutoStageManager _stage;

    [Header("UI")]
    [SerializeField] GameObject upperPanel;
    [SerializeField] Image panelImage;
    [SerializeField] GameObject _centerTextObj;
    [SerializeField] TextMeshProUGUI centerText;
    [SerializeField] public TextMeshProUGUI prog_text;
    [SerializeField] TextMeshProUGUI underText;

    [Header("ポーズメニュー")]
    [SerializeField] Canvas pauseCanvas;

    [Header("オプション")]
    [SerializeField] Option _option;
    [SerializeField] GameObject _firstButton;

    [Header("スタート時用タイマー")]
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
        // ポーズUIを隠す
        pauseCanvas.enabled = false;

        // ステート初期化
        S_Ready();
        // のちにこうするか、各キャラのステートを弄るようにするか。
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



    // ステート用
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // レディ
    void S_Ready()
    {
        state = State.Ready;
        DoEnableFalse();
    }

    // カウントダウン
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

    // プレイ
    void S_Play()
    {
        playerCtrl.state = PlayerCTRL.State.Alive;

        state = State.Play;

        DoEnableTrue();
    }

    // ゲームオーバー
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

    // ポーズ
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

    // ポーズ解除
    void S_Pause_End()
    {
        pauseCanvas.enabled = false;

        playerCtrl.state = PlayerCTRL.State.Alive;

        S_Play();
    }



    // その他処理 //
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // リロード
    void ReLoad()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // メインメニューへ戻る
    void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // のちに消す
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



    // ポーズUI用
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //

    // UI：ゲームに戻る
    public void Pause_Close()
    {
        S_Pause_End();
    }

    // UI：リスタート
    public void Pause_RestartGame()
    {
        ReLoad();
    }

    // UI：タイトルへ戻る
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaCTRL : MonoBehaviour
{

    [Header("最大・現在ウェーブ数(オート)")]
    [SerializeField] int max_Wave;
    [SerializeField] public int now_Wave;
    [Header("BPMコントロール(オート)")]
    [SerializeField] GC_BpmCTRL bpmCTRL;   // メトロノーム受け取り用
    [SerializeField] GC_GameCTRL gameCTRL;
    [Header("敵管理(マニュアル)")]
    [SerializeField] ArenaEnemyCTRL enemyCtrl;
    [Header("ゲート(マニュアル)")]
    [SerializeField] GateCTRL gate_N;
    [SerializeField] GateCTRL gate_S;
    [Header("インターバル用")]
    [SerializeField] int interval_MaxCount_Nomal;
    [SerializeField] int interval_MaxCount_Clear;
    [SerializeField] int interval_NowCount;
    [SerializeField] bool doInterval_Nomal;
    [SerializeField] bool doInterval_Clear;
    [Header("テキスト表示(オート)")]
    [SerializeField] TextMeshProUGUI prog_text;
    [Header("表示テキスト")]
    [SerializeField] string text_nomal1;
    [SerializeField] string text_nomal2;
    [SerializeField] string text_nomal3;
    [SerializeField] string text_nomal4;
    [SerializeField] string text_clear1;
    [SerializeField] string text_clear2;
    [Header("終点か")]
    [SerializeField] public bool isEndStage;
    [Header("ウェーブ中か")]
    [SerializeField] public bool inWave = false;

    // オーディオ関係
    [Header("オーディオ")]
    [SerializeField] AudioCTRL _audioCTRL;
    [SerializeField] AudioSource audioSource;   // オーディオソース
    [SerializeField] AudioClip[] audioClip;     // クリップ


    void Start()
    {
        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        bpmCTRL = gameCtrlObj.GetComponent<GC_BpmCTRL>();
        gameCTRL = gameCtrlObj.GetComponent<GC_GameCTRL>();

        
        // オーディオコントロール取得
        var audioCtrlObj = GameObject.FindGameObjectWithTag("AudioController");
        _audioCTRL = audioCtrlObj.GetComponent<AudioCTRL>();
        // オーディオ初期化
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = _audioCTRL.defVolume;
        audioClip = new AudioClip[_audioCTRL.clips_Progress.Length];
        audioClip = _audioCTRL.clips_Progress;

        prog_text = gameCTRL.prog_text;
        prog_text.text = " ";

        max_Wave = enemyCtrl.spawn_pattern_obj.Length;

        enabled = false;
    }

    void Update()
    {
        if (inWave)
        {
            if (enemyCtrl.DoEnemyAllDestroy())
            {
                inWave = false;
                //Debug.Log("ウェーブ進行");
                ProgressCheck();
            }
        }
        if (doInterval_Nomal)
        {
            Interval_Nomal();
        }
        if (doInterval_Clear)
        {
            Interval_Clear();
        }
    }


    public void Entry()
    {
        enabled = true;
        gate_S.GateClose();
        ProgressCheck();
    }

    void Interval_Nomal()
    {
        if (bpmCTRL.Metronome() || bpmCTRL.Step())
        {
            interval_NowCount++;
            switch (interval_NowCount)
            {
                case 1:
                    prog_text.text = text_nomal1;
                    audioSource.PlayOneShot(audioClip[0]);
                break;
                case 2:
                    prog_text.text = text_nomal2;
                    audioSource.PlayOneShot(audioClip[0]);
                    break;
                case 3:
                    prog_text.text = text_nomal3;
                    audioSource.PlayOneShot(audioClip[0]);
                    break;
                case 4:
                    prog_text.text = text_nomal4 + now_Wave.ToString();
                    audioSource.PlayOneShot(audioClip[2]);
                    break;
                default:
                    break;
            }
        }

        if (interval_NowCount > interval_MaxCount_Nomal)
        {
            prog_text.text = " ";
            enemyCtrl.WavaStart();

            doInterval_Nomal = false;
            inWave = true;
        }
    }

    void Interval_Clear()
    {
        if (bpmCTRL.Metronome() || bpmCTRL.Step())
        {
            interval_NowCount++;
            switch (interval_NowCount)
            {
                case 1:
                    prog_text.text = text_clear1;
                    audioSource.PlayOneShot(audioClip[1]);
                    break;
                case 2:
                    prog_text.text = text_clear2;
                    audioSource.PlayOneShot(audioClip[2]);
                    break;
            }
        }

        if (interval_NowCount > interval_MaxCount_Clear)
        {
            prog_text.text = " ";
            ArenaClear();
        }
    }

    void ProgressCheck()
    {
        interval_NowCount = 0;

        if (now_Wave == max_Wave)
        {
            doInterval_Clear = true;
        }
        else 
        {
            now_Wave++;
            doInterval_Nomal = true;
        }

    }

    void ArenaClear()
    {
        //Debug.Log("殲滅");
        gate_N.GateOpen();
        enabled = false;
    }
}

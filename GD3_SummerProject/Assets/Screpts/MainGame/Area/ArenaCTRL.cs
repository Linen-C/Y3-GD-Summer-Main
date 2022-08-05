using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaCTRL : MonoBehaviour
{

    [Header("最大・現在ウェーブ数(オート)")]
    [SerializeField] int _max_Wave;
    [SerializeField] public int _now_Wave;
    [Header("BPMコントロール(オート)")]
    [SerializeField] GC_BpmCTRL _bpmCTRL;   // メトロノーム受け取り用
    [SerializeField] GC_GameCTRL _gameCTRL;
    [Header("敵管理(マニュアル)")]
    [SerializeField] ArenaEnemyCTRL _enemyCtrl;
    [Header("ゲート(マニュアル)")]
    [SerializeField] GateCTRL _gate_N;
    [SerializeField] GateCTRL _gate_S;
    [Header("インターバル用")]
    [SerializeField] int _interval_MaxCount_Nomal = 4;
    [SerializeField] int _interval_MaxCount_Clear = 2;
    [SerializeField] int _interval_NowCount;
    [SerializeField] bool _doInterval_Nomal;
    [SerializeField] bool _doInterval_Clear;
    [Header("テキスト表示(オート)")]
    [SerializeField] TextMeshProUGUI _prog_text;
    [Header("表示テキスト")]
    [SerializeField] string text_nomal1;
    [SerializeField] string text_nomal2;
    [SerializeField] string text_nomal3;
    [SerializeField] string text_nomal4;
    [SerializeField] string text_clear1;
    [SerializeField] string text_clear2;
    [Header("終点か")]
    [SerializeField] public bool _isEndStage;
    [Header("ウェーブ中か")]
    [SerializeField] public bool _inWave = false;

    // オーディオ関係
    [Header("オーディオ")]
    [SerializeField] MainAudioCTRL _audioCTRL;
    [SerializeField] AudioSource _audioSource;   // オーディオソース
    [SerializeField] AudioClip[] _audioClip;     // クリップ


    void Start()
    {
        // bpmCTRLとgameCTRL取得
        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        _bpmCTRL = gameCtrlObj.GetComponent<GC_BpmCTRL>();
        _gameCTRL = gameCtrlObj.GetComponent<GC_GameCTRL>();
        

        // オーディオコントロール取得
        var audioCtrlObj = GameObject.FindGameObjectWithTag("AudioController");
        _audioCTRL = audioCtrlObj.GetComponent<MainAudioCTRL>();
        // オーディオ初期化
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _audioCTRL.nowVolume;
        _audioClip = new AudioClip[_audioCTRL.clips_Progress.Length];
        _audioClip = _audioCTRL.clips_Progress;


        // テキスト関係の初期化
        _prog_text = _gameCTRL.prog_text;
        _prog_text.text = " ";

        // 最大ウェーブ設定
        _max_Wave = _enemyCtrl.spawn_pattern_obj.Length;

        enabled = false;
    }

    void Update()
    {
        if (_inWave)
        {
            if (_enemyCtrl.DoEnemyAllDestroy())
            {
                _inWave = false;
                //Debug.Log("ウェーブ進行");
                ProgressCheck();
            }
        }
        if (_doInterval_Nomal)
        {
            Interval_Nomal();
        }
        if (_doInterval_Clear)
        {
            Interval_Clear();
        }
    }


    public void Entry()
    {
        enabled = true;
        _gate_S.GateClose();
        ProgressCheck();
    }

    void Interval_Nomal()
    {
        if (_bpmCTRL.Metronome() || _bpmCTRL.Step())
        {
            _interval_NowCount++;
            switch (_interval_NowCount)
            {
                case 1:
                    _prog_text.text = text_nomal1;
                    _audioSource.PlayOneShot(_audioClip[0]);
                break;
                case 2:
                    _prog_text.text = text_nomal2;
                    _audioSource.PlayOneShot(_audioClip[0]);
                    break;
                case 3:
                    _prog_text.text = text_nomal3;
                    _audioSource.PlayOneShot(_audioClip[0]);
                    break;
                case 4:
                    _prog_text.text = text_nomal4 + _now_Wave.ToString();
                    _audioSource.PlayOneShot(_audioClip[2]);
                    break;
                default:
                    break;
            }
        }

        if (_interval_NowCount > _interval_MaxCount_Nomal)
        {
            _prog_text.text = " ";
            _enemyCtrl.WavaStart();

            _doInterval_Nomal = false;
            _inWave = true;
        }
    }

    void Interval_Clear()
    {
        if (_bpmCTRL.Metronome() || _bpmCTRL.Step())
        {
            _interval_NowCount++;
            switch (_interval_NowCount)
            {
                case 1:
                    _prog_text.text = text_clear1;
                    _audioSource.PlayOneShot(_audioClip[1]);
                    break;
                case 2:
                    _prog_text.text = text_clear2;
                    _audioSource.PlayOneShot(_audioClip[2]);
                    break;
            }
        }

        if (_interval_NowCount > _interval_MaxCount_Clear)
        {
            _prog_text.text = " ";
            ArenaClear();
        }
    }

    void ProgressCheck()
    {
        _interval_NowCount = 0;

        if (_now_Wave == _max_Wave)
        {
            _doInterval_Clear = true;
        }
        else 
        {
            _now_Wave++;
            _doInterval_Nomal = true;
        }

    }

    void ArenaClear()
    {
        //Debug.Log("殲滅");
        _gate_N.GateOpen();
        enabled = false;
    }
}

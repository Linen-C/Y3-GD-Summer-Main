using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaCTRL : MonoBehaviour
{

    [Header("�ő�E���݃E�F�[�u��(�I�[�g)")]
    [SerializeField] int _max_Wave;
    [SerializeField] public int _now_Wave;
    [Header("BPM�R���g���[��(�I�[�g)")]
    [SerializeField] GC_BpmCTRL _bpmCTRL;   // ���g���m�[���󂯎��p
    [SerializeField] GC_GameCTRL _gameCTRL;
    [Header("�G�Ǘ�(�}�j���A��)")]
    [SerializeField] ArenaEnemyCTRL _enemyCtrl;
    [Header("�Q�[�g(�}�j���A��)")]
    [SerializeField] GateCTRL _gate_N;
    [SerializeField] GateCTRL _gate_S;
    [Header("�C���^�[�o���p")]
    [SerializeField] int _interval_MaxCount_Nomal = 4;
    [SerializeField] int _interval_MaxCount_Clear = 2;
    [SerializeField] int _interval_NowCount;
    [SerializeField] bool _doInterval_Nomal;
    [SerializeField] bool _doInterval_Clear;
    [Header("�e�L�X�g�\��(�I�[�g)")]
    [SerializeField] TextMeshProUGUI _prog_text;
    [Header("�\���e�L�X�g")]
    [SerializeField] string text_nomal1;
    [SerializeField] string text_nomal2;
    [SerializeField] string text_nomal3;
    [SerializeField] string text_nomal4;
    [SerializeField] string text_clear1;
    [SerializeField] string text_clear2;
    [Header("�I�_��")]
    [SerializeField] public bool _isEndStage;
    [Header("�E�F�[�u����")]
    [SerializeField] public bool _inWave = false;

    // �I�[�f�B�I�֌W
    [Header("�I�[�f�B�I")]
    [SerializeField] MainAudioCTRL _audioCTRL;
    [SerializeField] AudioSource _audioSource;   // �I�[�f�B�I�\�[�X
    [SerializeField] AudioClip[] _audioClip;     // �N���b�v


    void Start()
    {
        // bpmCTRL��gameCTRL�擾
        var gameCtrlObj = GameObject.FindGameObjectWithTag("GameController");
        _bpmCTRL = gameCtrlObj.GetComponent<GC_BpmCTRL>();
        _gameCTRL = gameCtrlObj.GetComponent<GC_GameCTRL>();
        

        // �I�[�f�B�I�R���g���[���擾
        var audioCtrlObj = GameObject.FindGameObjectWithTag("AudioController");
        _audioCTRL = audioCtrlObj.GetComponent<MainAudioCTRL>();
        // �I�[�f�B�I������
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _audioCTRL.nowVolume;
        _audioClip = new AudioClip[_audioCTRL.clips_Progress.Length];
        _audioClip = _audioCTRL.clips_Progress;


        // �e�L�X�g�֌W�̏�����
        _prog_text = _gameCTRL.prog_text;
        _prog_text.text = " ";

        // �ő�E�F�[�u�ݒ�
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
                //Debug.Log("�E�F�[�u�i�s");
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
        //Debug.Log("�r��");
        _gate_N.GateOpen();
        enabled = false;
    }
}

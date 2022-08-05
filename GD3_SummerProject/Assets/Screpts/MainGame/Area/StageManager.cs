using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    [Header("���݂̃G���A�i���o�[")]
    [SerializeField] public int _nowArenaNo;
    [Header("�G���A���X�g(�}�j���A��)")]
    [SerializeField] GameObject[] _arenas;
    [Header("���݂̃G���A(�I�[�g)")]
    [SerializeField] GameObject _cloned;
    [Header("�ҋ@���")]
    [SerializeField] float _defWaitTime;
    [SerializeField] float _nowWaitTime;
    [SerializeField] bool _wait = false;
    [Header("UI(�}�j���A��)")]
    [SerializeField] Animator _panelAnimator;
    [SerializeField] TextMeshProUGUI _text_NowFloor;
    [SerializeField] TextMeshProUGUI _text_Floor;
    [Header("�g�����X�t�H�[��(�}�j���A��)")]
    [SerializeField] Transform _entryPoint;
    [SerializeField] Transform _player;
    [SerializeField] Transform _gameController;
    [Header("�R���|�[�l���g(�}�j���A��)")]
    [SerializeField] Portal _portal;
    [Header("�R���|�[�l���g(�I�[�g)")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] GC_GameCTRL _gameCTRL;
    [SerializeField] GC_BpmCTRL _bpmCTRL;


    void Start()
    {
        _panelAnimator.SetBool("Close", false);
        
        _playerCTRL = _player.GetComponent<PlayerCTRL>();
        _gameCTRL = _gameController.GetComponent<GC_GameCTRL>();
        _bpmCTRL = _gameController.GetComponent<GC_BpmCTRL>();

        _text_Floor.text = "Floor�F1";

        SetArena();
    }

    void Update()
    {
        // ��ʐ؂�ւ��I��
        if (WaitFlip() && _wait)
        {
            SetArena();

            _text_Floor.text = "Floor�F" + (_nowArenaNo + 1).ToString();

            _bpmCTRL.ChangePause(false);

            _panelAnimator.SetBool("Close", false);

            _player.position = _entryPoint.position;
            _playerCTRL.state = PlayerCTRL.State.Alive;

            _wait = false;
        }
    }


    void SetArena()
    {
        // ���X�g���烉���_������
        int nextStage = Random.Range(0, _arenas.Length);
        _cloned = Instantiate(_arenas[nextStage], transform);
    }

    public void GetSignal()
    {
        // �K�w�̕ύX�Ƃ��A�E�F�C�g��ʕ\���Ƃ��A��ʐ؂�ւ��J�n�Ƃ�
        Destroy(_cloned);
        _nowArenaNo++;

        _text_NowFloor.text = (_nowArenaNo + 1).ToString();

        _bpmCTRL.ChangePause(true);

        _panelAnimator.SetBool("Close", true);
        _wait = true;
        _nowWaitTime = _defWaitTime;
        _playerCTRL.state = PlayerCTRL.State.Stop;
    }

    bool WaitFlip()
    {
        // �E�F�C�g��ʐ؂�ւ�
        if (0 < _nowWaitTime)
        {
            _nowWaitTime -= Time.deltaTime;
            return false;
        }
        return true;
    }
}

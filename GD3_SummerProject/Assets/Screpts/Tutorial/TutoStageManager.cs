using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutoStageManager : MonoBehaviour
{

    [Header("���݂̃t���A�i���o�[")]
    [SerializeField] public int _nowFloorNo;
    [SerializeField] public int _nowAreaNo;
    [Header("�G���A���X�g(�}�j���A��)")]
    [SerializeField] GameObject[] _arenas;
    [Header("���݂̃G���A(�I�[�g)")]
    [SerializeField] GameObject _cloned;
    [SerializeField] TutoArenaCTRL _arenaCTRL;
    [Header("�ҋ@���")]
    [SerializeField] float _defWaitTime;
    [SerializeField] float _nowWaitTime;
    [SerializeField] bool _wait = false;
    [Header("UI(�}�j���A��)")]
    [SerializeField] Animator _panelAnimator;
    [SerializeField] TextMeshProUGUI _text_NowFloor;
    [SerializeField] GameObject _gunUI;
    bool _gunUiActive = false;
    [Header("�g�����X�t�H�[��(�}�j���A��)")]
    [SerializeField] Transform _entryPoint;
    [SerializeField] Transform _player;
    [SerializeField] Transform _gameController;
    [Header("�R���|�[�l���g(�}�j���A��)")]
    [SerializeField] TutoPortal _portal;
    [Header("�R���|�[�l���g(�I�[�g)")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] PlayerAttack_B _playerAttack;
    [SerializeField] TU_GameCTRL _gameCTRL;
    [SerializeField] GC_BpmCTRL _bpmCTRL;


    void Start()
    {
        _panelAnimator.SetBool("Close", false);

        _playerCTRL = _player.GetComponent<PlayerCTRL>();
        _playerAttack = _player.GetComponent<PlayerAttack_B>();
        _gameCTRL = _gameController.GetComponent<TU_GameCTRL>();
        _bpmCTRL = _gameController.GetComponent<GC_BpmCTRL>();

        _gunUI.gameObject.SetActive(false);

        SetArena();
        _arenaCTRL = _cloned.GetComponent<TutoArenaCTRL>();
        _nowAreaNo = 1;
    }

    void Update()
    {
        if (WaitFlip() && _wait)
        {
            SetArena();

            _bpmCTRL.ChangePause(false);

            _panelAnimator.SetBool("Close", false);

            _player.position = _entryPoint.position;
            _playerCTRL.state = PlayerCTRL.State.Alive;
            _wait = false;
        }

        _nowAreaNo = _arenaCTRL.now_Wave;

        if (_nowAreaNo == 4)
        {
            _playerAttack._nowGunCharge = _playerAttack._needGunCharge;
            if (!_gunUiActive)
            {
                _gunUI.gameObject.SetActive(true);
                _gunUiActive = true;
            }
        }

    }


    void SetArena()
    {
        _cloned = Instantiate(_arenas[_nowFloorNo], transform);
    }

    public void GetSignal()
    {
        _gameCTRL.S_GameOver();

        //Destroy(_cloned);
        //_nowFloorNo++;

        //_text_NowFloor.text = (_nowFloorNo + 1).ToString();

        //_bpmCTRL.ChangePause(true);

        //if (_nowFloorNo > _arenas.Length)
        //{
            
        //    return;
        //}

        //_panelAnimator.SetBool("Close", true);
        //_wait = true;
        //_nowWaitTime = _defWaitTime;
        //_playerCTRL.state = PlayerCTRL.State.Stop;
    }

    bool WaitFlip()
    {
        if (0 < _nowWaitTime)
        {
            _nowWaitTime -= Time.deltaTime;
            return false;
        }
        return true;
    }
}

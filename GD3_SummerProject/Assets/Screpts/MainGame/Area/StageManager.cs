using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    [Header("現在のエリアナンバー")]
    [SerializeField] public int _nowArenaNo;
    [Header("エリアリスト(マニュアル)")]
    [SerializeField] GameObject[] _arenas;
    [Header("現在のエリア(オート)")]
    [SerializeField] GameObject _cloned;
    [Header("待機画面")]
    [SerializeField] float _defWaitTime;
    [SerializeField] float _nowWaitTime;
    [SerializeField] bool _wait = false;
    [Header("UI(マニュアル)")]
    [SerializeField] Animator _panelAnimator;
    [SerializeField] TextMeshProUGUI _text_NowFloor;
    [SerializeField] TextMeshProUGUI _text_Floor;
    [Header("トランスフォーム(マニュアル)")]
    [SerializeField] Transform _entryPoint;
    [SerializeField] Transform _player;
    [SerializeField] Transform _gameController;
    [Header("コンポーネント(マニュアル)")]
    [SerializeField] Portal _portal;
    [Header("コンポーネント(オート)")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] GC_GameCTRL _gameCTRL;
    [SerializeField] GC_BpmCTRL _bpmCTRL;


    void Start()
    {
        _panelAnimator.SetBool("Close", false);
        
        _playerCTRL = _player.GetComponent<PlayerCTRL>();
        _gameCTRL = _gameController.GetComponent<GC_GameCTRL>();
        _bpmCTRL = _gameController.GetComponent<GC_BpmCTRL>();

        _text_Floor.text = "Floor：1";

        SetArena();
    }

    void Update()
    {
        // 画面切り替え終了
        if (WaitFlip() && _wait)
        {
            SetArena();

            _text_Floor.text = "Floor：" + (_nowArenaNo + 1).ToString();

            _bpmCTRL.ChangePause(false);

            _panelAnimator.SetBool("Close", false);

            _player.position = _entryPoint.position;
            _playerCTRL.state = PlayerCTRL.State.Alive;

            _wait = false;
        }
    }


    void SetArena()
    {
        // リストからランダム生成
        int nextStage = Random.Range(0, _arenas.Length);
        _cloned = Instantiate(_arenas[nextStage], transform);
    }

    public void GetSignal()
    {
        // 階層の変更とか、ウェイト画面表示とか、画面切り替え開始とか
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
        // ウェイト画面切り替え
        if (0 < _nowWaitTime)
        {
            _nowWaitTime -= Time.deltaTime;
            return false;
        }
        return true;
    }
}

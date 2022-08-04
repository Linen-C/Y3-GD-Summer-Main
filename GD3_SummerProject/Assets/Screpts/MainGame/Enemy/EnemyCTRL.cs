using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCTRL : MonoBehaviour
{
    [Header("�`���[�g���A���p")]
    [SerializeField] int _damageCap = 0;
    [SerializeField] bool _kockBackResist = false;

    [Header("���_")]
    [SerializeField] int _havePoint = 100;

    [Header("�X�e�[�^�X")]
    [SerializeField] int maxHelthPoint;      // �ő�̗�
    [SerializeField] int nowHelthPoint = 0;  // ���ݑ̗�
    [SerializeField] int maxStan = 100;      // �ő�X�^���l
    [SerializeField] int nowStan;            // ���݃X�^���l
    [SerializeField] int doStanCount;        // �X�^���񕜂܂ł̃e���|��
    [SerializeField] int stanRecoverCount;   // �X�^���񕜂܂ł̃f�t�H���g�e���|��

    [Header("����")]
    [SerializeField] public int _nowWeaponCharge = 1;  // ���݃N�[���_�E��
    [SerializeField] int _needWeaponCharge;            // �K�v�N�[���_�E��

    [Header("�������U���̗L��")]
    [SerializeField] bool shootingType = false; // �������U���^�C�v��

    [Header("�m�b�N�o�b�N�Ɩ��G����")]
    [SerializeField] float knockBackPower;    // ������m�b�N�o�b�N�̋���
    [SerializeField] float defNonDamageTime = 0.2f;  // �f�t�H���g���G����4

    [Header("�Q�[���I�u�W�F�N�g(�����擾)")]
    [SerializeField] GameObject _player;    // �v���C���[

    [Header("�R���|�[�l���g")]
    // �O��
    [SerializeField] EnemyWepon _ownWepon;  // ��������
    [SerializeField] PlayerAttack_B _playerAttack;
    [SerializeField] StageManager _stageManager;  // �e�̃G���A
    [SerializeField] GC_GameCTRL _gameCTRL;       // �Q�[���R���g���[���[
    [SerializeField] GC_BpmCTRL _bpmCTRL;         // ���g���m�[��
    // ����
    [SerializeField] EnemyMove _enemyMove;
    [SerializeField] EnemyRotation _enemyRotation;
    [SerializeField] EnemyAttack _enemyAttack;
    [SerializeField] Renderer _renderer;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Rigidbody2D _body;
    [SerializeField] Animator _anim;

    [Header("�̗͕\��(�}�j���A��)")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider stanBar;
    [SerializeField] Image stanBarFill;

    [Header("�I�[�f�B�I(�}�j���A��)")]
    [SerializeField] MainAudioCTRL _audioCTRL;
    [SerializeField] AudioSource audioSource;   // �I�[�f�B�I�\�[�X
    [SerializeField] AudioClip audioClip;

    // �v���C�x�[�g�ϐ�
    float knockBackCounter = 0;  // �m�b�N�o�b�N���ԃJ�E���^�[
    float NonDamageTime = 0;     // ���G����
    Vector2 _diff;                // �v���C���[�̕���

    public enum State
    {
        Stop,
        Alive,
        Stan,
        Dead
    }
    State _state;


    void Awake()
    {
        // �R���|�[�l���g�擾
        // �O��
        _ownWepon = GetComponentInChildren<EnemyWepon>();
        // ����
        TryGetComponent(out _enemyMove);
        TryGetComponent(out _enemyRotation);
        TryGetComponent(out _enemyAttack);
        TryGetComponent(out _renderer);
        TryGetComponent(out _sprite);
        TryGetComponent(out _body);
        TryGetComponent(out _anim);
    }

    void Start()
    {
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;
        stanBar.value = 1;

        if (!shootingType) { _enemyAttack.SetBulletNull(); }

        // �e�G���A�R���|�[�l���g�̎擾
        var areaObj = transform.parent.parent.parent.parent.gameObject;
        _stageManager = areaObj.GetComponent<StageManager>();

        // �^�O�����擾
        // _gameCTRL��_bpmCTRL
        _gameCTRL = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC_GameCTRL>();
        _bpmCTRL = _gameCTRL.GetComponent<GC_BpmCTRL>();
        // _player��_PlayerAttack
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAttack = _player.GetComponent<PlayerAttack_B>();

        // �I�[�f�B�I������
        audioSource = GetComponent<AudioSource>();
        _audioCTRL = GameObject.FindGameObjectWithTag("AudioController").GetComponent<MainAudioCTRL>();
        audioSource.volume = _audioCTRL.nowVolume;
        audioClip = _audioCTRL.clips_Damage[0];

        // �X�e�[�g������
        _state = State.Stop;
    }

    void Update()
    {
        // ��~����
        if (!_stageManager.enabled)
        {
            _state = State.Stop;
            _anim.SetBool("Moving", false);
        }
        else if (_state != State.Dead)
        {
            _state = State.Alive;
            _anim.SetBool("Moving", true);
        }

        // �̗͕\���Z�b�g
        SetHP();

        // ���S����
        if (!IfIsAlive()) { return; }
        else { _anim.SetBool("Alive", true); }

        // ���G����
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // �X�^������
        if (!Stan()) { return; }


        // ����
        _diff = _enemyRotation.TracePlayer(_player);  // �v���C���[�⑫�E�ǐ�
        _enemyRotation.CursorRot(_nowWeaponCharge, _needWeaponCharge, _ownWepon.attakingTime, _diff, _sprite);    // ����
        _enemyAttack.Attack(_needWeaponCharge, shootingType, _bpmCTRL, _renderer, _ownWepon, _anim);       // �U��
    }

    void FixedUpdate()
    {
        // �X�e�[�g����
        knockBackCounter = _enemyMove.KnockBack(knockBackCounter, _body, _diff, knockBackPower);

        if (!_enemyMove.CanMove(_state, _nowWeaponCharge, _needWeaponCharge,_body)) { return; }

        _enemyMove.Move(knockBackCounter, doStanCount, _body, _diff);
    }



    // �X�^��
    bool Stan()
    {
        if (maxStan == -1) { return true; }

        if (nowStan >= maxStan)
        {
            doStanCount = stanRecoverCount;
            _nowWeaponCharge = 0;
            nowStan = 0;
        }

        if (doStanCount > 0)
        {
            if (_bpmCTRL.Count()) { doStanCount--; }
            return false;
        }
        else if (nowStan > 0)
        {
            if (_bpmCTRL.Count()) { nowStan -= 5; }
        }

        if (nowStan < 0) { nowStan = 0; }

        return true;
    }

    // ���S����
    bool IfIsAlive()
    {
        if (_state == State.Dead) { return false; }

        if (nowHelthPoint <= 0 && _state != State.Dead)
        {
            _state = State.Dead;
            _playerAttack.GetCharge();
            Destroy(gameObject, defNonDamageTime + 0.1f);

            _gameCTRL.AddPoint(_havePoint);

            return false;
        }
        return true;
    }

    // �̗͕\��
    void SetHP()
    {
        hpSlider.value = (float)nowHelthPoint / (float)maxHelthPoint;
        if (doStanCount > 0)
        {
            stanBar.value = stanBar.maxValue;
            stanBarFill.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else
        {
            stanBar.value = (float)nowStan / (float)maxStan;
            stanBarFill.color = new Color(1.0f, 1.0f, 0.0f);
        }
    }


    // �_���[�W���󂯂�
    public void TakeDamage(int damage,int knockback, int stanPower,int typeNum)
    {
        if (NonDamageTime > 0) { return; }
        if (_damageCap >= damage) { damage = 0; }

        nowHelthPoint -= damage;
        knockBackPower = knockback;
        if (_kockBackResist) { knockBackPower = 0; }

        if (doStanCount <= 0) { nowStan += stanPower; }

        NonDamageTime = defNonDamageTime;
        knockBackCounter = 0.1f;

        audioSource.PlayOneShot(audioClip);
        _anim.SetTrigger("Damage");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCTRL : MonoBehaviour
{
    [Header("�X�e�[�^�X")]
    [SerializeField] int _maxHelthPoint;      // �ő�̗�
    [SerializeField] int _nowHelthPoint = 0;  // ���ݑ̗�
    [SerializeField] public float _defNonDamageTime = 0.5f;  // �f�t�H���g���G����

    [Header("�Q�[���I�u�W�F�N�g(�}�j���A��)")]
    [SerializeField] GameObject _flashObj;  // �t���b�V���p
    [SerializeField] GameObject _gameCTRL;  // ���낢�����Ă���p

    [Header("�R���|�[�l���g(�I�[�g)")]
    // �O��
    [SerializeField] GC_BpmCTRL _bpmCTRL;             // BPM�R���g���[���[
    [SerializeField] EquipLoad _equipLoad;            // ��������擾
    [SerializeField] PlayerWeapon_B _playerWeapon;    // ����
    [SerializeField] public Animator _flashAnim;      // �t���b�V���A�j���[�V����
    [SerializeField] PlayerControls _playerControls;  // �R���g���[���[
    // ����
    [SerializeField] PlayerMove _playerMove;          // �ړ�
    [SerializeField] PlayerRotation _playerRotation;  // ����
    [SerializeField] PlayerAttack_B _playerAttack;    // �U��
    [SerializeField] PlayerUI _playerUI;              // UI�\��
    [SerializeField] SpriteRenderer _sprite;          // �X�v���C�g
    [SerializeField] Rigidbody2D _body;               // 2R���W�b�h�{�f�B
    [SerializeField] public Animator _anim;           // �A�j���[�V����

    [Header("���̑��ϐ�")]
    public JsonData equipList;  // ���탊�X�g
    public int equipNo = 0;    // �������Ă��镐��ԍ�(0�`2)

    public int comboTimeLeft = 0;     // �R���{�p���J�E���^�[
    public int comboCount = 0;        // �R���{�񐔃J�E���^�[
    public bool doComboMode = false;  // �R���{���[�h

    public float knockBackCounter = 0;  // �m�b�N�o�b�N���ԃJ�E���^�[
    float NonDamageTime = 0;            // ���G����
    Vector2 _moveDir;                   // �ړ��p�x�N�g��

    public int _orFaildCount = 0;  // ���b�N����
    public bool _orFaild = false;  // ���b�N�����ǂ���
    
    public enum State
    {
        Stop,
        Alive,
        Dead
    }
    public State state;


    void Awake()
    {
        // �R���|�[�l���g�擾
        // �O��
        _bpmCTRL = _gameCTRL.transform.GetComponent<GC_BpmCTRL>();
        _equipLoad = _gameCTRL.transform.GetComponent<EquipLoad>();
        _playerWeapon = GetComponentInChildren<PlayerWeapon_B>();
        _flashAnim = _flashObj.GetComponent<Animator>();
        _playerControls = new PlayerControls();
        // ����
        TryGetComponent(out _playerMove);
        TryGetComponent(out _playerRotation);
        TryGetComponent(out _playerAttack);
        TryGetComponent(out _playerUI);
        TryGetComponent(out _sprite);
        TryGetComponent(out _body);
        TryGetComponent(out _anim);
    }

    void Start()
    {
        // �̗�(�ƕ\��)������
        _nowHelthPoint = _maxHelthPoint;
        _playerUI.HelthSetStart();

        // ���평����
        equipList = _equipLoad.GetList();
        _playerWeapon.SwapWeapon(equipList.weaponList, 0);

        // UI�n������
        _playerUI.UIUpdate(_nowHelthPoint, _maxHelthPoint, comboCount, _orFaild, _orFaildCount, _playerWeapon);

        // �X�e�[�g������
        state = State.Stop;
        _anim.SetBool("Alive", true);

        // �C���v�b�g�V�X�e���L����
        _playerControls.Enable();
    }

    void Update()
    {
        // �ړ����͂̎擾
        _moveDir = _playerControls.Player.Move.ReadValue<Vector2>();

        // UI�X�V
        _playerUI.UIUpdate(_nowHelthPoint, _maxHelthPoint, comboCount, _orFaild, _orFaildCount, _playerWeapon);

        // ���S����
        IsDead();

        // �X�e�[�g����
        if (state != State.Alive)
        {
            _anim.SetBool("Moving", false);
            _anim.SetBool("Alive", false);
            _body.velocity = new Vector2(0,0);
            return;
        }
        else { _anim.SetBool("Alive", true); }

        // ���G����
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }


        // ����n
        _playerRotation.Rotation(_playerControls, _sprite);             // ����
        // �U���n
        _playerAttack.Attack(_playerControls, _bpmCTRL, _playerWeapon);  // �U��
        _playerAttack.SwapingWeapon(_playerControls, _playerWeapon);     // �������
        _playerAttack.Shooting(_playerControls, _bpmCTRL);              // �������U��
        // �ړ��n
        _playerMove.Dash(_playerControls, _bpmCTRL);                    // �_�b�V������

    }

    void FixedUpdate()
    {
        // �X�e�[�g����
        if (state != State.Alive) { return; }

        // �ړ����f
        _playerMove.Move(_moveDir, _body);
    }


    void IsDead()
    {
        if (_nowHelthPoint <= 0) { state = State.Dead; }
    }


    // ��_���[�W����
    // ���������� ���������� ���������� ���������� ���������� //
    // �U��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "EnemyAttack" ||
            collision.gameObject.tag == "EnemyBullet") &&
            (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    // �ڐG
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    // �_���[�W���f
    private void Damage()
    {
        _playerUI.DamagePanelAlphaSet();

        NonDamageTime = _defNonDamageTime;
        knockBackCounter = 0.2f;
        _nowHelthPoint -= 1;
        _anim.SetTrigger("Damage");
    }

}

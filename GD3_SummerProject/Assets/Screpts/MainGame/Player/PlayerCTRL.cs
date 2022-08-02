using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCTRL : MonoBehaviour
{
    // �ϐ�
    [Header("�X�e�[�^�X")]
    [SerializeField] int maxHelthPoint;      // �ő�̗�
    [SerializeField] int nowHelthPoint = 0;  // ���ݑ̗�

    [Header("���G����")]
    [SerializeField] public float defNonDamageTime;  // �f�t�H���g���G����

    // �Q�[���I�u�W�F�N�g
    [Header("�Q�[���I�u�W�F�N�g(�}�j���A��)")]
    [SerializeField] GameObject flashObj;   // �t���b�V���p

    // �X�N���v�g
    [Header("�R���|�[�l���g(�}�j���A��)")]
    [SerializeField] GameObject gamectrl;  // ���낢�����Ă���p
    

    [Header("�R���|�[�l���g(�I�[�g)")]
    [SerializeField] GC_BpmCTRL _bpmCTRL;        // BPM�R���g���[���[
    [SerializeField] PlayerWeapon_B playerWeapon;  // �U���p
    [SerializeField] EquipLoad equipLoad;       // ��������擾

    [Header("�R���|�[�l���g(�}�j���A��)")]
    [SerializeField] PlayerMove _playerMove;
    [SerializeField] PlayerRotation _playerRotation;
    [SerializeField] PlayerAttack_B _playerAttack;

    // �G���W���ˑ��R���|�[�l���g
    [Header("�R���|�[�l���g(�I�[�g)")]
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Rigidbody2D _body;
    [SerializeField] public Animator _anim;
    [SerializeField] public Animator _flashAnim;
    [SerializeField] PlayerControls _playerControls;

    public JsonData equipList; // �����擾

    // �L�����p�X
    [Header("�L�����o�XUI(�}�j���A��)")]
    [SerializeField] Image image_DamagePanel;
    [SerializeField] float image_DamagePanel_defalpha = 1.0f;
    [SerializeField] float image_DamagePanel_nowalpha = 0.0f;
    [SerializeField] public TextMeshProUGUI _comboText;
    [SerializeField] public TextMeshProUGUI _resultText;
    [SerializeField] Image image_Rock;
    [SerializeField] TextMeshProUGUI text_RockCount;
    [SerializeField] Image image_half_Hit;
    [SerializeField] Image image_half_Per;

    // �̗͕\��
    [Header("�̗͕\��(�}�j���A��)")]
    [SerializeField] Slider hpSlider;

    // �v���C�x�[�g�ϐ�
    [Header("�v���C�x�[�g�ϐ�����������")]
    public int equipNo = 0;            // �������Ă��镐��ԍ�(0�`2)

    public int comboTimeLeft = 0;     // �R���{�p���J�E���^�[
    public bool doComboMode = false;  // �R���{���[�h
    public int comboCount = 0;        // �R���{�񐔃J�E���^�[

    public float knockBackCounter = 0;  // �m�b�N�o�b�N���ԃJ�E���^�[
    float NonDamageTime = 0;     // ���G����
    Vector2 _moveDir;             // �ړ��p�x�N�g��

    public bool _orFaild = false;   // ���b�N�����ǂ���
    public int _orFaildCount = 0;   // ���b�N����
    float _rockAlpha = 0.0f;
    
    public float _comboTextAlpha = 0.0f;
    float _comboImageAlpha = 0.0f;



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
        playerWeapon = GetComponentInChildren<PlayerWeapon_B>();
        _bpmCTRL = gamectrl.transform.GetComponent<GC_BpmCTRL>();
        equipLoad = gamectrl.transform.GetComponent<EquipLoad>();

        _playerMove = GetComponent<PlayerMove>();

        TryGetComponent(out _body);
        TryGetComponent(out _sprite);
        TryGetComponent(out _anim);
        _flashAnim = flashObj.GetComponent<Animator>();
        _playerControls = new PlayerControls();


        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void Start()
    {
        // �̗�(�ƕ\��)������
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;

        // ���평����
        equipList = equipLoad.GetList();
        playerWeapon.SwapWeapon(equipList.weaponList, 0);

        // UI�n������
        UIUpdate();

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
        UIUpdate();

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


        // ����
        _playerRotation.Rotation(_playerControls, _sprite);             // ����n
        _playerAttack.Attack(_playerControls, _bpmCTRL, playerWeapon);  // �U��
        _playerAttack.SwapingWeapon(_playerControls, playerWeapon);     // �������
        _playerAttack.Shooting(_playerControls, _bpmCTRL);              // �������U��
        _playerMove.Dash(_playerControls, _bpmCTRL);                    // �_�b�V������

    }

    void FixedUpdate()
    {
        // �X�e�[�g����
        if (state != State.Alive) { return; }

        _playerMove.Move(_moveDir, _body);
    }



    // UI�X�V
    // ���������� ���������� ���������� ���������� ���������� //
    void UIUpdate()
    {
        // �̗͕\��
        hpSlider.value = (float)nowHelthPoint / (float)maxHelthPoint;

        // �_���[�W�\��
        if (image_DamagePanel_nowalpha > 0.0f) { image_DamagePanel_nowalpha -= Time.deltaTime; }
        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, image_DamagePanel_nowalpha);


        // �R���{�\��
        if (comboCount == 0)
        {
            if (_comboTextAlpha > 0.0f) _comboTextAlpha -= Time.deltaTime;
            if (_comboImageAlpha > 0.0f) _comboImageAlpha-= Time.deltaTime;
        }
        else
        {
            _comboTextAlpha = 1.0f;
            _comboText.text = "x" + comboCount;

            if(playerWeapon.GetTypeNum() != 2) { _comboImageAlpha = 0.35f; }
        }
        _comboText.alpha = _comboTextAlpha;
        _resultText.alpha = _comboTextAlpha;
        image_half_Hit.color = new Color(1.0f, 0, 0, _comboImageAlpha);
        image_half_Per.color = new Color(0, 1.0f, 1.0f, _comboImageAlpha);


        // �R���{�~�X
        if (_orFaild)
        {
            _rockAlpha = 0.6f;
            text_RockCount.text = _orFaildCount.ToString();
        }
        else
        {
            if (_rockAlpha > 0.0f) { _rockAlpha -= Time.deltaTime * 5.0f;  }
        }
        text_RockCount.alpha = _rockAlpha;
        image_Rock.color = new Color(1.0f, 0.0f, 0.0f, _rockAlpha);

    }


    // ���S����
    // ���������� ���������� ���������� ���������� ���������� //
    void IsDead()
    {
        if (nowHelthPoint <= 0)
        {
            //hpText.text = "HP�F" + nowHelthPoint.ToString();
            state = State.Dead;
        }
    }



    // ��_���[�W����
    // ���������� ���������� ���������� ���������� ���������� //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "EnemyAttack" ||
            collision.gameObject.tag == "EnemyBullet") &&
            (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && (NonDamageTime <= 0.0f))
        {
            Damage();
        }
    }

    private void Damage()
    {
        image_DamagePanel_nowalpha = image_DamagePanel_defalpha;
        NonDamageTime = defNonDamageTime;
        knockBackCounter = 0.2f;
        nowHelthPoint -= 1;
        _anim.SetTrigger("Damage");
    }

}

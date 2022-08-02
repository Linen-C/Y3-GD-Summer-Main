using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCTRL : MonoBehaviour
{
    [Header("�������U���p")]
    [SerializeField] bool shootingType = false; // �������U���^�C�v��
    [SerializeField] GameObject bullet; // �e��

    [Header("�`���[�g���A���p")]
    [SerializeField] bool _kockBackResist = false;
    [SerializeField] int _damageCap = 0;

    [Header("���_")]
    [SerializeField] int _havePoint = 100;

    [Header("�X�e�[�^�X")]
    [SerializeField] int maxHelthPoint;      // �ő�̗�
    [SerializeField] int nowHelthPoint = 0;  // ���ݑ̗�
    [SerializeField] int maxStan = 100;      // �ő�X�^���l
    [SerializeField] int nowStan;            // ���݃X�^���l
    [SerializeField] int doStanCount;        // �X�^���񕜂܂ł̃e���|��
    [SerializeField] int stanRecoverCount;   // �X�^���񕜂܂ł̃f�t�H���g�e���|��

    [Header("�ړ�")]
    [SerializeField] float moveSpeed;  // �ړ����x

    [Header("����")]
    [SerializeField] int needWeaponCharge;  // �K�v�N�[���_�E��

    [Header("�m�b�N�o�b�N�Ɩ��G����")]
    [SerializeField] float knockBackPower;    // ������m�b�N�o�b�N�̋���
    [SerializeField] float defNonDamageTime = 0.3f;  // �f�t�H���g���G����

    [Header("�e�X�g")]
    [SerializeField] EnemyMove _enemyMove;

    // �X�N���v�g
    [Header("�X�N���v�g(�}�j���A��)")]
    [SerializeField] EnemyWepon ownWepon;  // ��������
    [Header("�X�N���v�g(�����擾)")]
    [SerializeField] GC_GameCTRL _gameCTRL;
    [SerializeField] GC_BpmCTRL bpmCTRL;   // ���g���m�[���󂯎��p
    [SerializeField] StageManager _stageManager;
    [SerializeField] PlayerAttack_B _playerAttack;

    // �Q�[���I�u�W�F�N�g
    [Header("�Q�[���I�u�W�F�N�g(�}�j���A��)")]
    [SerializeField] GameObject cursor;    // �J�[�\���擾(�������ꂪ��ԑ���)
    [SerializeField] GameObject cursorImage;  // �J�[�\���C���[�W(TKIH)
    [SerializeField] GameObject flashObj;   // �t���b�V���p
    [Header("�Q�[���I�u�W�F�N�g(�����擾)")]
    [SerializeField] GameObject player;    // �v���C���[
    [SerializeField] GameObject areaObj;   // �G���A�I�u�W�F�N�g
    [SerializeField] Renderer _renderer;

    // �̗͕\��
    [Header("�̗͕\��(�}�j���A��)")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider stanBar;
    [SerializeField] Image stanBarFill;

    // �v���C�x�[�g�ϐ�
    private int weaponCharge = 1;         // ���݃N�[���_�E��
    private bool coolDownReset = false;  // �N�[���_�E���̃��Z�b�g�t���O
    private Vector2 diff;                // �v���C���[�̕���
    private float knockBackCounter = 0;  // �m�b�N�o�b�N���ԃJ�E���^�[
    private float NonDamageTime = 0;     // ���G����

    // �R���|�[�l���g
    SpriteRenderer sprite;
    Rigidbody2D body;
    Animator anim;
    Animator flashAnim;
    Transform curTrans;

    public enum State
    {
        Stop,
        Alive,
        Stan,
        Dead
    }
    State state;

    void Awake()
    {
        // �R���|�[�l���g�擾
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flashAnim = flashObj.GetComponent<Animator>();
        curTrans = cursor.GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        nowHelthPoint = maxHelthPoint;
        hpSlider.value = 1;
        stanBar.value = 1;

        if (!shootingType) { bullet = null; }

        // �e�G���A�R���|�[�l���g�̎擾
        areaObj = transform.parent.parent.parent.parent.gameObject;
        _stageManager = areaObj.GetComponent<StageManager>();

        // �L������
        var GC = GameObject.FindGameObjectWithTag("GameController");
        _gameCTRL = GC.GetComponent<GC_GameCTRL>();
        bpmCTRL = GC.GetComponent<GC_BpmCTRL>();

        // �Q����g���Ƃ��Ȃ������킢�c
        player = GameObject.FindGameObjectWithTag("Player");
        _playerAttack = player.GetComponent<PlayerAttack_B>();

        // �X�e�[�g������
        state = State.Stop;
    }

    void Update()
    {
        // ��~����
        if (!_stageManager.enabled)
        {
            state = State.Stop;
            anim.SetBool("Moving", false);
        }
        else if (state != State.Dead)
        {
            state = State.Alive;
            anim.SetBool("Moving", true);
        }

        // �̗͕\���Z�b�g
        SetHP();

        // ���S����
        if (!IfIsAlive()) { return; }
        else { anim.SetBool("Alive", true); }

        // ���G����
        if (NonDamageTime > 0) { NonDamageTime -= Time.deltaTime; }

        // �X�^������
        if (!Stan()) { return; }


        // ����
        TracePlayer();  // �v���C���[�⑫�E�ǐ�
        CursorRot();    // ����
        Attack();       // �U��

    }

    void FixedUpdate()
    {
        // �X�e�[�g����
        knockBackCounter = _enemyMove.KnockBack(knockBackCounter, body, diff, knockBackPower);

        if (!CanMove()) { return; }

        _enemyMove.Move(knockBackCounter, doStanCount, body, diff, moveSpeed);
    }



    // �X�^��
    bool Stan()
    {
        if (maxStan == -1) { return true; }

        if (nowStan >= maxStan)
        {
            doStanCount = stanRecoverCount;
            weaponCharge = 0;
            nowStan = 0;
        }

        if (doStanCount > 0)
        {
            if (bpmCTRL.Count()) { doStanCount--; }
            return false;
        }
        else if (nowStan > 0)
        {
            if (bpmCTRL.Count()) { nowStan -= 5; }
        }

        if (nowStan < 0) { nowStan = 0; }

        return true;
    }

    // ���S����
    bool IfIsAlive()
    {
        if (state == State.Dead) { return false; }

        if (nowHelthPoint <= 0 && state != State.Dead)
        {
            state = State.Dead;
            _playerAttack.GetCharge();
            Destroy(gameObject, defNonDamageTime + 0.1f);

            _gameCTRL.AddPoint(_havePoint);

            return false;
        }
        return true;
    }

    bool CanMove()
    {
        if (state == State.Dead ||
            state == State.Stop ||
            needWeaponCharge == -1 ||
            weaponCharge >= (needWeaponCharge - 1) )
        {
            body.velocity = new Vector2(0, 0);
            return false;
        }
        return true;
    }

    // �v���C���[�⑫�E�ǐ�
    void TracePlayer()
    {
        // ���������� ���������� ���������� ���������� //
        // �v���C���[�����̕⑫
        // ���������� ���������� ���������� ���������� //

        // �����̈ʒu
        Vector2 transPos = transform.position;

        // �v���C���[���W
        Vector2 playerPos = player.transform.position;

        // �x�N�g�����v�Z
        diff = (playerPos - transPos).normalized;
        // ���������� ���������� ���������� ���������� //
    }

    // ����
    void CursorRot()
    {
        // ���������� ���������� ���������� ���������� //
        // �J�[�\����]
        // ���������� ���������� ���������� ���������� //

        if (weaponCharge >= (needWeaponCharge - 1) ||
            (needWeaponCharge == -1) ||
            ownWepon.attakingTime > 0)
        {
            return;
        }

        // ��]�ɑ��
        var curRot = Quaternion.FromToRotation(Vector3.up, diff);

        // �J�[�\������Ƀp�X
        curTrans.rotation = curRot;
        // ���������� ���������� ���������� ���������� //

        // �X�v���C�g���]
        if (curTrans.eulerAngles.z < 180.0f) { sprite.flipX = true; }
        else { sprite.flipX = false; }
    }

    // �U��
    void Attack()
    {
        // ���������� ���������� ���������� ���������� //
        // �U���E�N�[���_�E��
        // ���������� ���������� ���������� ���������� //

        if ((weaponCharge == needWeaponCharge) && bpmCTRL.Signal() && coolDownReset == false)
        {
            // Debug.Log("ENEMY_ATTACK");

            if (shootingType)
            {
                if (_renderer.isVisible)
                {
                    Instantiate(
                    bullet,
                    new Vector3
                    (cursorImage.transform.position.x,
                    cursorImage.transform.position.y,
                    cursorImage.transform.position.z),
                    cursor.transform.rotation);
                }
            }
            else { ownWepon.Attacking(); }

            anim.SetTrigger("Attack");
            coolDownReset = true;
        }

        if (bpmCTRL.Metronome())
        {
            if (coolDownReset == true)
            {
                //state = State.Alive;
                weaponCharge = 1;
                coolDownReset = false;
            }
            else if (weaponCharge < needWeaponCharge) { weaponCharge++; }

            if (weaponCharge == (needWeaponCharge - 1))
            {
                anim.SetTrigger("Charge");
                flashAnim.SetTrigger("FlashTrigger");
            }
        }

        // ���������� ���������� ���������� ���������� //
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
        
        anim.SetTrigger("Damage");
    }
}

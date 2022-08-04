using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    /*
    // �ϐ�
    [Header("�ϐ�")]
    [SerializeField] float defAttackingTime = 0.3f;   // �U������̊�b��������
    [SerializeField] public float nowAttakingTime = 0.0f;  // ����̔�������

    // �X�N���v�g
    [Header("�X�N���v�g")]
    [SerializeField] PlayerCTRL _playerCTRL;
    [SerializeField] SpriteChanger _spriteChanger;

    // �L�����p�X
    //[Header("�L�����o�X")]
    //[SerializeField] Text weponNameText;  // ���햼�\���p
    [Header("UI�p(�}�j���A��)")]
    [SerializeField] Image image_Wepon;
    //[SerializeField] Image image_Gun;

    // �v���C�x�[�g�ϐ�
    float spriteAlpha = 0.0f;
    float chargeCool = 0.0f;

    [Header("�p�����[�^")]
    [SerializeField] int defDamage = 1;      // �ʏ�_���[�W
    [SerializeField] int maxDamage = 0;      // �ő�_���[�W
    [SerializeField] int defKnockBack = 0;   // �m�b�N�o�b�N�p���[
    [SerializeField] int maxKnockBack = 0;   // �m�b�N�o�b�N�p���[
    [SerializeField] int maxCharge = 0;      // �K�v�ő�`���[�W
    [SerializeField] int maxStanPower = 0;      // �X�^���l

    int nowDamage = 0;
    int nowKockBack = 0;
    int nowStanPower = 0;
    bool comboFlag = false;

    // �R���|�[�l���g
    BoxCollider2D coll;


    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;

        _spriteChanger.ChangeTransparency(spriteAlpha);
    }
    
    void Update()
    {
        // ���蔭��
        // ���������� ���������� ���������� ���������� //
        if (nowAttakingTime >= 0)
        {
            nowAttakingTime -= Time.deltaTime;
        }
        else{ coll.enabled = false; }

        if (spriteAlpha > 0.0f)
        {
            _spriteChanger.ChangeTransparency(spriteAlpha);
            spriteAlpha -= Time.deltaTime * 2.0f;
        }

        if (chargeCool >= 0.0f)
        {
            chargeCool -= Time.deltaTime;
        }
        // ���������� ���������� ���������� ���������� //
    }



    // ����؂�ւ�
    // ���������� ���������� ���������� ���������� //
    public int SwapWeapon(WeaponList[] wepon,int no)
    {
        // �o�O���Ă��狭���I��0��˂�����
        if (no + 1 > wepon.Length) { no = 0; }


        // Tags
        // ���������� ���������� ���������� ���������� //
        // �e�L�X�g�ύX
        //weponNameText.text = wepon[no].name;

        // �X�v���C�g�؂�ւ��̂��߃p�X
        Sprite inImage = Resources.Load<Sprite>(wepon[no].trail);
        _spriteChanger.ChangeSprite(inImage, wepon[no].offset);

        // �����ɃA�C�R�����ǉ����邩��
        // (Empty)


        // Status
        // ���������� ���������� ���������� ���������� //
        // �ő�_���[�W
        maxDamage = wepon[no].damage;

        // ��b�m�b�N�o�b�N��
        defKnockBack = wepon[no].defknockback;

        // �ő�m�b�N�o�b�N��
        maxKnockBack = wepon[no].maxknockback;

        // �ő�`���[�W��
        maxCharge = wepon[no].maxcharge;

        // �X�^���l
        maxStanPower = wepon[no].stanpower;


        // Sprites
        // ���������� ���������� ���������� ���������� //
        // �X�P�[��
        transform.localScale = new Vector3(
            wepon[no].wideth, wepon[no].height, 1.0f);

        // ���W
        transform.localPosition = new Vector3(
            0.0f, wepon[no].offset, 0.0f);


        // UI
        image_Wepon.sprite = Resources.Load<Sprite>(wepon[no].icon);


        // �v���C���[�ɕK�v�N�[���_�E����n���ă��^�[��
        return wepon[no].maxcharge;
    }

    // ���������� ���������� ���������� ���������� //



    // �U������
    // ���������� ���������� ���������� ���������� //
    public void Attacking(int nowCharge)
    {
        if (maxCharge == nowCharge)
        {
            nowDamage = maxDamage;
            nowKockBack = maxKnockBack;
            nowStanPower = maxStanPower;
        }
        else
        {
            nowDamage = defDamage;
            nowKockBack = defKnockBack;
            nowStanPower = 0;
        }

        coll.enabled = true;
        nowAttakingTime = defAttackingTime;

        spriteAlpha = 1.0f;
    }

    // ���������� ���������� ���������� ���������� //


    public bool Combo()
    {
        if (!comboFlag) { return false; }

        comboFlag = false;
        return true;
    }


    // �U���̖�������
    // ���������� ���������� ���������� ���������� //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyCTRL>().TakeDamage(nowDamage, nowKockBack, nowStanPower);
            
            if (nowDamage == maxDamage)
            {
                if (chargeCool <= 0)
                {
                    _playerCTRL.GetCharge();
                    chargeCool = defAttackingTime;
                    _playerCTRL.comboCount++;
                }
                comboFlag = true;
            }
        }
    }

    // ���������� ���������� ���������� ���������� //
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    /*
   [Header("PlayerCTRL")]
   [SerializeField] PlayerCTRL _plCTRL;

   [Header("�Q�[���I�u�W�F�N�g")]
   [SerializeField] GameObject cursor;
   [SerializeField] GameObject cursorImage;
   [SerializeField] GameObject bullet;

   [Header("�������U��")]
   [SerializeField] public int needGunCharge;  // �������U���ɕK�v�ȃ`���[�W
   [SerializeField] public int nowGunCharge;   // ���݂̃`���[�W
   [SerializeField] bool standby = false;
   [SerializeField] int needCountDown;
   [SerializeField] int nowCountDown = 0;

   [Header("�I�[�f�B�I(�}�j���A��)")]
   [SerializeField] AudioCTRL _audioCTRL;
   [SerializeField] AudioSource audioSource;   // �I�[�f�B�I�\�[�X
   [SerializeField] AudioClip[] audioClip_Gun;
   [SerializeField] AudioClip[] audioClip_Weapon;

   void Start()
   {
       // �I�[�f�B�I������
       audioSource = GetComponent<AudioSource>();
       audioSource.volume = _audioCTRL.defVolume;
       audioClip_Gun = new AudioClip[_audioCTRL.clips_Player_Gun.Length];
       audioClip_Gun = _audioCTRL.clips_Player_Gun;
       audioClip_Weapon = new AudioClip[_audioCTRL.clips_Player_Weapon.Length];
       audioClip_Weapon = _audioCTRL.clips_Player_Weapon;
   }

   public void Attack(PlayerControls playerControls, GC_BpmCTRL bpmCTRL, PlayerWeapon playerWeapon)
   {
       if (playerControls.Player.Attack.triggered
           && bpmCTRL.Signal()
           && _plCTRL.coolDownReset == false)
       {
           //audioSource.PlayOneShot(audioClip_Weapon[1]); // ���Y�����Ђǂ�
           _plCTRL._anim.SetTrigger("Attack");
           playerWeapon.Attacking(_plCTRL.nowWeaponCharge);
           _plCTRL.coolDownReset = true;
       }

       if (playerWeapon.Combo())
       {
           _plCTRL.nowWeaponCharge = _plCTRL.maxWeaponCharge;
           _plCTRL.doComboMode = true;
           _plCTRL.coolDownReset = false;
           _plCTRL.comboTimeLeft = 2;
       }


       if (bpmCTRL.Metronome())
       {
           // �N�[���_�E�����Z�b�g
           if (_plCTRL.coolDownReset == true && _plCTRL.doComboMode == false)
           {
               _plCTRL.nowWeaponCharge = 1;
               _plCTRL.coolDownReset = false;
           }
           else if (_plCTRL.nowWeaponCharge < _plCTRL.maxWeaponCharge) { _plCTRL.nowWeaponCharge++; }

           // �R���{�p�����Ԍ���
           if (_plCTRL.comboTimeLeft > 0)
           {
               _plCTRL.comboTimeLeft--;
           }

           // �R���{�I��
           if (_plCTRL.comboTimeLeft == 0 && _plCTRL.doComboMode == true)
           {
               _plCTRL.comboCount = 0;
               _plCTRL.doComboMode = false;
               _plCTRL.coolDownReset = true;
           }

           // �A�j���[�V����
           if (_plCTRL.nowWeaponCharge == (_plCTRL.maxWeaponCharge - 1))
           {
               //_plCTRL._anim.SetTrigger("Charge");
               _plCTRL._flashAnim.SetTrigger("FlashTrigger");
           }
       }

   }

   public void SwapWeapon(PlayerControls playerControls, PlayerWeapon playerWeapon)
   {
       var valueW = playerControls.Player.WeaponSwapWhile.ReadValue<float>();
       var valueUp = playerControls.Player.WeaponSwapButtonUp.triggered;
       var valueDwon = playerControls.Player.WeaponSwapButtonDown.triggered;

       if (valueW != 0 || (valueUp || valueDwon))
       {
           if (valueW > 0 || valueUp)
           {
               _plCTRL.equipNo++;
               if (_plCTRL.equipNo >= _plCTRL.equipList.weaponList.Length) { _plCTRL.equipNo = 0; }
           }

           if (valueW < 0 || valueDwon)
           {
               _plCTRL.equipNo--;
               if (_plCTRL.equipNo < 0) { _plCTRL.equipNo = 2; }
           }

           // �K�v�N�[���_�E���㏑��
           _plCTRL.maxWeaponCharge =
               playerWeapon.SwapWeapon(_plCTRL.equipList.weaponList, _plCTRL.equipNo);
           // ���N�[���_�E�����㏑��
           _plCTRL.nowWeaponCharge = 0;
       }
   }

   public void Shooting(PlayerControls playerControls, GC_BpmCTRL bpmCTRL)
   {
       if (bpmCTRL.Signal())
       {
           if (playerControls.Player.Shot.triggered)
           {
               if (nowGunCharge == needGunCharge && standby == false)
               {
                   audioSource.PlayOneShot(audioClip_Gun[0]);
                   standby = true;
                   nowCountDown = needCountDown;
               }
               else { audioSource.PlayOneShot(audioClip_Gun[2]); }
           }
       }

       if (standby) { BulletFire(bpmCTRL); }
   }

   void BulletFire(GC_BpmCTRL bpmCTRL)
   {
       if (bpmCTRL.Metronome())
       {
           if (nowCountDown == 0)
           {
               Instantiate(
                       bullet,
                       new Vector3
                       (cursorImage.transform.position.x,
                       cursorImage.transform.position.y,
                       cursorImage.transform.position.z),
                       cursor.transform.rotation);

               audioSource.PlayOneShot(audioClip_Gun[1]);
               standby = false;
               nowGunCharge = 0;
           }
           nowCountDown--;
       }
   }
   */
}

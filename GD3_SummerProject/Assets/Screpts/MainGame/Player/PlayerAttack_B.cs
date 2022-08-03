using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAttack_B : MonoBehaviour
{
    [Header("PlayerCTRL")]
    [SerializeField] PlayerCTRL _plCTRL;

    [Header("ゲームオブジェクト(マニュアル)")]
    [SerializeField] GameObject _cursor;
    [SerializeField] GameObject _cursorImage;
    [SerializeField] GameObject _bullet;

    [Header("キャンバスUI(マニュアル)")]
    [SerializeField] public Image _image_Wepon;
    [SerializeField] TextMeshProUGUI _text_Gun;
    [SerializeField] Slider _slider_Gun;

    [Header("パラメータ")]
    [SerializeField] public int _defPenalty = 1;

    [Header("遠距離攻撃")]
    [SerializeField] public int _needGunCharge;  // 遠距離攻撃に必要なチャージ
    [SerializeField] public int _nowGunCharge;   // 現在のチャージ
    [SerializeField] bool _standby = false;
    [SerializeField] int _needCountDown;
    [SerializeField] int _nowCountDown = 0;

    [Header("オーディオ(マニュアル)")]
    [SerializeField] MainAudioCTRL _audioCTRL;
    [SerializeField] AudioSource audioSource;   // オーディオソース
    [SerializeField] AudioClip[] audioClip_Weapon;
    [SerializeField] AudioClip[] audioClip_Gun;


    void Start()
    {
        // オーディオ初期化
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = _audioCTRL.nowVolume;
        audioClip_Gun = new AudioClip[_audioCTRL.clips_Player_Gun.Length];
        audioClip_Gun = _audioCTRL.clips_Player_Gun;
        audioClip_Weapon = new AudioClip[_audioCTRL.clips_Player_Weapon.Length];
        audioClip_Weapon = _audioCTRL.clips_Player_Weapon;
    }

    private void Update()
    {
        // 銃
        if (_nowGunCharge >= _needGunCharge) { _text_Gun.text = "Ready"; }
        else { _text_Gun.text = _nowGunCharge + " / " + _needGunCharge; }
        
        _slider_Gun.value = (float)_nowGunCharge / (float)_needGunCharge;
    }

    public void Attack(PlayerControls playerControls, GC_BpmCTRL bpmCTRL, PlayerWeapon_B playerWeapon)
    {
        if (playerControls.Player.Attack.triggered &&
            !_plCTRL._orFaild &&
            playerWeapon.nowAttakingTime < 0)
        {
            if ( bpmCTRL.Signal() ||
                (_plCTRL.doComboMode && bpmCTRL.HalfSignal() && playerWeapon.GetTypeNum() != 2) )
            {
                _plCTRL._anim.SetTrigger("Attack");
                playerWeapon.Attacking(bpmCTRL.Perfect());
                audioSource.PlayOneShot(audioClip_Weapon[0]);
            }
            else
            {
                _plCTRL._resultText.text = "miss...";
                _plCTRL._comboText.text = "";
                _plCTRL._comboTextAlpha = 1.0f;
                _plCTRL._orFaild = true;
                _plCTRL._orFaildCount = _defPenalty;
            }

        }

        if (playerWeapon.Combo())
        {
            if(playerWeapon.IsPerfect()) { _plCTRL._resultText.text = "PERFECT!"; }
            else { _plCTRL._resultText.text = "HIT!"; }
            
            _plCTRL._resultText.alpha = 1.0f;
            _plCTRL.doComboMode = true;
            _plCTRL.comboTimeLeft = 2;
        }

        if (bpmCTRL.Count())
        {
            // コンボ継続時間減少
            if (_plCTRL.comboTimeLeft > 0)
            {
                _plCTRL.comboTimeLeft--;
            }

            // コンボ終了
            if (_plCTRL.comboTimeLeft == 0 && _plCTRL.doComboMode == true)
            {
                _plCTRL.comboCount = 0;
                _plCTRL.doComboMode = false;
            }

            if (_plCTRL._orFaild)
            {
                if (_plCTRL._orFaildCount >= 1)
                {
                    _plCTRL._orFaildCount--;
                }

                if (_plCTRL._orFaildCount <= 0)
                {
                    _plCTRL._orFaild = false;
                }
            }
        }

    }

    public void SwapingWeapon(PlayerControls playerControls, PlayerWeapon_B playerWeapon)
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
                if (_plCTRL.equipNo < 0) { _plCTRL.equipNo = 1; }
            }
        }

        playerWeapon.SwapWeapon(_plCTRL.equipList.weaponList, _plCTRL.equipNo);
    }

    public void Shooting(PlayerControls playerControls, GC_BpmCTRL bpmCTRL)
    {
        if (bpmCTRL.Signal())
        {
            if (playerControls.Player.Shot.triggered)
            {
                if (_nowGunCharge == _needGunCharge && _standby == false)
                {
                    audioSource.PlayOneShot(audioClip_Gun[0]);
                    _standby = true;
                    _nowCountDown = _needCountDown;
                }
                else { audioSource.PlayOneShot(audioClip_Gun[2]); }
            }
        }

        if (_standby) { BulletFire(bpmCTRL); }

    }

    void BulletFire(GC_BpmCTRL bpmCTRL)
    {
        if (bpmCTRL.Count() || bpmCTRL.Step())
        {
            if (_nowCountDown == 0)
            {
                Instantiate(
                        _bullet,
                        new Vector3
                        (_cursorImage.transform.position.x,
                        _cursorImage.transform.position.y,
                        _cursorImage.transform.position.z),
                        _cursor.transform.rotation);

                audioSource.PlayOneShot(audioClip_Gun[1]);
                _standby = false;
                _nowGunCharge = 0;
            }
            else
            {
                _nowCountDown--;
            }
        }
    }

    public void GetCharge()
    {
        if (_nowGunCharge < _needGunCharge) { _nowGunCharge++; }
    }

}

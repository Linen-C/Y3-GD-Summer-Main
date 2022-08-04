using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("�_���[�W")]
    [SerializeField] Image image_DamagePanel;
    [SerializeField] float image_DamagePanel_defalpha = 1.0f;
    [SerializeField] float image_DamagePanel_nowalpha = 0.0f;

    [Header("�R���{")]
    [SerializeField] public TextMeshProUGUI _comboText;
    [SerializeField] public TextMeshProUGUI _resultText;
    [SerializeField] public float _comboTextAlpha = 0.0f;

    [Header("���b�N")]
    [SerializeField] TextMeshProUGUI text_RockCount;
    [SerializeField] Image image_Rock;
    [SerializeField] float _rockAlpha = 0.0f;

    [Header("�n�[�t�Q�[�W")]
    [SerializeField] Image image_half_Hit;
    [SerializeField] Image image_half_Per;
    [SerializeField] float _halfGaugeAlpha = 0.0f;

    [Header("�̗�")]
    [SerializeField] Slider hpSlider;

    [Header("����")]
    [SerializeField] public Image _image_Wepon;

    [Header("�ˌ�")]
    [SerializeField] public TextMeshProUGUI _text_Gun;
    [SerializeField] public Slider _slider_Gun;


    private void Awake()
    {
        // �A���t�@�l�̏����ݒ�
        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }


    public void HelthSetStart()
    {
        // �̗͏����l�ݒ�
        hpSlider.value = 1;
    }

    public void UIUpdate(int nowHP,int maxHP,int comboCount, bool orFaild, int failCount, PlayerWeapon_B plWeapon)
    {
        // �̗͕\��
        hpSlider.value = (float)nowHP / (float)maxHP;

        // �_���[�W�\��
        if (image_DamagePanel_nowalpha > 0.0f) { image_DamagePanel_nowalpha -= Time.deltaTime; }
        image_DamagePanel.color = new Color(1.0f, 1.0f, 1.0f, image_DamagePanel_nowalpha);


        // �R���{�\��
        if (comboCount == 0)
        {
            if (_comboTextAlpha > 0.0f) _comboTextAlpha -= Time.deltaTime;
            if (_halfGaugeAlpha > 0.0f) _halfGaugeAlpha -= Time.deltaTime;
        }
        else
        {
            _comboTextAlpha = 1.0f;
            _comboText.text = "x" + comboCount;

            if (plWeapon.GetTypeNum() != 2) { _halfGaugeAlpha = 0.35f; }
        }
        _comboText.alpha = _comboTextAlpha;
        _resultText.alpha = _comboTextAlpha;
        image_half_Hit.color = new Color(1.0f, 0, 0, _halfGaugeAlpha);
        image_half_Per.color = new Color(0, 1.0f, 1.0f, _halfGaugeAlpha);


        // �R���{�~�X
        if (orFaild)
        {
            _rockAlpha = 0.6f;
            text_RockCount.text = failCount.ToString();
        }
        else
        {
            if (_rockAlpha > 0.0f) { _rockAlpha -= Time.deltaTime * 5.0f; }
        }
        text_RockCount.alpha = _rockAlpha;
        image_Rock.color = new Color(1.0f, 0.0f, 0.0f, _rockAlpha);

    }

    public void DamagePanelAlphaSet()
    {
        // �_���[�W���󂯂��Ƃ��̊O�g
        image_DamagePanel_nowalpha = image_DamagePanel_defalpha;
    }

    public void GunUIUpdate(int nowGunCharge, int needGunCharge)
    {
        if (nowGunCharge >= needGunCharge) { _text_Gun.text = "Ready"; }
        else { _text_Gun.text = nowGunCharge + " / " + needGunCharge; }

        _slider_Gun.value = (float)nowGunCharge / (float)needGunCharge;
    }
}

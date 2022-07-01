using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu_Customize : MonoBehaviour
{
    [Header("武器リストと装備武器のロード・セーブ")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] WeaponListLoad weaponListLoad;
    JsonData jsonData;

    [Header("一覧")]
    [SerializeField] public WeaponList[] weaponList;

    [Header("装備管理")]
    [SerializeField] public WeaponList[] equipList;

    [Header("装備中の武器")]
    [SerializeField] int[] weaponNumbers;
    [SerializeField] Image image_weaponA;
    [SerializeField] Image image_weaponB;
    [SerializeField] Image image_weaponC;
    [SerializeField] Image[] weaponImages;

    [Header("ボタン動的生成")]
    [SerializeField] GameObject _buttonTemp;
    [SerializeField] GameObject _parent;   // これを親としてボタンを生成
    [SerializeField] int _defHoriDist;
    [SerializeField] int _horiDist;
    [SerializeField] int _defVertDist;
    [SerializeField] int _vertDist;

    [Header("右画面")]
    [SerializeField] TextMeshProUGUI text_weaponName;
    [SerializeField] TextMeshProUGUI text_weaponSpec;

    [Header("画面遷移用")]
    [SerializeField] Canvas _Main;
    [SerializeField] Canvas _Customize;

    [Header("変更用")]
    [SerializeField] int _target_Num;

    void Start()
    {
        weaponListLoad = saveManager.transform.GetComponent<WeaponListLoad>();
        jsonData = weaponListLoad.GetList();

        //weaponList = jsonData.weaponList;

        equipList = new WeaponList[3];
        equipList = saveManager.EquipLoad();

        weaponNumbers = new int[3];
        weaponNumbers[0] = equipList[0].number;
        weaponNumbers[1] = equipList[1].number;
        weaponNumbers[2] = equipList[2].number;

        weaponImages = new Image[3];
        weaponImages[0] = image_weaponA;
        weaponImages[1] = image_weaponB;
        weaponImages[2] = image_weaponC;
        weaponImages[0].sprite = Resources.Load<Sprite>(equipList[0].icon);
        weaponImages[1].sprite = Resources.Load<Sprite>(equipList[1].icon);
        weaponImages[2].sprite = Resources.Load<Sprite>(equipList[2].icon);
    }



    // 装備武器変更
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void SetWeaponChangeA()
    {
        ButtonErase();
        _target_Num = 0;
        ShowWeaponData();
        ButtonGenerate();
    }

    public void SetWeaponChangeB()
    {
        ButtonErase();
        _target_Num = 1;
        ShowWeaponData();
        ButtonGenerate();
    }

    public void SetWeaponChangeC()
    {
        ButtonErase();
        _target_Num = 2;
        ShowWeaponData();
        ButtonGenerate();
    }

    public void SetGunChange()
    {
        ButtonErase();
    }

    void ButtonGenerate()
    {
        _horiDist = -_defHoriDist;
        _vertDist = _defVertDist;

        for (int num = 0; jsonData.weaponList.Length > num; num++)
        {
            var button = Instantiate(_buttonTemp, new Vector2(_horiDist, _vertDist), Quaternion.identity);
            var image = button.transform.GetChild(0).GetComponent<Image>();

            button.transform.SetParent(_parent.transform, false);
            button.name = "weaponButton" + num.ToString();
            image.sprite = Resources.Load<Sprite>(jsonData.weaponList[num].icon);

            int indexNumber = num;
            button.GetComponent<Button>().onClick.AddListener(() => SetWepon(button, indexNumber));
            //Debug.Log("配列ナンバー：" + indexNumber);

            _horiDist += _defHoriDist;
            if (num % 2 == 0 && num != 0)
            {
                _horiDist = -_defHoriDist;
                _vertDist -= _defVertDist;
            }
        }
    }

    void SetWepon(GameObject btn, int number)
    {
        //Debug.Log("テスト：" + btn.name + "\nナンバー：" + number);
        equipList[_target_Num] = jsonData.weaponList[number];

        weaponNumbers[_target_Num] = equipList[_target_Num].number;
        weaponImages[_target_Num].sprite = Resources.Load<Sprite>(equipList[_target_Num].icon);

        ShowWeaponData();
    }

    void ButtonErase()
    {
        foreach (Transform child in _parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // 表示
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    void ShowWeaponData()
    {
        text_weaponName.text = equipList[_target_Num].name;

        string damage = equipList[_target_Num].damage.ToString();
        string knockBack = equipList[_target_Num].maxknockback.ToString();
        string maxCharge = equipList[_target_Num].maxcharge.ToString();
        string stanPower = equipList[_target_Num].stanpower.ToString();
        string width = equipList[_target_Num].wideth.ToString();
        string height = equipList[_target_Num].height.ToString();

        text_weaponSpec.text =
            damage + "\n" +
            knockBack + "\n" +
            maxCharge + "\n" +
            stanPower + "\n" +
            width + "x" + height;
    }



    // メインメニューへ戻る
    // ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ ＝＝＝＝＝ //
    public void Customize_to_Main()
    {
        ButtonErase();
        text_weaponName.text = "";
        text_weaponSpec.text = "";

        _Customize.enabled = false;
        _Main.enabled = true;

        saveManager.EquipSave();
        Debug.Log("Saved...");
    }

}

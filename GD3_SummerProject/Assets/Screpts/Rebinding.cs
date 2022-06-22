using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;

public class Rebinding : MonoBehaviour
{
    [SerializeField] Text _Text;
    [SerializeField] Text[] _ListText_MoveButton;
    [SerializeField] Text[] _ListText_WeponSwap;
    [SerializeField] PlayerInput _input;
    [SerializeField] InputActionReference _action;
    [SerializeField] InputActionReference[] _actions;
    [SerializeField] InputActionAsset _actionAsset;

    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    string filePath;

    InputBinding bindTarget;
    InputActionReference bindAction;

    private void Start()
    {
        // キーボード版を作ったらコントローラー版も作らなきゃ

        filePath = Application.dataPath + "/Resources/jsons/KeyBind.json";

        string inputJson = Resources.Load<TextAsset>("jsons/KeyBind").ToString();
        _actionAsset.actionMaps[0].LoadBindingOverridesFromJson(inputJson);

        /*
        // リストが必要になったとき用
        var listTarget = _actionAsset.actionMaps[0];
        var inspath = Application.dataPath + "/Resources/jsons/DefKeyList.json";
        File.WriteAllText(inspath, listTarget.ToJson());
        */

        // 不本意だけどこれでいける
        foreach (var actions in _actions)
        {
            if (actions.action.name == "Move") { /*Debug.Log("Find!");*/ }
        }

        //var listTarget = _actionAsset.actionMaps[0].actions[1].type;
        var test1 = _actions[4].action.bindings[0].name;
        var test2 = _actions[4].action.name;
        var test3 = _actions[4].action.bindings[1].name;
        //Debug.Log(test1);
        //Debug.Log(test2);
        //Debug.Log(test3);

        //foreach (var actions in _actionAsset.actionMaps[0])
        //{
        //    _ListText.text += actions.name + "：";
        //    _ListText.text += actions.GetBindingDisplayString() + "\n";
        //}

        // 移動方向ページ
        List_MoveDir_Key();
        List_WeponSwap();

        _Text.text = _action.action.name + "：" + _action.action.GetBindingDisplayString();

    }

    void List_MoveDir_Key()
    {
        int moveIndex = 0;
        foreach (var actions in _actionAsset.actionMaps[0].bindings)
        {
            if (actions.groups == "KeyBoard" && actions.action == "Move")
            {
                //Debug.Log(actions.name);
                _ListText_MoveButton[moveIndex].text = actions.ToDisplayString();
                moveIndex++;
            }
        }
    }

    void List_WeponSwap()
    {
        foreach (var actions in _actionAsset.actionMaps[0].bindings)
        {
            if (actions.action == "WeponSwapButtonUp")
            {
                _ListText_WeponSwap[0].text = actions.ToDisplayString();
            }
            if (actions.action == "WeponSwapButtonDown")
            {
                _ListText_WeponSwap[1].text = actions.ToDisplayString();
            }
        }
    }


    public void MoveDir_Up()
    {
        foreach (var actions in _actions)
        {
            foreach (var binds in actions.action.bindings)
            {
                if (binds.name == "up")
                {
                    Key_RebindingMode(actions);
                    return;
                }
            }
        }
    }

    public void Clicked()
    {
        _input.SwitchCurrentActionMap("Select");
        _Text.text = "バインディング中";

        //ターゲット設定
        bindTarget = _action.action.bindings[0];

        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .OnComplete(opth => Key_RebindingComplete())
            .Start();
    }

    void Key_RebindingMode(InputActionReference target)
    {
        _input.SwitchCurrentActionMap("Select");
        _ListText_MoveButton[0].text = "バインディング中";

        //ターゲット設定
        bindTarget = target.action.bindings[1];
        bindAction = target;

        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .OnComplete(opth => Key_RebindingComplete())
            .Start();
    }

    void Key_RebindingComplete()
    {
        bindTarget.overridePath = InputControlPath.ToHumanReadableString(
            _action.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        //Debug.Log("Target：" + bindTarget.path);
        //Debug.Log("OverWr：" + bindTarget.overridePath);

        _action.action.ApplyBindingOverride(new InputBinding { path = bindTarget.path, overridePath = bindTarget.overridePath });

        string output = _action.action.SaveBindingOverridesAsJson();
        //Debug.Log(output);
        File.WriteAllText(filePath, output);

        _input.SwitchCurrentActionMap("Player");
        _rebindingOperation.Dispose();

        /*
        _ListText.text = "";
        foreach (var actions in _actionAsset.actionMaps[0])
        {
            _ListText.text += actions.name + "：";
            _ListText.text += actions.GetBindingDisplayString() + "\n";
            //Debug.Log(actions.name + "：" + actions.GetBindingDisplayString());
        }
        _Text.text = _Text.text = _action.action.name + "：" + _action.action.GetBindingDisplayString();
        */

        List_MoveDir_Key();
        List_WeponSwap();

    }

}
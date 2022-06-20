using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;

public class Rebinding : MonoBehaviour
{
    [SerializeField] Text _Text;
    [SerializeField] Text _ListText;
    [SerializeField] PlayerInput _input;
    [SerializeField] InputActionReference _action;
    [SerializeField] InputActionAsset _actionAsset;

    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

    string bindTarget;

    private void Start()
    {
        // タスク：バインディング内容をjsonとしてresourceフォルダに生成、読み込みする
        // キーボード版を作ったらコントローラー版も作らなきゃ

        //string tes = _actionAsset.FindAction(_action.action.name).ToString();
        //Debug.Log(tes);

        foreach (var actions in _actionAsset.actionMaps[0])
        {
            _ListText.text += actions.name + "：";
            _ListText.text += actions.GetBindingDisplayString() + "\n";
            //Debug.Log(actions.name);
        }

        _Text.text = _action.action.name + "：" + _action.action.GetBindingDisplayString();
    }

    public void Clicked()
    {
        var target = _action.action;

        _input.SwitchCurrentActionMap("Select");
        _Text.text = "バインディング中";

        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .OnComplete(opth => Key_RebindingComplete())
            .Start();
    }

    void Key_RebindingMode()
    {
        // タスク：「_action」をClickedのトコで指定できれば自由にできそう

        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .OnComplete(opth => Key_RebindingComplete())
            .Start();
    }

    void Key_RebindingComplete()
    {
        bindTarget = InputControlPath.ToHumanReadableString(
            _action.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        Debug.Log(bindTarget);

        // タスク：元のpathンところも上手いことやればいけるかも？

        _action.action.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/g", overridePath = "<Keyboard>/" + bindTarget });
        _input.SwitchCurrentActionMap("Player");

        _rebindingOperation.Dispose();


        _ListText.text = "";

        foreach (var actions in _actionAsset.actionMaps[0])
        {
            _ListText.text += actions.name + "：";
            _ListText.text += actions.GetBindingDisplayString() + "\n";
            Debug.Log(actions.name + "：" + actions.GetBindingDisplayString());
        }

        _Text.text = _Text.text = _action.action.name + "：" + _action.action.GetBindingDisplayString();
    }

}
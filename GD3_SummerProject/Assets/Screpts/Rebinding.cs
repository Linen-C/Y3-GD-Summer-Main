using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;

public class Rebinding : MonoBehaviour
{
    [SerializeField] Text _Text;
    [SerializeField] PlayerInput _input;
    [SerializeField] InputControl _control;
    [SerializeField] InputActionReference _action;

    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

    private void Start()
    {
        _Text.text = _action.action.GetBindingDisplayString();
    }

    public void Clicked()
    {
        _input.SwitchCurrentActionMap("Select");
        _Text.text = "バインディング中";

        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .OnComplete(opth => Rebind())
            .Start();
    }

    private void Rebind()
    {
        var test = InputControlPath.ToHumanReadableString(_action.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        Debug.Log(test);

        _action.action.ApplyBindingOverride(new InputBinding { path = "<Keyboard>/g", overridePath = "<Keyboard>/" + test });
        _input.SwitchCurrentActionMap("Player");

        _rebindingOperation.Dispose();

        _Text.text = _action.action.GetBindingDisplayString();
    }

}
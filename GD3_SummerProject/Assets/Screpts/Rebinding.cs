using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;
using System.IO;

public class Rebinding : MonoBehaviour
{
    [SerializeField] string _schema;
    [SerializeField] InputActionReference _actionReference;
    [SerializeField] PlayerInput _input;

    [Header("UI")]
    [SerializeField] GameObject _overlay;
    [SerializeField] Text _text;

    InputAction _action;

    private void Start()
    {
        _action = _actionReference.ToInputAction();

        //var bindingIndex = _actionReference.action.GetBindingIndex(_schema);
        //var keycode = _action.bindings[bindingIndex].ToDisplayString();
        _text.text = _actionReference.action.GetBindingDisplayString();
    }

    public void Config()
    {
        void Done(RebindingOperation op)
        {
            //_overlay.SetActive(false);
            op.Dispose();
            _input.SwitchCurrentActionMap("Player");
        }

        //_overlay.SetActive(true);

        _input.SwitchCurrentActionMap("Select");

        _action.PerformInteractiveRebinding()
            .WithTargetBinding(_action.GetBindingIndex(_schema))
            .WithControlsExcluding("<Keybord>/g")
            .OnCancel(op => 
            {
                Done(op);
            })
            .OnComplete(op =>
            {
                UpdateDisplay();
                Done(op);
            })
            .Start();
    }

    void UpdateDisplay()
    {
        /*
        var bindingIndex  = _action.GetBindingIndex(_schema);
        var keycode = _action.bindings[bindingIndex].ToDisplayString();
        _text.text = keycode;
        */
        _text.text = _actionReference.action.GetBindingDisplayString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNewMoveTest : MonoBehaviour
{
    PlayerInput _input;
    Rigidbody2D _body;

    void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _body);
    }

    void OnEnable()
    {
        _input.actions["Move"].performed += OnMove;
    }

    void OnDisEnable()
    {
        _input.actions["Move"].performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<Vector2>();
        var dir = new Vector3(value.x, 0, value.y);

        _body.velocity = dir;
    }

}

using GGJ.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerType playerType;

    public IObservable<Unit> OnPressedFireButton => _onPressedFireButton;
    private Subject<Unit> _onPressedFireButton = new Subject<Unit>();

    public IObservable<Unit> OnCanceledFireButton => _onCanceledFireButton;
    private Subject<Unit> _onCanceledFireButton = new Subject<Unit>();

    public bool isFired { get; private set; } = false;

    private void Awake()
    {
        var inputActions = new FukuwaraiControls();

        switch (playerType)
        {
            case PlayerType.Player1:
                inputActions.Game.Fire.performed += OnFireButtonDown;
                inputActions.Game.Fire.canceled += OnFireButtonUp;
                break;
            case PlayerType.Player2:
                inputActions.Game.Fire2.performed += OnFireButtonDown;
                inputActions.Game.Fire2.canceled += OnFireButtonUp;
                break;
            case PlayerType.Player3:
                inputActions.Game.Fire3.performed += OnFireButtonDown;
                inputActions.Game.Fire3.canceled += OnFireButtonUp;
                break;
            default:
                inputActions.Game.Fire4.performed += OnFireButtonDown;
                inputActions.Game.Fire4.canceled += OnFireButtonUp;
                break;
        }
        inputActions.Enable();
    }


    private void OnFireButtonUp(InputAction.CallbackContext context)
    {
        if (isFired) return;
        isFired = true;
        _onCanceledFireButton.OnNext(default);
    }

    private void OnFireButtonDown(InputAction.CallbackContext context)
    {
        _onPressedFireButton.OnNext(default);
    }
}

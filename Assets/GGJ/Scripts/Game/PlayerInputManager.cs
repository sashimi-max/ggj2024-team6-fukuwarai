using Cysharp.Threading.Tasks;
using GGJ.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Game
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerType playerType;
        public PlayerType PlayerType
        {
            get { return this.playerType; }
            set { this.playerType = value; }
        }

        public bool IsPressedAnyFireButton => inputActions.Game.Fire.IsPressed() ||
                    inputActions.Game.Fire2.IsPressed() ||
                    inputActions.Game.Fire3.IsPressed() ||
                    inputActions.Game.Fire4.IsPressed();

        public IObservable<Unit> OnPressedFireButton => _onPressedFireButton;
        private Subject<Unit> _onPressedFireButton = new Subject<Unit>();

        public IObservable<Unit> OnCanceledFireButton => _onCanceledFireButton;
        private Subject<Unit> _onCanceledFireButton = new Subject<Unit>();

        public IReadOnlyReactiveProperty<bool> IsFired => _isFired;
        private BoolReactiveProperty _isFired = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> CanPressedButton => _canPressedButton;
        private BoolReactiveProperty _canPressedButton = new BoolReactiveProperty(true);
        private float reenabledTimer = 0.0f;

        private const int ABLE_FIRE_COUNT = 2;
        private int currentFireCount = 0;

        private FukuwaraiControls inputActions;

        public void Init(PlayerType playerType)
        {
            PlayerType = playerType;

            inputActions = new FukuwaraiControls();

            switch (PlayerType)
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

        private void Update()
        {
            if (_isFired.Value || _canPressedButton.Value) return;

            reenabledTimer += Time.deltaTime;
            if (reenabledTimer > 2.0f)
            {
                _canPressedButton.Value = true;
            }
        }

        private void OnFireButtonUp(InputAction.CallbackContext context)
        {
            if (_isFired.Value || !_canPressedButton.Value) return;
            _canPressedButton.Value = false;
            reenabledTimer = 0.0f;
            currentFireCount++;
            if (currentFireCount == ABLE_FIRE_COUNT)
            {
                _isFired.Value = true;
                _canPressedButton.Value = false;
            }

            _onCanceledFireButton.OnNext(default);
        }

        private void OnFireButtonDown(InputAction.CallbackContext context)
        {
            if (_isFired.Value || !_canPressedButton.Value) return;
            _onPressedFireButton.OnNext(default);
        }
    }
}

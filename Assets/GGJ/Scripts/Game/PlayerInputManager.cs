using Cysharp.Threading.Tasks;
using GGJ.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

        public bool IsPressedAnyFireButton => _fireInputAction.IsPressed() ||
                                              _buttonDownFlag;

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
        private Button _button;
        private bool _buttonDownFlag;
        private InputAction _fireInputAction;
        private Image _image;

        public void Init(PlayerType playerType, Button button, Image image)
        {
            PlayerType = playerType;
            _button = button;
            _image = image;

            inputActions = new FukuwaraiControls();

            _fireInputAction = PlayerType switch
            {
                PlayerType.Player1 => inputActions.Game.Fire,
                PlayerType.Player2 => inputActions.Game.Fire2,
                PlayerType.Player3 => inputActions.Game.Fire3,
                _ => inputActions.Game.Fire4
            };
            _fireInputAction.performed += OnFireButtonDown;
            _fireInputAction.canceled += OnFireButtonUp;

            _button.OnPointerDownAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _buttonDownFlag = true;
                    OnFireButtonDown();
                })
                .AddTo(this);
            _button.OnPointerUpAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    OnFireButtonUp();
                })
                .AddTo(this);
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
            OnFireButtonUp();
        }
        
        private void OnFireButtonUp()
        {
            if (_isFired.Value || !_canPressedButton.Value) return;
            _canPressedButton.Value = false;
            reenabledTimer = 0.0f;
            currentFireCount++;
            if (currentFireCount >= ABLE_FIRE_COUNT)
            {
                _isFired.Value = true;
                _canPressedButton.Value = false;
                _image.enabled = false;
            }
            else
            {
                var color = _image.color;
                color.a = 0.8f;
                _image.color = color;
            }

            _buttonDownFlag = false;
            _onCanceledFireButton.OnNext(default);
        }

        private void OnFireButtonDown(InputAction.CallbackContext context)
        {
            OnFireButtonDown();
        }

        private void OnFireButtonDown()
        {
            if (_isFired.Value || !_canPressedButton.Value) return;
            _onPressedFireButton.OnNext(default);
        }
    }
}

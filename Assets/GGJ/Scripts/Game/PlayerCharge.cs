using Cysharp.Threading.Tasks;
using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Game
{
    public class PlayerCharge : MonoBehaviour
    {
        private PlayerType playerType;
        [SerializeField] private GamePlayParameterAsset gamePlayParameter = default;


        public float chargedTime { get; private set; } = 0.0f;

        // 0~1の値を行ったり来たり
        public float normalizedChargedTime => Mathf.PingPong(chargedTime, gamePlayParameter.playerChargeSeconds) / gamePlayParameter.playerChargeSeconds;

        private FukuwaraiControls inputActions;
        private PlayerInputManager _playerInputManager;

        private void Awake()
        {
            inputActions = new FukuwaraiControls();
            inputActions.Enable();
        }

        private void Start()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
            playerType = _playerInputManager.PlayerType;

            _playerInputManager.OnCanceledFireButton
                .Subscribe(async _ =>
                {
                    await UniTask.WaitForSeconds(0.1f);
                    chargedTime = 0.0f;
                })
                .AddTo(this);
        }

        private void Update()
        {
            if (_playerInputManager.IsPressedAnyFireButton)
            {
                chargedTime += Time.deltaTime;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using static GamePlayParameterAssetInstaller;
using GGJ.Common;

namespace GGJ.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class FacePartsHolder : MonoBehaviour
    {
        [SerializeField] private GamePlayParameter gamePlayParameter = default;

        public PlayerType playerType;

        RectTransform rectTransform;
        Sequence sequence;
        float width;

        private void Awake()
        {
            var inputActions = new FukuwaraiControls();

            switch (playerType)
            {
                case PlayerType.Player1:
                    inputActions.Game.Fire.started += OnFireButtonDown;
                    break;
                case PlayerType.Player2:
                    inputActions.Game.Fire2.started += OnFireButtonDown;
                    break;
                case PlayerType.Player3:
                    inputActions.Game.Fire3.started += OnFireButtonDown;
                    break;
                default:
                    inputActions.Game.Fire4.started += OnFireButtonDown;
                    break;
            }
            inputActions.Enable();
        }


        // Start is called before the first frame update
        void Start()
        {
            var parentRectTransform = transform.parent.GetComponent<RectTransform>();
            width = parentRectTransform.sizeDelta.x;
            rectTransform = GetComponent<RectTransform>();
            DoYoYo();
        }

        public void DoYoYo()
        {
            sequence = DOTween.Sequence();

            sequence
                .Append(rectTransform.DOAnchorPos(new Vector3(width, 0, 0), gamePlayParameter.playerBarMoveTime))
                .Append(rectTransform.DOAnchorPos(new Vector3(0, 0, 0), gamePlayParameter.playerBarMoveTime))
                .SetLoops(-1)
                .Play();
        }

        private void OnFireButtonDown(InputAction.CallbackContext context)
        {
            sequence.Kill();
        }
    }
}

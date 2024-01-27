using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using GGJ.Common;
using UniRx;
using KanKikuchi.AudioManager;

namespace GGJ.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class FacePartsHolder : MonoBehaviour
    {
        [SerializeField] private GamePlayParameterAsset gamePlayParameter = default;

        private PlayerInputManager playerInputManager;

        RectTransform rectTransform;
        Sequence sequence;
        float width;

        // Start is called before the first frame update
        void Start()
        {
            playerInputManager = GetComponentInParent<PlayerInputManager>();
            var parentRectTransform = transform.parent.GetComponent<RectTransform>();
            width = parentRectTransform.sizeDelta.x;
            rectTransform = GetComponent<RectTransform>();
            DoYoYo();

            playerInputManager
                .OnPressedFireButton
                .Subscribe(_ =>
                {
                    SEManager.Instance.Play(AudioRandomContainer.Instance.RandomSE(SEPath.SE_FACE_SELECT1, SEPath.SE_FACE_SELECT2, SEPath.SE_FACE_SELECT3));
                    sequence.Pause();
                })
                .AddTo(this);

            playerInputManager
                .OnCanceledFireButton
                .Subscribe(_ => sequence.Play())
                .AddTo(this);
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
    }
}

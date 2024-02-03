using DG.Tweening;
using GGJ.Common;
using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Game
{
    public class PlayerBar : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] private FacePartsHolder facePartsHolder;

        [SerializeField] private GamePlayParameterAsset gamePlayParameter = default;

        private Image assignedKeyImage;
        RectTransform playerBarRect;
        RectTransform facePartsHolderRect;
        Sequence sequence;
        float width;
        float timer = 0.0f;
        bool isRotation = false;
        Vector2 origin;
        float distance;

        public void Init(PlayerType playerType, Image keyImage, Button button)
        {
            playerInputManager.Init(playerType, button, keyImage);
            assignedKeyImage = keyImage;
            InitHolder();
        }

        private void Update()
        {
            if (SceneContext.Instance.IsHardMode)
            {
                if (isRotation)
                {
                    transform.position = new Vector2(Mathf.Cos(timer), Mathf.Sin(timer)) * distance;
                    var rot = timer / Mathf.PI * 180.0f + 90.0f;
                    transform.localRotation = Quaternion.Euler(Vector3.forward * rot);
                }
                timer += Time.deltaTime;
            }
        }

        public void InitHolder()
        {
            facePartsHolderRect = facePartsHolder.GetComponent<RectTransform>();
            playerBarRect = GetComponent<RectTransform>();
            origin = transform.position;
            distance = Vector2.Distance(Vector2.zero, origin);
            width = playerBarRect.sizeDelta.x;

            if (SceneContext.Instance.IsHardMode)
            {
                DoRotateAround();
            }
            else
            {
                DoYoYo();
            }

            playerInputManager
                .OnPressedFireButton
                .Subscribe(_ =>
                {
                    SEManager.Instance.Play(AudioRandomContainer.Instance.RandomSE(SEPath.SE_FACE_SELECT1, SEPath.SE_FACE_SELECT2, SEPath.SE_FACE_SELECT3));
                    sequence.Pause();
                    if (SceneContext.Instance.IsHardMode) isRotation = false;
                })
                .AddTo(this);

            playerInputManager
                .OnCanceledFireButton
                .Subscribe(_ =>
                {
                    sequence.Play();
                    if (SceneContext.Instance.IsHardMode) isRotation = true;
                })
                .AddTo(this);

            playerInputManager
                .CanPressedButton
                .DistinctUntilChanged()
                .Subscribe(canPressed => assignedKeyImage.enabled = canPressed)
                .AddTo(this);
        }

        private void DoYoYo()
        {
            sequence = DOTween.Sequence();

            sequence
                .Append(facePartsHolderRect.DOAnchorPos(new Vector3(width, 0, 0), gamePlayParameter.playerBarMoveTime).SetEase(Ease.Linear))
                .Append(facePartsHolderRect.DOAnchorPos(new Vector3(0, 0, 0), gamePlayParameter.playerBarMoveTime).SetEase(Ease.Linear))
                .SetLoops(-1)
                .Play();
        }

        private void DoRotateAround()
        {
            facePartsHolderRect.anchoredPosition = new Vector2(width / 2.0f, facePartsHolderRect.anchoredPosition.y);
            switch (playerInputManager.PlayerType)
            {
                case PlayerType.Player1:
                    timer = 1.5f * Mathf.PI;
                    break;
                case PlayerType.Player2:
                    timer = 0.0f;
                    break;
                case PlayerType.Player3:
                    timer = 0.5f * Mathf.PI;
                    break;
                default:
                    timer = Mathf.PI;
                    break;
            }
            isRotation = true;
        }

        public FacePartsHolder GetFacePartsHolder()
        {
            return facePartsHolder;
        }
    }
}
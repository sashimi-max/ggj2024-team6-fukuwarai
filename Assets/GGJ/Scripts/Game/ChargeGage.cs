using GGJ.Common;
using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Game
{
    public class ChargeGage : MonoBehaviour
    {
        [SerializeField] private AnimationCurve widthAnimationCurve;
        [SerializeField] private AnimationCurve heightAnimationCurve;

        [SerializeField] private Sprite lowerImage;
        [SerializeField] private Sprite higherImage;

        [SerializeField] private PlayerInputManager playerInputManager;
        [SerializeField] PlayerCharge playerCharge;

        [SerializeField] private Image viewImage;
        [SerializeField] private RectTransform view;

        private bool prevHigher = false;
        private bool isHigher = false;

        // Start is called before the first frame update
        void Start()
        {
            view.gameObject.SetActive(false);
            playerInputManager.OnPressedFireButton
                .Subscribe(_ =>
                {
                    SEManager.Instance.Play(SEPath.SE_GAUGE, isLoop: true);

                    if (SceneContext.Instance.IsHardMode)
                    {
                        Vector3 relative = -view.transform.position.normalized;
                        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                        view.transform.rotation = Quaternion.Euler(0, 0, -angle - 90.0f);
                    }
                    view.gameObject.SetActive(true);
                })
                .AddTo(this);

            playerInputManager.OnCanceledFireButton
                .Subscribe(_ => view.gameObject.SetActive(false))
                .AddTo(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (!view.gameObject.activeSelf) return;
            var width = widthAnimationCurve.Evaluate(playerCharge.normalizedChargedTime);
            var height = heightAnimationCurve.Evaluate(playerCharge.normalizedChargedTime);
            view.sizeDelta = new Vector2(width, height);
            isHigher = playerCharge.normalizedChargedTime >= 0.5f;


            if (isHigher != prevHigher)
            {
                if (isHigher)
                {
                    viewImage.sprite = higherImage;
                }
                else
                {
                    viewImage.sprite = lowerImage;
                }
                prevHigher = isHigher;
            }
            //v.value = playerCharge.normalizedChargedTime;
        }
    }
}


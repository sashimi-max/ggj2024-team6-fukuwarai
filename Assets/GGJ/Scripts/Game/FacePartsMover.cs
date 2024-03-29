﻿using GGJ.Common;
using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GGJ.Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
    public class FacePartsMover : MonoBehaviour
    {
        [SerializeField] GamePlayParameterAsset gamePlayParameterAsset = default;
        [SerializeField] Transform _view;
        public Rigidbody2D rb { get; private set; }
        private RectTransform rectTransform;
        private PolygonCollider2D polygonCollider2D;
        private PlayerInputManager playerInputManager;
        private bool isEjected = false;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            rb = GetComponent<Rigidbody2D>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            playerInputManager = GetComponentInParent<PlayerInputManager>();
            var playerCharge = GetComponentInParent<PlayerCharge>();

            playerInputManager.OnCanceledFireButton
                .Where(_ => !isEjected)
                .Subscribe(_ =>
                {
                    if (!playerInputManager.IsPressedAnyFireButton) SEManager.Instance.Stop(SEPath.SE_GAUGE);
                    SEManager.Instance.Play(AudioRandomContainer.Instance.RandomSE(SEPath.SE_FACE_RELEASE1, SEPath.SE_FACE_RELEASE2));
                    EffectManager.Instance.PlayFireEffect(transform.position);
                    polygonCollider2D.enabled = true;
                    isEjected = true;
                    var moveTrans = transform.up;
                    if (SceneContext.Instance.IsHardMode)
                    {
                        moveTrans = -transform.position.normalized;
                    }
                    rectTransform.SetParent(PartsField.Instance.gameObject.transform);
                    rb.velocity = moveTrans * gamePlayParameterAsset.playerFirePower * Mathf.Clamp(playerCharge.normalizedChargedTime, 0.2f, 1.0f);
                    //rb.AddRelativeForce(Vector2.up * gamePlayParameterAsset.playerFirePower * playerCharge.normalizedChargedTime, ForceMode2D.Impulse);
                })
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            if (rb.velocity.magnitude < 0.25f && rb.velocity.magnitude > float.Epsilon)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
            }

            if (!isEjected && SceneContext.Instance.IsHardMode)
            {
                _view.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public void OnCollisionEnter2D()
        {
            EffectManager.Instance.PlayCrashEffect(transform.position);
            SEManager.Instance.Play(AudioRandomContainer.Instance.RandomSE(SEPath.SE_CRUSH1, SEPath.SE_CRUSH2, SEPath.SE_CRUSH3, SEPath.SE_CRUSH4, SEPath.SE_CRUSH5, SEPath.SE_CRUSH6, SEPath.SE_CRUSH7, SEPath.SE_CRUSH8, SEPath.SE_CRUSH9));
        }
    }
}

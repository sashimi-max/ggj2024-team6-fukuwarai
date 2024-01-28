using GGJ.Common;
using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
    public class FacePartsMover : MonoBehaviour
    {
        [SerializeField] GamePlayParameterAsset gamePlayParameterAsset = default;
        public Rigidbody2D rb { get; private set; }
        private PolygonCollider2D polygonCollider2D;
        private PlayerInputManager playerInputManager;
        private bool isEjected = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            playerInputManager = GetComponentInParent<PlayerInputManager>();
            var playerCharge = GetComponentInParent<PlayerCharge>();

            playerInputManager.OnCanceledFireButton
                .Where(_ => !isEjected)
                .Subscribe(_ =>
                {
                    SEManager.Instance.Stop(SEPath.SE_GAUGE);
                    SEManager.Instance.Play(AudioRandomContainer.Instance.RandomSE(SEPath.SE_FACE_RELEASE1, SEPath.SE_FACE_RELEASE2));
                    polygonCollider2D.enabled = true;
                    isEjected = true;
                    transform.parent = transform.parent.parent;
                    rb.velocity = transform.up * gamePlayParameterAsset.playerFirePower * Mathf.Clamp(playerCharge.normalizedChargedTime, 0.2f, 1.0f);
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
        }
    }
}

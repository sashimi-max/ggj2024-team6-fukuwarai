using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FacePartsMover : MonoBehaviour
    {
        public Rigidbody2D rb { get; private set; }
        private PlayerInputManager playerInputManager;
        private bool isEjected = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInputManager = GetComponentInParent<PlayerInputManager>();
            var playerCharge = GetComponentInParent<PlayerCharge>();

            playerInputManager.OnCanceledFireButton
                .Where(_ => !isEjected)
                .Subscribe(_ =>
                {
                    isEjected = true;
                    transform.parent = transform.parent.parent;
                    rb.AddRelativeForce(Vector2.up * 4.0f * playerCharge.normalizedChargedTime, ForceMode2D.Impulse);
                })
                .AddTo(this);
        }
    }
}

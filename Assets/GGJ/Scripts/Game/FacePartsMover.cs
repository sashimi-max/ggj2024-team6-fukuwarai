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
        public bool isFired { get; private set; } = false;

        private void Awake()
        {
            var playerType = GetComponentInParent<FacePartsHolder>().playerType;

            var inputActions = new FukuwaraiControls();

            switch (playerType)
            {
                case PlayerType.Player1:
                    inputActions.Game.Fire.canceled += OnFireButtonUp;
                    break;
                case PlayerType.Player2:
                    inputActions.Game.Fire2.canceled += OnFireButtonUp;
                    break;
                case PlayerType.Player3:
                    inputActions.Game.Fire3.canceled += OnFireButtonUp;
                    break;
                default:
                    inputActions.Game.Fire4.canceled += OnFireButtonUp;
                    break;
            }
            inputActions.Enable();
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnFireButtonUp(InputAction.CallbackContext context)
        {
            if (isFired) return;

            rb.AddRelativeForce(Vector2.up * 4.0f, ForceMode2D.Impulse);
            isFired = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using static GamePlayParameterAssetInstaller;

namespace GGJ.Game
{
    [RequireComponent(typeof(RectTransform))]
    public class FacePartsHolder : MonoBehaviour
    {
        [SerializeField] private GamePlayParameter gamePlayParameter = default;

        Keyboard current;
        RectTransform rectTransform;
        Sequence sequence;
        float width;
        // Start is called before the first frame update
        void Start()
        {
            current = Keyboard.current;
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

        // Update is called once per frame
        void Update()
        {
            if (current[Key.W].wasPressedThisFrame)
            {
                sequence.Kill();
            }
        }
    }
}

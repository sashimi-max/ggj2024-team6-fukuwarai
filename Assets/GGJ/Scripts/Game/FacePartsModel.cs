using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Game
{
    [RequireComponent(typeof(RectTransform), typeof(Rigidbody2D), typeof(Image))]
    public class FacePartsModel : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Rigidbody2D rb;
        private Image image;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rb = GetComponent<Rigidbody2D>();
            image = GetComponent<Image>();
        }

        public void Init(FacePartsData facePartsData)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            image.sprite = facePartsData.sprite;
            rb.drag = facePartsData.drag;
        }
    }
}

using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ.Game
{
    [RequireComponent(typeof(RectTransform), typeof(Rigidbody2D))]
    public class FacePartsModel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer view;
        private PolygonCollider2D polygonCollider2D;
        private RectTransform rectTransform;
        private Rigidbody2D rb;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rb = GetComponent<Rigidbody2D>();
            var playerType = GetComponentInParent<PlayerInputManager>().playerType;
            if (SceneManager.GetActiveScene().name != "Game2")
            {
                switch (playerType)
                {
                    case PlayerType.Player1:
                        break;
                    case PlayerType.Player2:
                        view.gameObject.transform.localRotation = Quaternion.Euler(Vector3.back * 90.0f);
                        break;
                    case PlayerType.Player3:
                        view.gameObject.transform.localRotation = Quaternion.Euler(Vector3.back * 180.0f);
                        break;
                    default:
                        view.gameObject.transform.localRotation = Quaternion.Euler(Vector3.back * 270.0f);
                        break;
                }
            }
            polygonCollider2D = GetComponent<PolygonCollider2D>();
        }

        public void Init(FacePartsData facePartsData)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            view.sprite = facePartsData.sprite;
            rb.drag = 5;

            gameObject.layer = LayerMask.NameToLayer(collidableObjectTypeName(facePartsData.collidableObjectType));
            if (facePartsData.collidableObjectType == CollidableObjectType.BlackEyeObject)
            {
                view.sortingOrder = 2;
            }
            if (facePartsData.collidableObjectType == CollidableObjectType.MayuObject)
            {
                view.sortingOrder = 42;
            }

            var sprite = view.sprite;
            var physicsShapeCount = sprite.GetPhysicsShapeCount();

            polygonCollider2D.pathCount = physicsShapeCount;

            var physicsShape = new List<Vector2>();

            for (var i = 0; i < physicsShapeCount; i++)
            {
                physicsShape.Clear();
                sprite.GetPhysicsShape(i, physicsShape);
                var points = physicsShape.ToArray();
                polygonCollider2D.SetPath(i, points);
            }

            polygonCollider2D.enabled = false;
        }

        private string collidableObjectTypeName(CollidableObjectType collidableObjectType)
        {
            switch (collidableObjectType)
            {
                case CollidableObjectType.WhiteEyeObject:
                    return "WhiteEyeObject";
                case CollidableObjectType.BlackEyeObject:
                    return "BlackEyeObject";
                case CollidableObjectType.MayuObject:
                    return "MayuObject";
                default:
                    return "EverythingCollidableObject";
            }
        }
    }
}

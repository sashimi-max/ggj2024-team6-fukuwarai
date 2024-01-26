using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FacePartsMover : MonoBehaviour
    {
        Keyboard current;
        Rigidbody2D rb;
        // Start is called before the first frame update
        void Start()
        {
            current = Keyboard.current;
            rb = GetComponent<Rigidbody2D>();
        }



        // Update is called once per frame
        void Update()
        {
            if (current[Key.W].wasReleasedThisFrame)
            {
                rb.AddRelativeForce(Vector2.up * 4.0f, ForceMode2D.Impulse);
            }
        }
    }
}

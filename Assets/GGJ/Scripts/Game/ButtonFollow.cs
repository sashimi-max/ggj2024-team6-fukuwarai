using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class ButtonFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _plusPos;

        // Update is called once per frame
        void Update()
        {
            transform.position = _target.transform.position + _plusPos;
        }
    }
}
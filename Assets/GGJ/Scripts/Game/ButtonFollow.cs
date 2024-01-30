using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class ButtonFollow : MonoBehaviour
    {
        private Transform target;

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            transform.position = target.transform.position + target.transform.position.normalized;
        }

        public void Init(Transform _target)
        {
            target = _target;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

namespace GGJ
{
    public class TitleScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            BGMManager.Instance.Play(BGMPath.FANTASY14);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
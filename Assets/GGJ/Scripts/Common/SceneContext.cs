using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Common
{
    public class SceneContext : SingletonMonoBehaviour<SceneContext>
    {
        protected override bool dontDestroyOnLoad { get { return true; } }

        [SerializeField] private GameMode gameMode;
        private int _faceNo = -1;

        public bool IsNormalMode => gameMode == GameMode.normal;

        public bool IsHardMode => gameMode == GameMode.hard;
        
        public int FaceNo => _faceNo;

        public void SetGameMode(GameMode _gameMode, int faceNo)
        {
            gameMode = _gameMode;
            _faceNo = faceNo;
        }
    }
}

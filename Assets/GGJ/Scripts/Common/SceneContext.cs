using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Common
{
    public class SceneContext : SingletonMonoBehaviour<SceneContext>
    {
        protected override bool dontDestroyOnLoad { get { return true; } }

        [SerializeField] private GameMode gameMode;

        public bool IsNormalMode => gameMode == GameMode.normal;

        public bool IsHardMode => gameMode == GameMode.hard;

        public void SetGameMode(GameMode _gameMode)
        {
            gameMode = _gameMode;
        }
    }
}

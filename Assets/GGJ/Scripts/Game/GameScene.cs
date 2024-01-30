using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ.Game
{
    public class GameScene : SingletonMonoBehaviour<GameScene>
    {
        protected override bool dontDestroyOnLoad { get { return false; } }

        [SerializeField] Sprite hardModeBgSprite = default;
        [SerializeField] Image nomalBgImage = default;

        [SerializeField] GameObject nomalBg = default;
        [SerializeField] GameObject resultBg = default;
        [SerializeField] GameObject buttons = default;

        private void Start()
        {
            if (SceneContext.Instance.IsHardMode)
            {
                nomalBgImage.sprite = hardModeBgSprite;
            }
        }

        public void GameOver()
        {
            buttons.SetActive(false);
            nomalBg.SetActive(false);
            resultBg.SetActive(true);
            SceneManager.LoadScene("Result", LoadSceneMode.Additive);
        }
    }
}
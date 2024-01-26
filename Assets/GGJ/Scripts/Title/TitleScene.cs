using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace GGJ
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        [SerializeField, Tooltip("ゲーム開始ボタン")]
        private Button _startButton;

        // Start is called before the first frame update
        void Start()
        {
            // BGM再生
            BGMManager.Instance.Play(BGMPath.FANTASY14);
            
            // 開始ボタン
            _startButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // SceneManager.LoadScene("Game");
                    SceneManager.LoadScene("Result");
                })
                .AddTo(gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using TransitionsPlus;
using UnityEngine.EventSystems;

namespace GGJ
{
    /// <summary>
    /// リザルトシーン
    /// </summary>
    public class ResultScene : MonoBehaviour
    {
        [SerializeField, Tooltip("タイトルに戻るボタン")]
        private Button _returnButton;

        // Start is called before the first frame update
        void Start()
        {
            //  BGM再生
            BGMManager.Instance.Play(BGMPath.HEARTBEAT01);

            // スタートボタン選択状態
            EventSystem.current.SetSelectedGameObject (_returnButton.gameObject);

            // タイトル戻るボタン
            _returnButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    // SceneManager.LoadScene("Game");
                    SceneManager.LoadScene("Title");
                })
                .AddTo(gameObject);
        }
    }
}
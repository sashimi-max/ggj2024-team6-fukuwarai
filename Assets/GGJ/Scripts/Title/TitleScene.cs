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
using UnityEngine.InputSystem;

namespace GGJ
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        [SerializeField, Tooltip("ゲーム開始ボタン")]
        private Button _startButton;

        [SerializeField, Tooltip("トランジションプロファイル")]
        private TransitionProfile _starTransitionProfile;
        
        // Start is called before the first frame update
        void Start()
        {
            // BGM再生
            BGMManager.Instance.Play(BGMPath.FANTASY14);
            
            // 開始ボタン
            _startButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Result");
                    // SceneManager.LoadScene("Game");
                    // SceneManager.LoadScene("Result");
                })
                .AddTo(gameObject);
        }
        
        private void Update()
        {
            // // 現在のキーボード情報
            // var currentKeyboard = Keyboard.current;
            //
            // // キーボード接続チェック
            // if (currentKeyboard == null)
            // {
            //     // キーボードが接続されていないと
            //     // Keyboard.currentがnullになる
            //     return;
            // }
            //
            // // Aキーの入力状態取得
            // var aKey = currentKeyboard.aKey;
            //
            // // Aキーが押された瞬間かどうか
            // if (aKey.wasPressedThisFrame)
            // {
            //     Debug.Log("Aキーが押された！");
            // }
            //
            // // Aキーが離された瞬間かどうか
            // if (aKey.wasReleasedThisFrame)
            // {
            //     Debug.Log("Aキーが離された！");
            // }
            //
            // // Aキーが押されているかどうか
            // if (aKey.isPressed)
            // {
            //     Debug.Log("Aキーが押されている！");
            // }
            //
            //
            // // 現在のマウス情報
            // var current = Mouse.current;
            //
            // // マウス接続チェック
            // if (current == null)
            // {
            //     // マウスが接続されていないと
            //     // Mouse.currentがnullになる
            //     return;
            // }
            //
            // // マウスカーソル位置取得
            // var cursorPosition = current.position.ReadValue();
            //
            // // 左ボタンの入力状態取得
            // var leftButton = current.leftButton;
            //
            // // 左ボタンが押された瞬間かどうか
            // if (leftButton.wasPressedThisFrame)
            // {
            //     Debug.Log($"左ボタンが押された！ {cursorPosition}");
            // }
            //
            // // 左ボタンが離された瞬間かどうか
            // if (leftButton.wasReleasedThisFrame)
            // {
            //     Debug.Log($"左ボタンが離された！{cursorPosition}");
            // }
            //
            // // 左ボタンが押されているかどうか
            // if (leftButton.isPressed)
            // {
            //     Debug.Log($"左ボタンが押されている！{cursorPosition}");
            // }
        }
    }
}
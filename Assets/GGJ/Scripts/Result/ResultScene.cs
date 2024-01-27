using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using TMPro;
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
        
        [SerializeField, Tooltip("もう一回ボタン")]
        private Button _retryButton;

        [SerializeField, Tooltip("ノーマルリザルト")]
        private GameObject _nomalLayer;
        
        [SerializeField, Tooltip("ウルフレイヤー")]
        private GameObject _wolfLayer;

        [SerializeField, Tooltip("NEXTボタン")]
        private Button  _nextButton;
        
        [SerializeField, Tooltip("インフォボタン")]
        private TextMeshProUGUI  _infoText;
        
        [SerializeField, Tooltip("P1ボタン")]
        private Button  _p1Button;
        
        [SerializeField, Tooltip("P2ボタン")]
        private Button  _p2Button;
        
        [SerializeField, Tooltip("P3ボタン")]
        private Button  _p3Button;
        
        [SerializeField, Tooltip("P4ボタン")]
        private Button  _p4Button;
        
        // InputSystem
        private FukuwaraiControls _fukuwaraiControls;
        private CancellationToken _cancellationToken;
        
        private void Awake()
        {
            _fukuwaraiControls = new FukuwaraiControls();
            _fukuwaraiControls.Enable();
        }

        // Start is called before the first frame update
        void Start()
        {
            //  BGM再生
            BGMManager.Instance.Play(BGMPath.HEARTBEAT01);

            var isWolf = TitleScene.PlayerStateList.Any(x => x);
            Debug.Log($"Mode IsWolf:{isWolf}");

            _nomalLayer.SetActive(!isWolf);
            _wolfLayer.SetActive(isWolf);

            if (!isWolf)
            {
                // ボタン選択状態
                EventSystem.current.SetSelectedGameObject(_returnButton.gameObject);

                // タイトル戻るボタン
                _returnButton.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        // SceneManager.LoadScene("Game");
                        SceneTitleLoad();
                    })
                    .AddTo(gameObject);
                // もう一回ボタン
                _retryButton.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        // SceneManager.LoadScene("Game");
                        SceneReLoad();
                    })
                    .AddTo(gameObject);

                ResultAsync(_cancellationToken).Forget();
            }
            else
            {
                _cancellationToken = new CancellationToken();
                WolfResultAsync(_cancellationToken).Forget();
            }
        }

        private void SceneReLoad()
        {
            SceneManager.LoadScene("Game");
        }
        
        private void SceneTitleLoad()
        {
            SceneManager.LoadScene("Title");
        }

        private async UniTask ResultAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _fukuwaraiControls.UI.Move.OnPerformedAsync<Vector2>(_cancellationToken);
            if (result.x > 0)
            {
                SceneReLoad();
            }
            else
            {
                SceneTitleLoad();
            }
        }

        private async UniTask WolfResultAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            _infoText.SetText("ウルフと思われる人を選択して下さい");
            
            _nextButton.gameObject.SetActive(false);
            
            var bp1Event = _p1Button.onClick.GetAsyncEventHandler(cancellationToken);
            var bp2Event = _p2Button.onClick.GetAsyncEventHandler(cancellationToken);
            var bp3Event = _p3Button.onClick.GetAsyncEventHandler(cancellationToken);
            var bp4Event = _p4Button.onClick.GetAsyncEventHandler(cancellationToken);

            EventSystem.current.SetSelectedGameObject(_p1Button.gameObject);
            
            var selectIndex = await UniTask.WhenAny(
            bp1Event.OnInvokeAsync(),
            bp2Event.OnInvokeAsync(),
            bp3Event.OnInvokeAsync(),
            bp4Event.OnInvokeAsync());

            var isWolfLose = false;
            var wolfIndex = 0;
            for (var i = 0; i < 4; i++)
            {
                if (TitleScene.PlayerStateList[i])
                {
                    wolfIndex = i;
                }
                if (selectIndex == i && TitleScene.PlayerStateList[i])
                {
                    isWolfLose = true;
                }
            }
            
            _p1Button.gameObject.SetActive(false);
            _p2Button.gameObject.SetActive(false);
            _p3Button.gameObject.SetActive(false);
            _p4Button.gameObject.SetActive(false);
            
            EventSystem.current.SetSelectedGameObject(_nextButton.gameObject);
            _nextButton.gameObject.SetActive(true);
            
            var winner = isWolfLose? "ウルフ": "市民";
            _infoText.SetText($"{winner}側の勝利です！\n\nウルフはPlayer{wolfIndex + 1}の方です。");

            var nextEvent = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            await UniTask.WhenAny(nextEvent.OnInvokeAsync());
            
            SceneTitleLoad();
        }
    }
}
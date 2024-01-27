using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TransitionsPlus;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks.Linq;
using DG.Tweening;

namespace GGJ
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        public static List<bool> PlayerStateList = new List<bool>(4);
        
        [SerializeField, Tooltip("ゲーム開始ボタン")]
        private Button _startButton;
        
        [SerializeField, Tooltip("オプションボタン")]
        private Button _optionButton;

        [SerializeField, Tooltip("トランジションプロファイル")]
        private TransitionProfile _starTransitionProfile;

        [SerializeField, Tooltip("テキスト")]
        private TextMeshProUGUI _infoText;
        
        [SerializeField, Tooltip("NEXTボタン")]
        private Button _nextButton;
        
        [SerializeField, Tooltip("タイトルレイヤー")]
        private GameObject _titleLayer;
        [SerializeField, Tooltip("ウルフレイヤー")]
        private GameObject _worfLayer;

        // InputSystem
        private FukuwaraiControls _fukuwaraiControls;

        private bool _startInputBlock = false;

        private void Awake()
        {
            _fukuwaraiControls = new FukuwaraiControls();
            _fukuwaraiControls.UI.Enter.canceled += (x) =>
            {
                StartGame();
            };
            _fukuwaraiControls.UI.FireI.canceled += (x) =>
            {
                // Wolf Mode
                _optionButton.onClick.Invoke();
            };
            _fukuwaraiControls.Enable();
            
            PlayerStateList = new List<bool>(4);
            for (int i = 0, count = 4; i < count; i++)
            {
                PlayerStateList.Add(false);
            }
            
            _titleLayer.SetActive(true);
            _worfLayer.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            // BGM再生
            BGMManager.Instance.Play(BGMPath.FANTASY14);

            // スタートボタン選択状態
            EventSystem.current.SetSelectedGameObject(_startButton.gameObject);

            // 開始ボタン
            _startButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    StartGame();
                })
                .AddTo(gameObject);
            // オプションボタン
            _optionButton.OnClickAsAsyncEnumerable()
                .SubscribeAwait(async (unit, token)  =>
                {
                    await WolfCheckAsync(token);
                })
                .AddTo(gameObject);
        }

        private void StartGame()
        {
            if (_startInputBlock)
            {
                return;
            }
            _startInputBlock = true;
            TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Game");
            // TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Result");
        }
        
        private async UniTask WolfCheckAsync(CancellationToken cancellationToken)
        {
            if (_startInputBlock)
            {
                return;
            }

            _startInputBlock = true;
            cancellationToken.ThrowIfCancellationRequested();
            
            _titleLayer.SetActive(false);
            _worfLayer.SetActive(true);
            
            var rindex = UnityEngine.Random.Range(0, 4);
            PlayerStateList[rindex] = true;

            // UnityEventを変換
            var buttonEvent = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            
            EventSystem.current.SetSelectedGameObject(_nextButton.gameObject);
            
            await OneCheckAsync(cancellationToken, "Player1", 0);
            await OneCheckAsync(cancellationToken, "Player2", 1);
            await OneCheckAsync(cancellationToken, "Player3", 2);
            await OneCheckAsync(cancellationToken, "Player4", 3);

            await UniTask.WhenAny(buttonEvent.OnInvokeAsync());
            
            async UniTask OneCheckAsync(CancellationToken cancellationToken, string playerName, int index)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _infoText.SetText($"{playerName}の番です。役を確認してください。");

                var step1Event = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
                await UniTask.WhenAny(step1Event.OnInvokeAsync());
            
                var pType = PlayerStateList[index]? "ウルフ": "市民";
                _infoText.SetText($"あなたの役職は{pType}です。\n確認したらボタンを押してください。");
            
                var step2Event = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
                await UniTask.WhenAny(step2Event.OnInvokeAsync());
            }
            
            _infoText.SetText($"準備が整いました！\nボタンを押したらゲームが開始します。");
            var stepEndEvent = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            await UniTask.WhenAny(stepEndEvent.OnInvokeAsync());
            
            _startInputBlock = false;
            StartGame();
        }
    }
}
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

namespace GGJ
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
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

        private static List<bool> _playerStateList = new List<bool>(4);
        
        // InputSystem
        private FukuwaraiControls _fukuwaraiControls;

        private void Awake()
        {
            _fukuwaraiControls = new FukuwaraiControls();
            _fukuwaraiControls.Enable();
            
            _playerStateList = new List<bool>(4);
            for (int i = 0, count = 4; i < count; i++)
            {
                _playerStateList.Add(false);
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
            TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Game");
        }
        
        private async UniTask WolfCheckAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            _titleLayer.SetActive(false);
            _worfLayer.SetActive(true);
            
            var rindex = UnityEngine.Random.Range(0, 4);
            _playerStateList[rindex] = true;

            // UnityEventを変換
            var buttonEvent = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            
            await OneCheckAsync(cancellationToken, "Player1", 0);
            await OneCheckAsync(cancellationToken, "Player2", 1);
            await OneCheckAsync(cancellationToken, "Player3", 2);
            await OneCheckAsync(cancellationToken, "Player4", 3);

            await UniTask.WhenAny(
                _fukuwaraiControls.UI.Enter.OnPerformedAsync(cancellationToken),
                buttonEvent.OnInvokeAsync());
            
            async UniTask OneCheckAsync(CancellationToken cancellationToken, string playerName, int index)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _infoText.SetText($"{playerName}の番です。役を確認してください。");

                await UniTask.WhenAny(
                    _fukuwaraiControls.UI.Enter.OnPerformedAsync(cancellationToken),
                    buttonEvent.OnInvokeAsync());
            
                var pType = _playerStateList[index]? "ウルフ": "市民";
                _infoText.SetText($"あなたの役職は{pType}です。\n確認したらボタンを押してください。");
            
                await UniTask.WhenAny(
                    _fukuwaraiControls.UI.Enter.OnPerformedAsync(cancellationToken),
                    buttonEvent.OnInvokeAsync());
            }
            
            _infoText.SetText($"準備が整いました！\nボタンを押したらゲームが開始します。");
            await UniTask.WhenAny(
                _fukuwaraiControls.UI.Enter.OnPerformedAsync(cancellationToken),
                buttonEvent.OnInvokeAsync());
            
            StartGame();
        }
    }
}
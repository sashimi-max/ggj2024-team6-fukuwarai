using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TransitionsPlus;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks.Linq;
using GGJ.Common;
using TMPro;
using UniRx.Triggers;
using UnityEngine.InputSystem;

namespace GGJ
{
    /// <summary>
    /// タイトルシーン
    /// </summary>
    public class TitleScene : MonoBehaviour
    {
        public static List<bool> PlayerStateList = new List<bool>(4);

        [SerializeField, Tooltip("Pゲーム開始ボタン")]
        private Button _startPButton;
        [SerializeField, Tooltip("Aゲーム開始ボタン")]
        private Button _startAButton;
        [SerializeField, Tooltip("Nゲーム開始ボタン")]
        private Button _startNButton;
        [SerializeField, Tooltip("Iゲーム開始ボタン")]
        private Button _startIButton;
        [SerializeField, Tooltip("Cゲーム開始ボタン")]
        private Button _startCButton;
        [SerializeField, Tooltip("Wゲーム開始ボタン")]
        private Button _wolfWButton;
        [SerializeField, Tooltip("戻るボタン")]
        private Button _howtoButton;
        [SerializeField, Tooltip("戻るボタン")]
        private Button _escButton;
        
        [SerializeField, Tooltip("トランジションプロファイル")]
        private TransitionProfile _starTransitionProfile;

        [SerializeField]
        private GameObject _howTo;

        [SerializeField]
        private GameObject _howToW;
        [SerializeField]
        private GameObject _p1;
        [SerializeField]
        private GameObject _p2;
        [SerializeField]
        private GameObject _p3;
        [SerializeField]
        private GameObject _p4;
        [SerializeField]
        private GameObject _wolfN;
        [SerializeField]
        private GameObject _wolfW;
        [SerializeField]
        private GameObject _wolfS;
        [SerializeField]
        private CanvasGroup _galleryCanvasGroup;
        [SerializeField, Tooltip("NEXTボタン")]
        private Button _nextButton;

        [SerializeField, Tooltip("タイトルレイヤー")]
        private GameObject _titleLayer;
        [SerializeField, Tooltip("ウルフレイヤー")]
        private GameObject _worfLayer;
        [SerializeField]
        private TextMeshProUGUI _infoText;

        private ReadOnlyReactiveProperty<bool> _enterKey = default;
        private ReadOnlyReactiveProperty<bool> _firePKey = default;
        private ReadOnlyReactiveProperty<bool> _fireAKey = default;
        private ReadOnlyReactiveProperty<bool> _fireNKey = default;
        private ReadOnlyReactiveProperty<bool> _fireIKey = default;
        private ReadOnlyReactiveProperty<bool> _fireCKey = default;
        private ReadOnlyReactiveProperty<bool> _fireWKey = default;
        private ReadOnlyReactiveProperty<bool> _escKey = default;
        
        private CancellationTokenSource _cancellationTokenSource;
        // InputSystem
        private FukuwaraiControls _fukuwaraiControls;

        private bool _startInputBlock = false;

        private bool _isHowTo = false;

        private int _faceNo = -1;

        private void Awake()
        {
            _fukuwaraiControls = new FukuwaraiControls();
            // _fukuwaraiControls.UI.Enter.canceled += OnEnter;
            _fukuwaraiControls.UI.P.canceled += OnFireP;
            _fukuwaraiControls.UI.A.canceled += OnFireA;
            _fukuwaraiControls.UI.N.canceled += OnFireN;
            _fukuwaraiControls.UI.FireI.canceled += OnFireI;
            _fukuwaraiControls.UI.C.canceled += OnFireC;
            _fukuwaraiControls.UI.FireW.canceled += OnFireW;
            _fukuwaraiControls.UI.Cancel.canceled += OnCancel;
            _fukuwaraiControls.Enable();

            _escButton.gameObject.SetActive(false);
            
            PlayerStateList = new List<bool>(4);
            for (int i = 0, count = 4; i < count; i++)
            {
                PlayerStateList.Add(false);
            }

            _titleLayer.SetActive(true);
            _worfLayer.SetActive(false);
        }

        private void OnDisable()
        {
            // _fukuwaraiControls.UI.Enter.canceled -= OnEnter;
            _fukuwaraiControls.UI.P.canceled -= OnFireP;
            _fukuwaraiControls.UI.A.canceled -= OnFireA;
            _fukuwaraiControls.UI.N.canceled -= OnFireN;
            _fukuwaraiControls.UI.FireI.canceled -= OnFireI;
            _fukuwaraiControls.UI.C.canceled -= OnFireC;
            _fukuwaraiControls.UI.FireW.canceled -= OnFireW;
            _fukuwaraiControls.UI.Cancel.canceled -= OnCancel;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            // BGM再生
            BGMManager.Instance.Play(BGMPath.BGM_TITLE, delay: 3.0f, isLoop: true);

            SEManager.Instance.Play(SEPath.SE_TITLE);

            // スタートボタン選択状態
            EventSystem.current.SetSelectedGameObject(_startPButton.gameObject);

            // 開始ボタン
            _startPButton
                .OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _faceNo = -1;
                    PushPANC();
                })
                .AddTo(this);
            _startAButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _faceNo = 0;
                    PushPANC();
                })
                .AddTo(this);
            _startNButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _faceNo = 1;
                    PushPANC();
                })
                .AddTo(this);
            _startCButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _galleryCanvasGroup.alpha = _galleryCanvasGroup.alpha > 0.5f ? 0f : 1;
                })
                .AddTo(this);
            _howtoButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    Debug.Log($"START");
                    StartGame();                    
                })
                .AddTo(this);
            _startIButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    Debug.Log($"START GAME2");
                    StartGame2();
                })
                .AddTo(this);
            // ウルフボタン
            _wolfWButton.OnClickAsAsyncEnumerable()
                .SubscribeAwait(async (unit, _) =>
                {
                    Debug.Log($"WOLF START _isHowTo:{_isHowTo}");
                    if (_isHowTo)
                    {
                        return;
                    }

                    _cancellationTokenSource = new CancellationTokenSource();
                    await WolfCheckAsync(_cancellationTokenSource.Token);
                })
                .AddTo(this);
            
            // 戻る
            _escButton.OnClickAsObservable()
                .ThrottleFirst(System.TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    Debug.Log($"ESC _isHowTo:{_isHowTo}");
                    if (!_escButton.gameObject.activeSelf)
                    {
                        return;
                    }
                    Debug.Log($"ESC _isHowTo:{_isHowTo}");
                    SEManager.Instance.Play(SEPath.SE_RETURN_TITLE);
                    
                    _wolfWButton.gameObject.SetActive(true);
                    _startPButton.gameObject.SetActive(true);
                    _startAButton.gameObject.SetActive(true);
                    _startNButton.gameObject.SetActive(true);
                    _startIButton.gameObject.SetActive(true);
                    _startCButton.gameObject.SetActive(true);
                    _escButton.gameObject.SetActive(false);
                    
                    _startInputBlock = false;
                    EventSystem.current.SetSelectedGameObject(_startPButton.gameObject);
                    
                    if (_isHowTo)
                    {
                        _howTo.SetActive(false);
                        _isHowTo = false;
                        return;
                    }

                    if (_cancellationTokenSource != null)
                    {
                        _cancellationTokenSource.Cancel();
                        _worfLayer.SetActive(false);
                    }
                    _titleLayer.SetActive(true);
                })
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var name = EventSystem.current.currentSelectedGameObject?.name ?? "";
                    name = name switch
                    {
                        "P" => "ゲーム開始",
                        "A" => "龍でゲーム開始",
                        "N" => "ベルバラでゲーム開始",
                        "I" => "ハードモードでゲーム開始",
                        "C" => "ギャラリー表示切替",
                        "W" => "ウルフモードでゲーム開始",
                        "HOWTO" => "",
                        "ESC" => "戻る",
                        _ => ""
                    };
                    _infoText.SetText(name);
                })
                .AddTo(this);
        }

        private void PushPANC()
        {
            if (_worfLayer.activeSelf)
            {
                return;
            }
            if (!_isHowTo)
            {
                Debug.Log($"START HOWTO");
                SEManager.Instance.Play(SEPath.SE_TITLE_START_BUTTON);
                _howTo.SetActive(true);
                _isHowTo = true;
                        
                _wolfWButton.gameObject.SetActive(false);
                _startPButton.gameObject.SetActive(false);
                _startAButton.gameObject.SetActive(false);
                _startNButton.gameObject.SetActive(false);
                _startIButton.gameObject.SetActive(false);
                _startCButton.gameObject.SetActive(false);
                _escButton.gameObject.SetActive(true);
                        
                EventSystem.current.SetSelectedGameObject(_howtoButton.gameObject);
            }
        }

        private void OnFireP(InputAction.CallbackContext context)
        {
            Debug.Log($"P");
            if (_isHowTo)
            {
                StartGame();
            }
            _startPButton.onClick.Invoke();
        }
        
        private void OnFireA(InputAction.CallbackContext context)
        {
            Debug.Log($"A");
            if (_isHowTo)
            {
                StartGame();
            }
            _startAButton.onClick.Invoke();
        }
        
        private void OnFireN(InputAction.CallbackContext context)
        {
            Debug.Log($"N");
            if (_isHowTo)
            {
                StartGame();
            }
            _startNButton.onClick.Invoke();
        }
        
        private void OnFireI(InputAction.CallbackContext context)
        {
            Debug.Log($"I");
            if (_isHowTo)
            {
                StartGame2();
            }
            _startIButton.onClick.Invoke();
        }
        private void OnFireC(InputAction.CallbackContext context)
        {
            Debug.Log($"C");
            _startCButton.onClick.Invoke();
        }
        
        private void OnFireW(InputAction.CallbackContext context)
        {
            Debug.Log($"W");
            _wolfWButton.onClick.Invoke();
        }
        
        private void OnCancel(InputAction.CallbackContext context)
        {
            Debug.Log($"Cancel");
            _escButton.onClick.Invoke();
        }
        
        private void StartGame()
        {
            if (_startInputBlock)
            {
                return;
            }
            _startInputBlock = true;
            SEManager.Instance.Play(SEPath.SE_CLOSE_RULE);
            SceneContext.Instance.SetGameMode(GameMode.normal, _faceNo);
            TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Game");
        }

        private void StartGame2()
        {
            if (_startInputBlock)
            {
                return;
            }
            _startInputBlock = true;
            SEManager.Instance.Play(SEPath.SE_CLOSE_RULE);
            SceneContext.Instance.SetGameMode(GameMode.hard, _faceNo);
            TransitionAnimator.Start(_starTransitionProfile, sceneNameToLoad: "Game");
        }

        private async UniTask WolfCheckAsync(CancellationToken cancellationToken)
        {
            if (_startInputBlock)
            {
                return;
            }

            SEManager.Instance.Play(SEPath.SE_WOLF_START);
            _startInputBlock = true;
            cancellationToken.ThrowIfCancellationRequested();

            _titleLayer.SetActive(false);
            _worfLayer.SetActive(true);

            _wolfWButton.gameObject.SetActive(false);
            _startPButton.gameObject.SetActive(false);
            _startAButton.gameObject.SetActive(false);
            _startNButton.gameObject.SetActive(false);
            _startIButton.gameObject.SetActive(false);
            _startCButton.gameObject.SetActive(false);
            _escButton.gameObject.SetActive(true);
            
            _p1.SetActive(false);
            _p2.SetActive(false);
            _p3.SetActive(false);
            _p4.SetActive(false);
            _wolfN.SetActive(false);
            _wolfW.SetActive(false);
            _howToW.SetActive(false);
            _wolfS.SetActive(false);
            
            var rindex = UnityEngine.Random.Range(0, 4);
            PlayerStateList[rindex] = true;

            EventSystem.current.SetSelectedGameObject(_nextButton.gameObject);

            _howToW.SetActive(true);

            var step0Event = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            await UniTask.WhenAny(step0Event.OnInvokeAsync());

            _howToW.SetActive(false);

            await OneCheckAsync(cancellationToken, "Player1", 0, _p1);
            await OneCheckAsync(cancellationToken, "Player2", 1, _p2);
            await OneCheckAsync(cancellationToken, "Player3", 2, _p3);
            await OneCheckAsync(cancellationToken, "Player4", 3, _p4);

            async UniTask OneCheckAsync(CancellationToken cancellationToken, string playerName, int index, GameObject playerInfo)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _p1.SetActive(false);
                _p2.SetActive(false);
                _p3.SetActive(false);
                _p4.SetActive(false);
                _wolfN.SetActive(false);
                _wolfW.SetActive(false);

                playerInfo.SetActive(true);

                var step1Event = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
                await UniTask.WhenAny(step1Event.OnInvokeAsync());
                playerInfo.SetActive(false);
                if (PlayerStateList[index])
                {
                    _wolfW.SetActive(true);
                }
                else
                {
                    _wolfN.SetActive(true);
                }


                var step2Event = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
                await UniTask.WhenAny(step2Event.OnInvokeAsync());
                _wolfN.SetActive(false);
                _wolfW.SetActive(false);
            }

            _wolfS.SetActive(true);

            var stepEndEvent = _nextButton.onClick.GetAsyncEventHandler(cancellationToken);
            await UniTask.WhenAny(stepEndEvent.OnInvokeAsync());

            _startInputBlock = false;

            StartGame();
        }
    }
}
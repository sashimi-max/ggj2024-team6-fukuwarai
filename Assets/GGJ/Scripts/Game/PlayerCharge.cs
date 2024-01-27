using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GamePlayParameterAssetInstaller;

public class PlayerCharge : MonoBehaviour
{
    private PlayerType playerType;
    public GamePlayParameter gamePlayParameter;

    public float chargedTime { get; private set; } = 0.0f;

    // 0~1の値を行ったり来たり
    public float normalizedChargedTime => Mathf.PingPong(chargedTime, gamePlayParameter.playerChargeSeconds) / gamePlayParameter.playerChargeSeconds;

    private FukuwaraiControls inputActions;

    private void Awake()
    {
        inputActions = new FukuwaraiControls();
        inputActions.Enable();
    }

    private void Start()
    {
        playerType = GetComponent<PlayerInputManager>().playerType;
    }

    private void Update()
    {
        switch (playerType)
        {
            case PlayerType.Player1:
                if (inputActions.Game.Fire.IsPressed()) chargedTime += Time.deltaTime;
                break;
            case PlayerType.Player2:
                if (inputActions.Game.Fire2.IsPressed()) chargedTime += Time.deltaTime;
                break;
            case PlayerType.Player3:
                if (inputActions.Game.Fire3.IsPressed()) chargedTime += Time.deltaTime;
                break;
            default:
                if (inputActions.Game.Fire4.IsPressed()) chargedTime += Time.deltaTime;
                break;
        }
    }
}

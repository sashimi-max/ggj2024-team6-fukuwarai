using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePlayParameterData", menuName = "ScriptableObjects/CreateGamePlayParameter")]
public class GamePlayParameterAssetInstaller : ScriptableObject
{
    [System.Serializable]
    public class GamePlayParameter
    {
        public float remainingTime = 10.0f;
        public float playerBarMoveTime = 1.0f;
    }

    [SerializeField]
    GamePlayParameter gamePlayParameter = default;
}

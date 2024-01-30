using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Game
{
    public class PlayerBarFactory : MonoBehaviour
    {
        [SerializeField] Transform[] barSpawnPoints = default;

        [SerializeField] PlayerBar playerBarPrefab = default;

        [SerializeField] Image[] keyImages = default;

        [SerializeField] FacePartsSpawner facePartsSpawner = default;

        private const int PLAYER_COUNT = 4;

        // Start is called before the first frame update
        void Awake()
        {
            for (var i = 0; i < PLAYER_COUNT; i++)
            {
                var playerBar = Instantiate(playerBarPrefab, barSpawnPoints[i]);
                playerBar.Init((PlayerType)i, keyImages[i]);

                var playerHolder = playerBar.GetFacePartsHolder();
                facePartsSpawner.AddFacePartsHolder(playerHolder);

                if (SceneContext.Instance.IsHardMode)
                {
                    var buttonFollow = keyImages[i].gameObject.AddComponent<ButtonFollow>();
                    buttonFollow.Init(playerHolder.transform);
                }
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.Game
{
    public class FacePartsSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<FacePartsHolder> facePartsHolders = new List<FacePartsHolder>();

        [SerializeField] FacePartsModel FacePartsPrefab = default;

        [SerializeField] FacePartsAsset facePartsAsset = default;

        private IEnumerable<FacePartsMover> movers;
        private IEnumerable<PlayerInputManager> inputManagers;
        bool isGameOver = false;

        private void Start()
        {
            SpawnFaceParts(0);
        }

        public void SpawnFaceParts(int index)
        {
            var faceParts = facePartsAsset.facePartsSet[index];
            var offset = Random.Range(0, facePartsHolders.Count);
            for (var i = 0; i < facePartsHolders.Count; i++)
            {
                var obj = Instantiate(FacePartsPrefab, facePartsHolders[i].transform);
                obj.Init(faceParts.facePartsData[(i + offset) % facePartsHolders.Count]);
            }

            movers = facePartsHolders.Select(obj => obj.GetComponentInChildren<FacePartsMover>());
            inputManagers = facePartsHolders.Select(obj => obj.GetComponentInParent<PlayerInputManager>());
        }

        private void FixedUpdate()
        {
            if (isGameOver || movers == null || movers.Count() == 0 || inputManagers == null || inputManagers.Count() == 0) return;

            var allFireds = inputManagers.All(inputs => inputs.isFired);

            if (!allFireds) return;

            var moveEnds = movers.All(mover => mover.rb.velocity.magnitude < 0.1f);

            if (moveEnds)
            {
                gameOver();
            }
        }

        private void gameOver()
        {
            Debug.Log("gameover!");
            isGameOver = true;

            SceneManager.LoadScene("Result", LoadSceneMode.Additive);
        }
    }
}

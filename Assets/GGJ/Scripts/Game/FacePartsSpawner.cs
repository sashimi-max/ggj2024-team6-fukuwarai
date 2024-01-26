using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Game
{
    public class FacePartsSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<FacePartsHolder> facePartsHolders = new List<FacePartsHolder>();

        [SerializeField] FacePartsModel FacePartsPrefab = default;

        [SerializeField] FacePartsAsset facePartsAsset = default;

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
        }
    }
}

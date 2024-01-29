using Cysharp.Threading.Tasks;
using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Game
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        protected override bool dontDestroyOnLoad { get { return false; } }

        [SerializeField] CrashEffectPool[] crashEffectPools = default;

        [SerializeField] FireEffectPool[] fireEffectPools = default;

        public void PlayCrashEffect(Vector2 position)
        {
            var index = Random.Range(0, crashEffectPools.Length);
            crashEffectPools[index].SpawnEffect(position).Forget();
        }

        public void PlayFireEffect(Vector2 position)
        {
            var index = Random.Range(0, fireEffectPools.Length);
            fireEffectPools[index].SpawnEffect(position).Forget();
        }
    }
}
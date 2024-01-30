using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireEffectPool : MonoBehaviour
{
    [SerializeField] private FireEffect fireEffect = default;

    ObjectPool<FireEffect> pool;

    void Awake()
    {
        pool = new ObjectPool<FireEffect>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    FireEffect OnCreatePooledObject()
    {
        return Instantiate(fireEffect, transform);
    }

    void OnGetFromPool(FireEffect effect)
    {
        effect.gameObject.SetActive(true);
    }

    void OnReleaseToPool(FireEffect effect)
    {
        effect.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(FireEffect effect)
    {
        Destroy(effect.gameObject);
    }

    public async UniTask SpawnEffect(Vector2 position)
    {
        FireEffect effect = pool.Get();
        effect.transform.position = position;

        await effect.Play();
        ReleaseGameObject(effect);
    }

    public void ReleaseGameObject(FireEffect obj)
    {
        pool.Release(obj);
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject playerLaserPrefab;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] GameObject destroyEffectPrefab;
    List<GameObject> playerLasers = new List<GameObject>(5);
    List<GameObject> enemyLasers = new List<GameObject>(15);
    List<GameObject> destroyEffects = new List<GameObject>(5);

    public enum ObjectType
    {
        PlayerLaser,
        EnemyLaser,
        DestroyEffect
    }

    private GameObject GetPrefabForType(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.PlayerLaser:
                return playerLaserPrefab;

            case ObjectType.EnemyLaser:
                return enemyLaserPrefab;

            case ObjectType.DestroyEffect:
                return destroyEffectPrefab;
        }

        return null;
    }

    private List<GameObject> GetListForType(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.PlayerLaser:
                return playerLasers;

            case ObjectType.EnemyLaser:
                return enemyLasers;

            case ObjectType.DestroyEffect:
                return destroyEffects;
        }

        return null;
    }

    public GameObject RequestLaserObject(ObjectType type)
    {
        GameObject obj = null;

        List<GameObject> list = GetListForType(type);
        GameObject prefab = GetPrefabForType(type);

        for (int i = 0; i < list.Count; i++)
        {

            if (!list[i].activeInHierarchy)
            {
                obj = list[i];
                obj.SetActive(true);
                break;
            }
        }

        if (!obj)
        {
            obj = Instantiate(prefab);
            obj.name = prefab.name;
            obj.transform.SetParent(transform);
            list.Add(obj);
        }

        return obj;
    }
}

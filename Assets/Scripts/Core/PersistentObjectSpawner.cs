using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned == false)
            {
                SpawnPersistentObjects();
                hasSpawned = true;
            }
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}

using System;
using Unity.Netcode;
using UnityEngine;

namespace LittleSword.Network
{
    public class NetworkGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += SpawnEnemies;
        }

        private void SpawnEnemies()
        {
            if (!NetworkManager.Singleton.IsServer) return;

            foreach(var spawnPoint in spawnPoints)
            {
                var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                //네트워크 객체로 등록
                enemy.GetComponent<NetworkObject>().Spawn();
            }
        }
    }

}
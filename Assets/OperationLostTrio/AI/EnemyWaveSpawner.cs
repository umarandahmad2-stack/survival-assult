using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace OperationLostTrio
{
    /// <summary>Spawns escalating waves around the arena. Assign an enemy prefab and player target.</summary>
    public class EnemyWaveSpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public Transform player;
        public Transform[] spawnPoints;
        public int firstWaveCount = 3;
        public int extraEnemiesPerWave = 2;
        public float delayBetweenWaves = 8f;
        public float spawnInterval = 0.8f;
        public int maxAliveEnemies = 18;

        public int CurrentWave { get; private set; }
        public int AliveEnemies { get; private set; }
        public bool IsRunning { get; private set; }
        Coroutine routine;

        public void StartWaves()
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(WaveLoop());
        }

        IEnumerator WaveLoop()
        {
            IsRunning = true;
            while (true)
            {
                CurrentWave++;
                int count = Mathf.Min(firstWaveCount + (CurrentWave - 1) * extraEnemiesPerWave, maxAliveEnemies);
                Debug.Log("Incoming enemy wave " + CurrentWave + " — hostiles: " + count);
                int spawned = 0;
                while (spawned < count)
                {
                    if (AliveEnemies < maxAliveEnemies)
                    {
                        SpawnOne();
                        spawned++;
                    }
                    yield return new WaitForSeconds(spawnInterval);
                }
                while (AliveEnemies > 0) yield return new WaitForSeconds(0.5f);
                yield return new WaitForSeconds(delayBetweenWaves);
            }
        }

        void SpawnOne()
        {
            if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);
            EnemyPatrol ai = enemy.GetComponent<EnemyPatrol>();
            if (ai != null && player != null) ai.target = player;
            Health health = enemy.GetComponent<Health>();
            AliveEnemies++;
            if (health != null) health.onDeath.AddListener(() => {
                AliveEnemies = Mathf.Max(0, AliveEnemies - 1);
                Destroy(enemy, 2f);
            });
            else
            {
                // Prefabs should have Health so wave completion can be tracked correctly.
                Debug.LogWarning("Enemy prefab needs a Health component.");
                AliveEnemies = Mathf.Max(0, AliveEnemies - 1);
            }
        }

        public void StopWaves()
        {
            if (routine != null) StopCoroutine(routine);
            routine = null; IsRunning = false;
        }

        void OnDisable() { StopWaves(); }
    }
}
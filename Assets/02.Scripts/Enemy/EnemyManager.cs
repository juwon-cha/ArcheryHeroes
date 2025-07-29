using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 스폰, 웨이브 관리 매니저
public class EnemyManager : Singleton<EnemyManager>
{
    private Coroutine waveRoutine;

    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] private List<Rect> spawnAreas;
    [SerializeField] private Color gizmoColor = Color.red;

    private List<EnemyController> activeEnemies = new List<EnemyController>();

    private bool enemySpawnComplete;

    [SerializeField] private int timeBetweenSpawns = 5;
    [SerializeField] private int timeBetweenWaves = 5;

    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            GameManager.Instance.EndOfWave();
            return;
        }

        if (waveRoutine != null)
        {
            StopCoroutine(waveRoutine);
        }

        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplete = false;

        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < waveCount; ++i)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        enemySpawnComplete = true;
    }

    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs or spawn areas defined.");
            return;
        }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 몬스터 생성 영역 랜덤으로 만듦
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        Vector2 randomPosition = new Vector2(
            Random.Range(randomArea.xMin, randomArea.xMax),
            Random.Range(randomArea.yMin, randomArea.yMax)
        );

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, randomPosition.y), Quaternion.identity);
        EnemyController enemyController = spawnEnemy.GetComponent<EnemyController>();
        enemyController.Init(GameManager.Instance.Player.transform);

        activeEnemies.Add(enemyController);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null)
        {
            return;
        }

        Gizmos.color = gizmoColor;
        foreach (Rect area in spawnAreas)
        {
            Vector3 center = new Vector3(area.x + area.width / 2, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, area.height);

            Gizmos.DrawCube(center, size);
        }
    }

    public void RemoveEnemyOnDead(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        if (activeEnemies.Count == 0 && enemySpawnComplete)
        {
            GameManager.Instance.EndOfWave();
        }
    }
}

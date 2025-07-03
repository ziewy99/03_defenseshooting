using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public GameObject[] Enemy_Prefab; // 0: Orc, 1: EliteOrc, 2: OrcRider

    public float spawnInterval = 5.0f;
    public float minY = -4.5f;
    public float maxY = -2.5f;

    private float timer;
    private float elapsedTime;
    private bool hasSpawnedOrcRider = false;

    private int orcCount = 0;
    private int eliteCount = 0;

    void Update()
    {
        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (!hasSpawnedOrcRider && elapsedTime >= 60f)
            {
                SpawnSpecificEnemy(2); // OrcRider
                hasSpawnedOrcRider = true;
                ResetCounts();
            }
            else
            {
                int index = GetControlledEnemyIndex();
                SpawnSpecificEnemy(index);
            }
        }
    }

    private int GetControlledEnemyIndex()
    {
        // 強制的にEliteOrcを出す
        if (orcCount >= 3)
        {
            return 1;
        }

        // OrcRider解禁済みでEliteOrcが3回連続ならRiderを出す
        if (hasSpawnedOrcRider && eliteCount >= 3)
        {
            return 2;
        }

        return GetRandomEnemyIndex();
    }

    private int GetRandomEnemyIndex()
    {
        int rand;

        if (!hasSpawnedOrcRider)
        {
            // Orc:EliteOrc = 4:1
            rand = Random.Range(0, 5);
            return rand < 4 ? 0 : 1;
        }
        else
        {
            // Orc:EliteOrc:OrcRider = 2:2:1
            rand = Random.Range(0, 5);
            if (rand < 2) return 0;
            else if (rand < 4) return 1;
            else return 2;
        }
    }

    private void SpawnSpecificEnemy(int index)
    {
        if (index < 0 || index >= Enemy_Prefab.Length) return;

        GameObject enemyPrefab = Enemy_Prefab[index];
        Vector3 spawnPosition = GetRightEdgePosition();
        spawnPosition.y = Random.Range(minY, maxY);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Transform target = GameObject.Find("TowerPoint")?.transform;

        if (newEnemy.TryGetComponent<E_Orc_Script>(out var orc))
        {
            orc.targetObject = target;
            orcCount++;
            eliteCount = 0;
        }
        else if (newEnemy.TryGetComponent<E_EliteOrc_Script>(out var elite))
        {
            elite.targetObject = target;
            eliteCount++;
            orcCount = 0;
        }
        else if (newEnemy.TryGetComponent<E_OrcRider_Script>(out var rider))
        {
            rider.targetObject = target;
            ResetCounts();
        }
    }

    private void ResetCounts()
    {
        orcCount = 0;
        eliteCount = 0;
    }

    private Vector3 GetRightEdgePosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 viewportPoint = new Vector3(1f, 0.5f, 0f);
        float distance = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 worldPoint = mainCamera.ViewportToWorldPoint(new Vector3(viewportPoint.x, viewportPoint.y, distance));
        worldPoint.z = 0f;
        return worldPoint;
    }
}

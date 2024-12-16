using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject tilePrefab;         // Normal blok prefab
    public GameObject tileUpPrefab;       // Üst blok prefab
    public Transform topBound;            // Üst referans noktası
    public Transform bottomBound;         // Alt referans noktası
    public Transform spawnStartPoint;     // Spawn başlangıç noktası
    public GameObject level;              // Level GameObject
    public float moveSpeed = 2f;          // Hareket hızı
    public float destroyXPosition = -10f; // Yok olma pozisyonu
    public int minTiles = 2;              // Minimum obje sayısı
    public int maxTiles = 5;              // Maksimum obje sayısı

    public float spawnInterval = 3f;      // Spawn aralığı

    private void Start()
    {
        if (level == null)
        {
            Debug.LogError("Level GameObject is not assigned in the Inspector.");
            return;
        }

        StartCoroutine(SpawnObstacleGroups());
    }

    private IEnumerator SpawnObstacleGroups()
    {
        while (true)
        {
            SpawnObstacleSet();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObstacleSet()
    {
        // ObstacleSet GameObject oluştur
        GameObject obstacleSet = new GameObject("ObstacleSet");
        obstacleSet.transform.position = spawnStartPoint.position; // Sağdan başlat
        obstacleSet.transform.SetParent(level.transform);

        // Alt ve üst setleri oluştur
        SpawnBottomSet(obstacleSet);
        SpawnUpSet(obstacleSet);

        // Obstacle set hareket ettir
        obstacleSet.AddComponent<ObstacleMover>().Initialize(moveSpeed, destroyXPosition);
    }

    private void SpawnBottomSet(GameObject parent)
    {
        int tileCount = Random.Range(minTiles, maxTiles + 1);
        Vector3 position = new Vector3(spawnStartPoint.position.x, bottomBound.position.y, -2); // Sabit Z değeri

        for (int i = 0; i < tileCount - 1; i++)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(parent.transform);

            // Z eksenini değiştirme, ancak artık sabit -2 olacak
            position.z = -2;  // Z eksenini -2 olarak ayarla

            // SpriteRenderer sıralamasını eşitle
            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingLayerName = "Default";
                renderer.sortingOrder = 0;
            }

            // Y eksenine göre pozisyonu güncelle
            position.y += tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        // Son obje, tileUpPrefab
        GameObject topTile = Instantiate(tileUpPrefab, position, Quaternion.identity);
        topTile.transform.SetParent(parent.transform);

        // Z eksenini sabitle
        topTile.transform.position = new Vector3(topTile.transform.position.x, topTile.transform.position.y, -2); // Z değeri -2 olacak
    }

    private void SpawnUpSet(GameObject parent)
    {
        // Rastgele kaç tile yerleştirileceğini belirle
        int tileCount = Random.Range(minTiles, maxTiles + 1);

        // İlk tile için başlangıç pozisyonu (üst referans)
        Vector3 position = new Vector3(spawnStartPoint.position.x, topBound.position.y, -2);  // Sabit Z değeri

        // Normal tilePrefab yerleştir
        for (int i = 0; i < tileCount - 1; i++)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(parent.transform);

            // Pozisyonu güncelle: Obje yüksekliği kadar aşağı kaydır
            position.y -= tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        // En alta ters çevrilmiş tileUpPrefab yerleştir
        GameObject bottomTile = Instantiate(tileUpPrefab, position, Quaternion.Euler(0, 0, 180)); // 180 derece döndür
        bottomTile.transform.SetParent(parent.transform);

        // Z pozisyonunu sabitle
        bottomTile.transform.position = new Vector3(bottomTile.transform.position.x, bottomTile.transform.position.y, -2);
    }
}
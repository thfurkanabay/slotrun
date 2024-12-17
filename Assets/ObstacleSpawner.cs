using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tilePrefab;         // Normal blok prefab
    public GameObject tileUpPrefab;       // Üst blok prefab
    public GameObject collectablePrefab;
    [Header("Variables ")]
    public Transform topBound;            // Üst referans noktası
    public Transform bottomBound;         // Alt referans noktası
    public Transform spawnStartPoint;     // Spawn başlangıç noktası
    public GameObject level;              // Level GameObject
    public float moveSpeed = 2f;          // Hareket hızı
    public float destroyXPosition = -10f; // Yok olma pozisyonu
    public int minTiles = 2;              // Minimum obje sayısı
    public int maxTiles = 5;              // Maksimum obje sayısı

    public float spawnInterval = 3f;      // Spawn aralığı

    public static ObstacleSpawner Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        if (level == null)
        {
            Debug.LogError("Level GameObject is not assigned in the Inspector.");
            return;
        }
    }

    public IEnumerator SpawnObstacleGroups()
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

        // Alt seti oluştur ve üst sınırı al
        float bottomEndY = SpawnBottomSet(obstacleSet);

        // Üst seti oluştur ve alt sınırı al
        float topStartY = SpawnUpSet(obstacleSet);

        // Collectable nesne ekle (boşluk içinde)
        if (topStartY > bottomEndY) // Boşluk kontrolü
        {
            SpawnCollectable(obstacleSet, bottomEndY, topStartY);
        }

        // Obstacle set hareket ettir
        obstacleSet.AddComponent<ObstacleMover>().Initialize(moveSpeed, destroyXPosition);
    }

    private float SpawnBottomSet(GameObject parent)
    {
        int tileCount = Random.Range(minTiles, maxTiles + 1);
        Vector3 position = new Vector3(spawnStartPoint.position.x, bottomBound.position.y, -2); // Sabit Z değeri

        for (int i = 0; i < tileCount - 1; i++)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(parent.transform);

            position.y += tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        // Son obje, tileUpPrefab
        GameObject topTile = Instantiate(tileUpPrefab, position, Quaternion.identity);
        topTile.transform.SetParent(parent.transform);

        // Yüksekliği geri döndür (topTile'ın üst kısmı)
        return position.y + tileUpPrefab.GetComponent<Renderer>().bounds.size.y;
    }
    private float SpawnUpSet(GameObject parent)
    {
        int tileCount = Random.Range(minTiles, maxTiles + 1);
        Vector3 position = new Vector3(spawnStartPoint.position.x, topBound.position.y, -2); // Sabit Z değeri

        for (int i = 0; i < tileCount - 1; i++)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(parent.transform);

            position.y -= tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        // En alta ters çevrilmiş tileUpPrefab yerleştir
        GameObject bottomTile = Instantiate(tileUpPrefab, position, Quaternion.Euler(0, 0, 180)); // 180 derece döndür
        bottomTile.transform.SetParent(parent.transform);

        // Yüksekliği geri döndür (bottomTile'ın alt kısmı)
        return position.y - bottomTile.GetComponent<Renderer>().bounds.size.y;
    }
    private void SpawnCollectable(GameObject parent, float bottomEndY, float topStartY)
    {
        // Alt setin üst bitiş y pozisyonu ve üst setin alt başlangıç y pozisyonu arasında spawn
        float randomY = Random.Range(bottomEndY, topStartY);

        // Pozisyon ayarla (Z ekseni sabit -2 olacak)
        Vector3 position = new Vector3(spawnStartPoint.position.x, randomY, -2);

        // Collectable nesnesi oluştur
        GameObject collectable = Instantiate(collectablePrefab, position, Quaternion.identity);
        collectable.transform.SetParent(parent.transform);

        // Rastgele bir sprite ata
        SpriteRenderer renderer = collectable.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = GetRandomSprite();
        }
    }
    private Sprite GetRandomSprite()
    {

        List<Sprite> goalObjectImagelist = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex].goalObjectImagelist; // create new list
        if (goalObjectImagelist != null && goalObjectImagelist.Count > 0)
        {
            int randomIndex = Random.Range(0, goalObjectImagelist.Count);
            return goalObjectImagelist[randomIndex];
        }
        else
        {
            Debug.LogWarning("goalObjectImagelist boş veya tanımlı değil!");
            return null;
        }
    }
}
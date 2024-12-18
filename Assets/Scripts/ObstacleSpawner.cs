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
    public GameObject slotPrefab;
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

    public int selectedSlotItemIndex; // Seçilen slot item'ı

    // Slot tarafından bildirilen öğeyi al

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
    private void Update()
    {

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
            SpawnSlot(obstacleSet, bottomEndY, topStartY);
        }

        // Obstacle set hareket ettir
        obstacleSet.AddComponent<ObstacleMover>().Initialize(moveSpeed, destroyXPosition);
    }

    private float SpawnBottomSet(GameObject parent)
    {
        int tileCount = Random.Range(minTiles, maxTiles + 1);
        Vector3 position = new Vector3(spawnStartPoint.position.x, bottomBound.position.y, -3); // Sabit Z değeri

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
        Vector3 position = new Vector3(spawnStartPoint.position.x, topBound.position.y, -3);

        for (int i = 0; i < tileCount - 1; i++)
        {
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
            tile.transform.SetParent(parent.transform);

            position.y -= tilePrefab.GetComponent<Renderer>().bounds.size.y;
        }

        // En alta ters çevrilmiş tileUpPrefab yerleştir
        GameObject bottomTile = Instantiate(tileUpPrefab, position, Quaternion.Euler(0, 0, 180));
        bottomTile.transform.SetParent(parent.transform);

        // Yüksekliği geri döndür (bottomTile'ın alt kısmı)
        return position.y - bottomTile.GetComponent<Renderer>().bounds.size.y;
    }
    private void SpawnSlot(GameObject parent, float bottomEndY, float topStartY)
    {
        float centerOfYBound = (bottomEndY + topStartY) / 2;
        Vector3 positionSlot = new Vector3(spawnStartPoint.position.x, centerOfYBound, -2);

        // Spawn the slot
        GameObject slot = Instantiate(slotPrefab, positionSlot, Quaternion.identity);
        Slot slotScript = slot.GetComponent<Slot>();

        if (slotScript != null)
        {
            // Start coroutine for the slot
            slotScript.StartCoroutine(slotScript.StopAndDestroySlot());
        }

        // Slot rotation adjustment
        slot.transform.rotation = Quaternion.Euler(0, -90, 0);
        slot.transform.SetParent(parent.transform);

        // Wait until the slot is finished before spawning collectable
        StartCoroutine(WaitForSlotToFinishAndSpawnCollectable(slotScript, parent, bottomEndY, topStartY));
        //Spin Sound Effect
        SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.Spin);

    }

    // Coroutine to wait for the slot to finish
    private IEnumerator WaitForSlotToFinishAndSpawnCollectable(Slot slotScript, GameObject parent, float bottomEndY, float topStartY)
    {
        // Wait for the isSlotFinish flag to become true
        while (!slotScript.isSlotStop)
        {
            yield return null; // Wait for the next frame
        }

        // Once the slot is finished, spawn the collectable
        // We need to spawn the collectable at the updated position of the parent (obstacle set)
        SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.ObjectAppear);
        SpawnCollectable(parent, bottomEndY, topStartY);

    }

    public void SpawnCollectable(GameObject parent, float bottomEndY, float topStartY)
    {
        // Alt setin üst bitiş y pozisyonu ve üst setin alt başlangıç y pozisyonu arasında spawn
        //float randomY = Random.Range(bottomEndY, topStartY);
        float centerOfYBound = (bottomEndY + topStartY) / 2;

        // Pozisyon ayarla (Z ekseni sabit -2 olacak)
        Vector3 position = new Vector3(parent.transform.position.x, centerOfYBound, -2); // Güncel parent pozisyonuna göre konumlandır

        // Collectable nesnesi oluştur
        GameObject collectable = Instantiate(collectablePrefab, position, Quaternion.identity);
        collectable.transform.SetParent(parent.transform);
        collectable.GetComponent<Animation>().Play();

        if (selectedSlotItemIndex >= 0)  // Check if the index is valid
        {
            Sprite selectedSprite = GetSpriteForSelectedItem(selectedSlotItemIndex);
            if (selectedSprite != null)
            {
                SpriteRenderer renderer = collectable.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = selectedSprite;

                    // If the assigned sprite's name matches currentGoalObjectImage's sprite name, change the tag
                    if (selectedSprite.name == ChapterManager.Instance.currentGoalObjectImage.sprite.name)
                    {
                        collectable.tag = "GoalObject";
                    }
                }
            }
        }
    }
    private Sprite GetSpriteForSelectedItem(int index)
    {
        List<Sprite> spriteList = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex].goalObjectImagelist;        // Assuming you have an array or list of sprites
        if (index >= 0 && index < spriteList.Count)  // Check for valid index
        {
            return spriteList[index];
        }
        return null;
    }

    public void AssignSlotItem(int item)
    {
        selectedSlotItemIndex = item;
        Debug.Log("Selected Slot Item: " + selectedSlotItemIndex);
    }
}
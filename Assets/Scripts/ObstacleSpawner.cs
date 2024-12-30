using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject tileUpPrefab;
    public GameObject collectablePrefab;
    public GameObject slotPrefab;
    public GameObject tileAnimPrefab;

    [Header("Variables ")]
    public Transform topBound;            // Üst referans noktası
    public Transform bottomBound;         // Alt referans noktası
    public Transform spawnStartPoint;     // Spawn başlangıç noktası
    public GameObject level;              // Level GameObject
    public float moveSpeed = 2f;          // Hareket hızı
    public float destroyXPosition = -10f; // Yok olma pozisyonu
    public int minTiles = 2;              // Minimum obje sayısı
    public int maxTiles = 8;              // Maksimum obje sayısı
    public float spawnInterval = 3f;      // Spawn aralığı
    public int selectedSlotItemIndex; // Seçilen slot item'ı

    public bool isGameOn = false;

    [Header("Obstacle Game Objects")]
    public GameObject tileAnimGameObject;
    public List<GameObject> obstaclesSetGameObjectList;

    public static ObstacleSpawner Instance;

    public Material chapterSlotMaterial;

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
        else
        {
            level.SetActive(false);
        }
    }
    private void Update()
    {

    }

    public IEnumerator SpawnObstacleGroups()
    {
        while (isGameOn)
        {
            // Tile animasyonu oynat
            yield return PlayTileAnimation();

            // Eğer oyun devam etmiyorsa çık
            if (!isGameOn) yield break;

            // Animasyon tamamlandıktan sonra obstacle set oluştur
            SpawnObstacleSet();
            spawnStartPoint.position = new Vector3(8.71f, 0, -1);

            // Spawn aralığı kadar bekle
            yield return new WaitForSeconds(spawnInterval);

            // Eğer oyun devam etmiyorsa çık
            if (!isGameOn) yield break;
        }
    }
    private IEnumerator PlayTileAnimation()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.ObstacleAnim);
        // Tile objesini oluştur
        GameObject tileAnim = Instantiate(tileAnimPrefab, spawnStartPoint.position, Quaternion.identity);
        tileAnim.GetComponent<ObstacleMover>().Initialize(moveSpeed, destroyXPosition);
        tileAnimGameObject = tileAnim;

        // Animation bileşenini al
        Animation animation = tileAnim.GetComponent<Animation>();

        if (animation != null)
        {
            // Animasyonu başlat
            animation.Play();

            // Animasyon 3 saniye süreyle devam etsin
            yield return new WaitForSeconds(3f);
        }

        // Animasyon tamamlandıktan sonra objenin son pozisyonunu al
        if (tileAnim != null)
        {
            Vector3 lastPosition = tileAnim.transform.position;
            spawnStartPoint.position = lastPosition;
            Destroy(tileAnim);
        }
        // spawnStartPoint pozisyonunu son pozisyona eşitle
        // Animasyon tamamlandıktan sonra objeyi yok et
        SoundManager.Instance.StopSFX();
    }
    private void SpawnObstacleSet()
    {
        Debug.Log("Start Spawn Obstacle");
        // ObstacleSet GameObject oluştur
        GameObject obstacleSet = new GameObject("ObstacleSet");
        obstacleSet.transform.position = spawnStartPoint.position; // Sağdan başlat

        //obstacleSet.transform.SetParent(level.transform);
        obstaclesSetGameObjectList.Add(obstacleSet);

        // Toplam obstacle sayısını belirle
        int totalObstacles = Random.Range(minTiles * 2, maxTiles + 1);

        // Alt ve üst obstacle sayısını belirle
        int bottomTileCount = Random.Range(1, totalObstacles); // Alt için bir sayı seç
        int topTileCount = totalObstacles - bottomTileCount;   // Üst için kalan sayıyı al
        if (obstaclesSetGameObjectList != null)
        {
            // Alt seti oluştur ve üst sınırı al
            float bottomEndY = SpawnBottomSet(obstacleSet, bottomTileCount);

            // Üst seti oluştur ve alt sınırı al
            float topStartY = SpawnUpSet(obstacleSet, topTileCount);

            // Collectable nesne ekle (boşluk içinde)
            if (topStartY > bottomEndY) // Boşluk kontrolü
            {
                SpawnSlot(obstacleSet, bottomEndY, topStartY);
            }

            // Obstacle set hareket ettir
            obstacleSet.AddComponent<ObstacleMover>().Initialize(moveSpeed, destroyXPosition);
        }

    }

    private float SpawnBottomSet(GameObject parent, int tileCount)
    {
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

    private float SpawnUpSet(GameObject parent, int tileCount)
    {
        Vector3 position = new Vector3(spawnStartPoint.position.x, topBound.position.y, -2);

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

        // Slot prefab'ını instantiate et
        GameObject slot = Instantiate(slotPrefab, positionSlot, Quaternion.identity);

        // Slot'un mesh renderer bileşenine ulaş
        MeshRenderer meshRenderer = slot.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // Mevcut materyali al
            Material slotMaterial = meshRenderer.material;

            // ChapterManager üzerinden materyal sprite'ını al
            Sprite chapterSprite = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex].chapterItemsMaterialSprite;

            if (slotMaterial != null && chapterSprite != null)
            {
                // Sprite'ı bir Texture2D'ye dönüştür
                Texture2D texture = chapterSprite.texture;

                // Material'in texture'ını değiştir
                slotMaterial.mainTexture = texture;
            }
        }

        // Slot script'i al ve coroutine başlat
        Slot slotScript = slot.GetComponent<Slot>();
        if (slotScript != null)
        {
            slotScript.StartCoroutine(slotScript.StopAndDestroySlot());
        }

        // Slot'un rotasyonunu ayarla
        slot.transform.rotation = Quaternion.Euler(0, -90, 0);
        slot.transform.SetParent(parent.transform);

        // Slot tamamlandığında collectable spawn et
        StartCoroutine(WaitForSlotToFinishAndSpawnCollectable(slotScript, parent, bottomEndY, topStartY));

        // Spin ses efekti oynat
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

    public void LoseObstaceleSetting()
    {
        // Destroy Tile Anim if any
        if (tileAnimGameObject != null)
        {
            Destroy(tileAnimGameObject);
        }
        // Destroy ObdtacelSet if any
        if (obstaclesSetGameObjectList != null)
        {
            for (int i = 0; i < obstaclesSetGameObjectList.Count; i++)
            {
                Destroy(obstaclesSetGameObjectList[i]);
            }
        }
        isGameOn = false;
    }
    private void UpdateSlotMaterial(GameObject slot)
    {
        Renderer slotRenderer = slot.GetComponent<Renderer>();
        if (slotRenderer != null && chapterSlotMaterial != null)
        {
            // Yeni materyali ayarla
            slotRenderer.material = chapterSlotMaterial;

            // Materyalin sprite'ını güncelle
            if (ChapterManager.Instance != null &&
                ChapterManager.Instance.chapters != null &&
                ChapterManager.Instance.currentChapterIndex < ChapterManager.Instance.chapters.Count)
            {
                Sprite chapterSprite = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex].chapterItemsMaterialSprite;

                if (chapterSprite != null)
                {
                    slotRenderer.material.mainTexture = chapterSprite.texture;
                }
            }
        }
    }
}
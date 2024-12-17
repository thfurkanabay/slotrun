using UnityEngine;
using System.Collections.Generic;

public class CloudMovement : MonoBehaviour
{
    public List<GameObject> cloudPrefabs; // Farklı bulut prefabs'leri
    public int cloudCount = 5;            // Kaç adet bulut spawn edilecek
    public float spawnHeightMin = -3f;    // Bulutların spawn edileceği minimum Y konumu
    public float spawnHeightMax = 3f;     // Bulutların spawn edileceği maksimum Y konumu
    public float minSpeed = 1f;           // Minimum hız
    public float maxSpeed = 3f;           // Maksimum hız
    public float spawnRightX = 10f;       // Sağ tarafta spawn edilecek X konumu
    public float despawnLeftX = -10f;     // Sol tarafta despawn edilecek X konumu
    public Transform cloudsParent;        // Bulutların altına ekleneceği parent nesne

    private GameObject[] clouds;          // Bulutların referanslarını tutar
    private float[] cloudSpeeds;          // Her bir bulutun hızını saklar
    private bool isMoving = false;        // Bulutların hareket etme durumu
    public static CloudMovement Instance;
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
    void Start()
    {
        if (cloudsParent == null)
        {
            // Clouds adında bir GameObject oluştur ve bunu parent olarak ayarla.
            cloudsParent = new GameObject("Clouds").transform;
        }

        clouds = new GameObject[cloudCount];
        cloudSpeeds = new float[cloudCount];
    }

    void Update()
    {
        if (!isMoving) return; // Eğer hareket durumu aktif değilse, hiçbir şey yapma

        for (int i = 0; i < clouds.Length; i++)
        {
            if (clouds[i] != null)
            {
                // Bulutu sola hareket ettir
                clouds[i].transform.Translate(Vector3.left * cloudSpeeds[i] * Time.deltaTime);

                // Bulut sahneden çıktıysa yeniden spawn et
                if (clouds[i].transform.position.x < despawnLeftX)
                {
                    SpawnCloud(i, spawnRightX);
                }
            }
        }
    }

    public void StartCloudMovement()
    {
        // Bulutları spawn et ve hareket etmelerini başlat
        for (int i = 0; i < cloudCount; i++)
        {
            SpawnCloud(i, Random.Range(spawnRightX - 5f, spawnRightX));
        }

        isMoving = true; // Hareketi aktif et
    }

    public void StopCloudMovement()
    {
        // Bulutları durdur
        isMoving = false;
    }

    private void SpawnCloud(int index, float spawnX)
    {
        if (clouds[index] != null)
            Destroy(clouds[index]);

        // Rastgele bir bulut prefab seç
        GameObject selectedCloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];

        // Spawn pozisyonu belirle
        Vector3 spawnPosition = new Vector3(
            spawnX,
            Random.Range(spawnHeightMin, spawnHeightMax),
            -1 // Z ekseni sabit
        );

        // Bulutu oluştur ve parent olarak Clouds GameObject'ini ayarla
        clouds[index] = Instantiate(selectedCloudPrefab, spawnPosition, Quaternion.identity, cloudsParent);

        // Hızını rastgele belirle
        cloudSpeeds[index] = Random.Range(minSpeed, maxSpeed);
    }
}
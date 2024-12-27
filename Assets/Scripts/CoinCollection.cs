using System.Collections;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public GameObject coinPrefab; // Coin prefabı
    public RectTransform targetPoint; // Coin'in gideceği hedef nokta (örneğin, coin sayacı UI'si)
    public GameObject spawnParent; // Parent objesi
    public float animationDuration = 1f; // Animasyonun süresi
    public float spawnInterval = 0.5f; // Coinler arası bekleme süresi
    public int coinsCollected = 0; // Toplanan coin sayısı
    public static CoinCollection Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Belirli sayıda coin gönderir.
    /// </summary>
    /// <param name="worldCollisionPosition">Coinlerin spawn edileceği dünya pozisyonu.</param>
    /// <param name="coinCount">Gönderilecek coin sayısı.</param>
    public void CollectCoinsInSequence(Vector3 worldCollisionPosition, int coinCount)
    {
        // 1. Dünya pozisyonunu ekran pozisyonuna dönüştür
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldCollisionPosition);

        // 2. Ekran pozisyonunu UI (Canvas) pozisyonuna dönüştür
        RectTransform canvasRectTransform = spawnParent.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null, out uiPosition);

        // 3. Coinleri UI'deki başlangıç pozisyonundan başlat
        for (int i = 0; i < coinCount; i++)
        {
            // Coinlerin spawnlanması için gecikmeyi ayarla
            StartCoroutine(SpawnCoinWithDelay(uiPosition, i * spawnInterval)); // spawnInterval ile her coin için gecikme ekliyoruz
        }
    }

    private IEnumerator SpawnCoinWithDelay(Vector2 uiPosition, float delay)
    {
        // Coinin spawnlanma zamanı için gecikme
        yield return new WaitForSeconds(delay);

        // Yeni bir coin oluştur
        GameObject coin = Instantiate(coinPrefab, spawnParent.transform);

        // Coin'in başlangıç pozisyonunu UI pozisyonuna ayarla
        RectTransform coinRect = coin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = uiPosition;

        // Coin hedef UI'ye hareket etmeden önce
        StartCoroutine(MoveCoinToTarget(coin, coinRect)); // Hareketi başlat
    }

    private IEnumerator MoveCoinToTarget(GameObject coin, RectTransform coinRect)
    {
        Vector2 startPosition = coinRect.anchoredPosition;
        Vector2 endPosition = targetPoint.position;
        float elapsedTime = 0f;

        // 6. Animasyon sırasında coin'in hareketi
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // Coin'in pozisyonunu güncelle (Lerp ile yumuşak hareket)
            coinRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            // Coin'in boyutunu küçültmek isterseniz:
            coinRect.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

            yield return null;
        }

        // 7. Animasyon tamamlandığında coini yok et ve sayacı güncelle
        coinsCollected++;

        UserManager.Instance.IncreaseCoins(RandomReward());

        Destroy(coin);
        Debug.Log($"Coins Collected: {coinsCollected}");
    }
    public int RandomReward()
    {
        // Rastgele bir ödül verebiliriz. Örneğin, rastgele bir coin spawn edebiliriz
        int randomCoinCount = Random.Range(10, 50); // Rastgele coin sayısı (1-5 arasında)
        return randomCoinCount;
    }
}
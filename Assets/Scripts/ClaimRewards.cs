using System.Collections;
using UnityEngine;

public class ClaimRewards : MonoBehaviour
{
    /// <summary>
    /// IsAllRewardsCollected
    /// IsCoinRewardCollected 
    /// IsGemRewardCollected
    /// IsXPRewardCollected
    /// </summary>
    [Header("Reward Prefabs")]
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    public GameObject xpPrefab;

    [Header("UI Positions")]
    public Transform coinStartPos; // Coin başlangıç pozisyonu
    public Transform gemStartPos;  // Gem başlangıç pozisyonu
    public Transform xpStartPos;   // XP başlangıç pozisyonu
    public Transform coinEndPos;   // Coin hedef pozisyonu
    public Transform gemEndPos;    // Gem hedef pozisyonu
    public Transform xpEndPos;     // XP hedef pozisyonu

    [Header("Settings")]
    public float moveDuration = 1f; // Hareket süresi
    public float spawnInterval = 0.2f; // Ödüller arasındaki spawn gecikmesi
    public float groupDelay = 1f;   // Ödül grupları arasında bekleme süresi
    public int rewardCount = 5; // Her ödül için spawnlanacak miktar
    public Transform rewardContainer; // Ödüllerin spawn edileceği UI container

    public static ClaimRewards Instance;

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

    public void ClaimReward()
    {
        StartCoroutine(SpawnRewardsSequentially());
    }

    private IEnumerator SpawnRewardsSequentially()
    {
        // Coin grubunu spawnla ve bitmesini bekle
        yield return StartCoroutine(SpawnRewards(coinPrefab, coinStartPos.position, coinEndPos.position));
        yield return new WaitForSeconds(groupDelay);

        // Gem grubunu spawnla ve bitmesini bekle
        yield return StartCoroutine(SpawnRewards(gemPrefab, gemStartPos.position, gemEndPos.position));
        yield return new WaitForSeconds(groupDelay);

        // XP grubunu spawnla ve bitmesini bekle
        yield return StartCoroutine(SpawnRewards(xpPrefab, xpStartPos.position, xpEndPos.position));
    }

    private IEnumerator SpawnRewards(GameObject prefab, Vector3 startPos, Vector3 endPos)
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.RewardCollect);

        for (int i = 0; i < rewardCount; i++)
        {
            // Her ödül arasında belirli bir süre bekle
            yield return new WaitForSeconds(spawnInterval);

            // Yeni bir ödül oluştur
            GameObject reward = Instantiate(prefab, rewardContainer);
            RectTransform rewardTransform = reward.GetComponent<RectTransform>();
            rewardTransform.position = startPos;

            // Animasyonu başlat
            StartCoroutine(MoveAndShrink(reward, rewardTransform, endPos));
        }
    }

    private IEnumerator MoveAndShrink(GameObject reward, RectTransform rewardTransform, Vector3 endPos)
    {
        Vector2 startPosition = rewardTransform.position;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.zero;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // Pozisyon ve ölçek interpolasyonu
            rewardTransform.position = Vector2.Lerp(startPosition, endPos, t);
            rewardTransform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        // Hedefe ulaştığında ödülü yok et
        Destroy(reward);
    }
}
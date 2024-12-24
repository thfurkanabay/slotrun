using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float rotationSpeed = 300f; // Başlangıç hızı
    public int steps = 10; // Toplam kaç adımda dönmesi gerektiği
    private float stepAngle; // Her adımda dönecek açı
    private int currentStep = 0; // Hangi adımda olduğunu takip etmek için
    public Dictionary<(float, float), string> angleRanges;
    public float StopZAngle;
    public bool isSlotStop;
    public static Slot Instance;
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
        isSlotStop = false;
        angleRanges = new Dictionary<(float, float), string>
        {
            {(0, 36), "item1"},
            {(36, 72), "item2"},
            {(72, 108), "item3"},
            {(108, 144), "item4"},
            {(144, 180), "item5"},
            {(180, 216), "item6"},
            {(216, 252), "item7"},
            {(252, 288), "item8"},
            {(288, 314), "item9"},
            {(314, 360), "item10"}
        };
        // Random bir açıyla spawn olma
        float randomZAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 90f, randomZAngle); // Z eksenine rastgele bir açı ver
        // Her adımda dönecek açıyı hesapla
        stepAngle = 360f / steps;
    }

    void Update()
    {
        if (currentStep < steps)
        {
            // Z ekseninde döndürme işlemi
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            // Her adımda bir tam dönüş yaptıysa, bir sonraki adıma geç
            if (Mathf.Abs(transform.eulerAngles.z) >= stepAngle * (currentStep + 1))
            {
                currentStep++;
            }
        }
    }

    public IEnumerator StopAndDestroySlot()
    {
        float duration = 3f; // Yavaşlama süresi
        float elapsedTime = 0f; // Geçen süreyi takip et
        float initialSpeed = rotationSpeed; // Başlangıç hızını kaydet

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Dönüş hızını üstel olarak azalt
            float t = elapsedTime / duration;
            rotationSpeed = Mathf.Lerp(initialSpeed, 0f, t * t); // t^2 daha yumuşak bir yavaşlama sağlar

            yield return null; // Bir sonraki kareyi bekle
        }
        rotationSpeed = 0; // Tamamen durduğundan emin ol
        Debug.Log("Final rotation angle: " + transform.eulerAngles.z);
        StopZAngle = transform.eulerAngles.z;
        StopItem(StopZAngle);

        foreach (var range in angleRanges)
        {
            if (StopZAngle >= range.Key.Item1 && StopZAngle < range.Key.Item2)
            {
                int itemIndex = new List<(float, float)>(angleRanges.Keys).IndexOf(range.Key);
                ObstacleSpawner.Instance.AssignSlotItem(itemIndex);
                Debug.Log(range.Value);
                break;
            }
        }
        // stop the sfx
        SoundManager.Instance.StopSFX();

        Destroy(gameObject); // `this` yerine `gameObject` kullan
        isSlotStop = true;

    }

    public void StopItem(float itemTransform)
    {
        // 360 dereceyi 10 dilime böldük
        float segment = 360f / 10f;

        // itemTransform değerini dilimlere göre kontrol et
        for (int i = 0; i < 10; i++)
        {
            // Her dilim için kontrol
            if (itemTransform >= i * segment && itemTransform < (i + 1) * segment)
            {
                // Konsola hangi item olduğunu yazdır
                Debug.Log($"Item is in segment {i + 1} ({i * segment}° - {(i + 1) * segment}°)");
                break;
            }
        }
    }
}
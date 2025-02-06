using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float rotationSpeed = 300f; // Başlangıç dönüş hızı
    public float spinDuration = 1f; // Sabit hızda dönme süresi
    public float slowDownDuration = 1f; // Yavaşlama süresi
    public bool isSlotStopped = false; // Slot durdu mu?
    public int currentChapterIndex; // Mevcut chapter
    private int selectedItemIndex; // Seçilen nesnenin indeksi

    [System.Serializable]
    public class SlotItem
    {
        public int itemIndex;
        public float probability;
        public float angle; // Hedef açı
    }

    public List<SlotItem> slotItems = new List<SlotItem>();

    private void Start()
    {
        currentChapterIndex = ChapterManager.Instance.currentChapterIndex;
        transform.eulerAngles = new Vector3(180f, -90f, 0f);
    }

    public void ChooseSelectedItem()
    {
        float totalProbability = 0f;
        foreach (SlotItem item in slotItems)
        {
            totalProbability += item.probability;
        }

        float randomPoint = Random.value * totalProbability;
        float cumulativeProbability = 0f;

        foreach (SlotItem item in slotItems)
        {
            cumulativeProbability += item.probability;
            if (randomPoint <= cumulativeProbability)
            {
                selectedItemIndex = item.itemIndex;
                break;
            }
        }

        Debug.Log($"Seçilen item indexi: {selectedItemIndex}, Hedef açı: {slotItems[selectedItemIndex].angle}");
    }

    public IEnumerator SpinAndSlowDown()
    {
        // Hedef açı
        float targetAngle = slotItems[selectedItemIndex].angle;

        // Mevcut açı
        float currentZRotation = transform.eulerAngles.z;

        // **1. Aşama: Sabit Hızda Dönüş**
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;

            // Sabit hızla dönüş
            currentZRotation += rotationSpeed * Time.deltaTime;

            // Açı normalize ediliyor
            currentZRotation %= 360f;

            // Yeni açıyı uygula
            transform.eulerAngles = new Vector3(180f, -90f, currentZRotation);

            yield return null;
        }

        // **2. Aşama: Yavaşlama ve Hedef Açıya Ulaşma**
        float initialSpeed = rotationSpeed; // Başlangıç dönüş hızı
        elapsedTime = 0f;

        while (true)
        {
            // Hedef açıya olan farkı hesapla
            float angleDifference = (targetAngle - currentZRotation + 360f) % 360f;

            // Eğer açı farkı çok küçükse döngüyü kır
            if (angleDifference < 0.1f)
            {
                currentZRotation = targetAngle;
                break;
            }

            // Hız azaltılıyor
            rotationSpeed = Mathf.SmoothStep(initialSpeed, 0f, elapsedTime / slowDownDuration);
            elapsedTime += Time.deltaTime;

            // Dönüş adımı
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Eğer adım, kalan açı farkından büyükse, direkt hedef açıda dur
            if (rotationStep >= angleDifference)
            {
                currentZRotation = targetAngle;
                break;
            }
            else
            {
                // Normal dönüşe devam
                currentZRotation += rotationStep;
            }

            // Açı normalize ediliyor
            currentZRotation %= 360f;

            // Yeni açıyı uygula
            transform.eulerAngles = new Vector3(180f, -90f, currentZRotation);

            yield return null;
        }

        // Hedef açıyı kesin olarak ayarla
        transform.eulerAngles = new Vector3(180f, -90f, targetAngle);

        Debug.Log($"Slot durdu. Hedef açıda: {transform.eulerAngles.z}");

        // Slotu durdur
        isSlotStopped = true;

        // Seçilen itemi ChapterManager'a bildir
        NotifyChapterManager();

        // Biraz bekle ve slotu yok et
        yield return new WaitForSeconds(0.5f); // Yavaşlama bittikten sonra 0.5 saniye bekle

        SoundManager.Instance.StopSFX();

        Destroy(gameObject);

    }

    private void NotifyChapterManager()
    {
        if (ChapterManager.Instance != null)
        {
            Sprite selectedSprite = ChapterManager.Instance.chapters[currentChapterIndex].goalObjectImagelist[selectedItemIndex];
            ChapterManager.Instance.chapters[currentChapterIndex].selectedItemIndex = selectedItemIndex;
            Debug.Log($"Seçilen item: {selectedSprite.name}");
        }
        else
        {
            Debug.LogError("ChapterManager bulunamadı!");
        }
    }
}
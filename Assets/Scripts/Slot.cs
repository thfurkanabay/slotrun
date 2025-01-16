using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float rotationSpeed = 300f; // Başlangıç dönüş hızı
    public float spinDuration = 1f; // Sabit hızda dönme süresi
    public float slowDownDuration = 2f; // Yavaşlama süresi
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
        //ChooseSelectedItem();

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
        Debug.Log("Slowed selectedItemIndex: " + selectedItemIndex);
        float targetAngle = slotItems[selectedItemIndex].angle;

        // Sabit hızda dönme
        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Yavaşlama süreci
        float initialSpeed = rotationSpeed;
        elapsedTime = 1f;
        float currentZRotation = transform.eulerAngles.z;

        while (elapsedTime < slowDownDuration)
        {
            elapsedTime += Time.deltaTime;

            // Hız azaltılıyor
            rotationSpeed = Mathf.Lerp(initialSpeed, 0f, elapsedTime / slowDownDuration);

            // Mevcut açıdan 360'a doğru ilerle ve sonra hedef açıya geç
            float angleDifference = (targetAngle - currentZRotation + 360f) % 360f; // Saat yönünde hesapla
            if (angleDifference < 0) angleDifference += 360f; // Negatif olmasın

            // Eğer mevcut açı hedef açıdan daha küçükse, önce 360'a tamamla
            if (currentZRotation < targetAngle)
            {
                // 360'a doğru dönecek adımı hesapla
                float stepTo360 = Mathf.Min((360f - currentZRotation), rotationSpeed * Time.deltaTime);
                currentZRotation += stepTo360;

                // Eğer 360'a ulaştıysa, artık hedef açıya yönel
                if (currentZRotation >= 360f)
                {
                    currentZRotation = 0f;
                }
            }

            // Sonra hedef açıya doğru ilerle
            float stepToTarget = Mathf.Min(angleDifference, rotationSpeed * Time.deltaTime);
            currentZRotation += stepToTarget;

            // Yeni açıyı uygula
            transform.eulerAngles = new Vector3(180f, -90f, currentZRotation % 360f); // Açı normalize ediliyor

            Debug.Log($"Mevcut açı: {transform.eulerAngles.z}, Hedef açı: {targetAngle}, Hız: {rotationSpeed}");
            yield return null;
        }

        // Açıyı kesin olarak hedefe ayarla
        transform.eulerAngles = new Vector3(180f, -90f, targetAngle);

        Debug.Log($"Slot durdu. Hedef açıda: {transform.eulerAngles.z}");

        // Slotu durdur
        isSlotStopped = true;

        // Seçilen itemi ChapterManager'a bildir
        NotifyChapterManager();

        // Destroy işlemi
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
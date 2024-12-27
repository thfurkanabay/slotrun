using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryFee : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI entryFeeText; // Giriş ücretini göstermek için kullanılan UI Text
    [Header("UI Buttons")]

    [Header("Fee Settings")]
    public int levelFeeMultiplier = 100; // Çarpan değeri
    public int minFee = 0;               // Minimum giriş ücreti
    public int maxFee = 10000;           // Maksimum giriş ücreti

    private int currentFee = 0; // Şu anki giriş ücreti


    void Start()
    {
        LoadLevelFee();   // Başlangıç giriş ücretini çek
        UpdateFeeText();  // UI'yı güncelle
    }

    private void LoadLevelFee()
    {
        // `ChapterManager`'dan giriş ücretini çek
        if (ChapterManager.Instance != null)
        {
            currentFee = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex]
                .levels[ChapterManager.Instance.currentLevelIndex].entryFee;
            currentFee = Mathf.Clamp(currentFee, minFee, maxFee); // Ücreti sınırla
        }
        else
        {
            Debug.LogError("ChapterManager atanmadı!");
        }
    }

    // + Butonuna tıklandığında çağrılır
    public void IncreaseFee()
    {
        if (currentFee + levelFeeMultiplier <= maxFee)
        {
            currentFee += levelFeeMultiplier;
            UpdateFeeText();
        }
    }

    // - Butonuna tıklandığında çağrılır
    public void DecreaseFee()
    {
        if (currentFee - levelFeeMultiplier >= minFee)
        {
            currentFee -= levelFeeMultiplier;
            UpdateFeeText();
        }
    }

    // UI'yı güncellemek için kullanılan metot
    private void UpdateFeeText()
    {
        entryFeeText.text = currentFee.ToString();
    }
    public void ResetEntryFee()
    {
        entryFeeText.text = currentFee.ToString();

    }
}
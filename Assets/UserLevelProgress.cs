using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLevelProgress : MonoBehaviour
{
    [Header("Level Settings")]
    public int userLevel = 1; // Kullanıcı başlangıç seviyesi
    public float currentXP = 0f; // Şu anki XP
    public float requiredXP = 10f; // Başlangıçta gerekli XP
    public float baseXP = 10f; // İlk seviye için baz XP
    public float xpMultiplier = 1.5f; // XP artış çarpanı

    [Header("UI Elements")]
    public TextMeshProUGUI userLevelText; // Kullanıcı seviyesi
    public Slider userLevelSlider; // Seviye ilerleme çubuğu

    void Start()
    {
        UpdateLevelUI(); // Başlangıçta UI'yi güncelle
    }

    void Update()
    {
        // Test amaçlı XP eklemek için
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddXP(15); // 15 XP ekler
        }
    }

    /// <summary>
    /// XP ekleme metodu
    /// </summary>
    /// <param name="xp">Eklenecek XP miktarı</param>
    public void AddXP(float xp)
    {
        currentXP += xp;

        // Eğer mevcut XP gerekli XP'yi aşarsa
        if (currentXP >= requiredXP)
        {
            LevelUp(); // Seviye atla
        }

        UpdateLevelUI(); // UI'yi güncelle
    }

    /// <summary>
    /// Seviye atlama işlemi
    /// </summary>
    private void LevelUp()
    {
        currentXP -= requiredXP; // Fazla XP'yi taşı
        userLevel++; // Seviyeyi artır
        requiredXP = baseXP * Mathf.Pow(userLevel, xpMultiplier); // Yeni gerekli XP'yi hesapla

        Debug.Log($"Seviye Atlama: Yeni Seviye {userLevel}, Gerekli XP: {requiredXP}");
    }

    /// <summary>
    /// UI güncelleme işlemi
    /// </summary>
    private void UpdateLevelUI()
    {
        userLevelText.text = $"{userLevel}";
        userLevelSlider.maxValue = requiredXP;
        userLevelSlider.value = currentXP;
    }
}
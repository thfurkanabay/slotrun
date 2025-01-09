using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI userLevelText; // Kullanıcı seviyesi
    public TextMeshProUGUI currentXPText; // Kullanıcı seviyesi
    public TextMeshProUGUI requiredXPText; // Kullanıcı seviyesi
    public TextMeshProUGUI playerNameText;
    public Image userIcon; // Kullanıcı ikonu   
    public Image userBadge; // Kullanıcı ikonu   

    public List<Sprite> userIcons; // Kullanılabilir kullanıcı ikonları
    public List<Sprite> userBadges; // Kullanılabilir kullanıcı ikonları

    public PlayerDataManager playerDataManager;
    public int userLevel = 1; // Kullanıcı başlangıç seviyesi
    public float currentXP = 0f; // Şu anki XP
    public float requiredXP = 10f; // Başlangıçta gerekli XP
    public float baseXP = 10f; // İlk seviye için baz XP
    public float xpMultiplier = 1.5f; // XP artış çarpanı

    [Header("UI Elements")]
    public Slider userLevelSlider; // Seviye ilerleme çubuğu


    public static UserManager Instance;

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

    void Update()
    {
        // Test amaçlı XP eklemek için
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseCoins(100); // 15 XP ekler
            IncreaseGems(10);
            IncreaseXP(10);
        }

    }

    public void InitializeUser()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        // Mevcut değerleri UI'ya yansıt
        UpdateUI();
    }

    public void UpdateUI()
    {
        coinsText.text = playerDataManager.playerCoins.ToString();
        gemsText.text = playerDataManager.playerGems.ToString();
        userLevelText.text = playerDataManager.userLevel.ToString();
        currentXPText.text = playerDataManager.currentXP.ToString();
        requiredXPText.text = playerDataManager.requiredXP.ToString();

        if (playerDataManager.userIcon != null)
        {
            userIcon.sprite = playerDataManager.userIcon.sprite;
        }
        else
        {
            Debug.LogWarning("User icon is not set in PlayerDataManager.");
        }
        if (playerDataManager.userBadges != null)
        {
            userBadge.sprite = playerDataManager.userBadges.sprite;
        }
        else
        {
            Debug.LogWarning("User badge is not set in PlayerDataManager.");
        }

        playerNameText.text = playerDataManager.playerName;
        userLevelSlider.maxValue = playerDataManager.requiredXP;
        userLevelSlider.value = playerDataManager.currentXP;
        Debug.Log("maxValue:" + userLevelSlider.maxValue);
        Debug.Log("value:" + userLevelSlider.value);

        Debug.Log("playerNameText:" + playerNameText);

        PlayerDataManager.Instance.SavePlayerData();
        // Dropdown'u güncelle
        //completedLevelDropdown.value = playerDataManager.completedLevel;
    }


    public void IncreaseCoins(int amount)
    {
        playerDataManager.playerCoins += amount; // Parayı artır
        playerDataManager.SavePlayerData();   // Kaydet
        UpdateUI();                           // UI'yı güncelle
    }

    public void DecreaseCoins(int amount)
    {
        if (playerDataManager.playerCoins > 0)
        {
            Debug.Log("Descrease amount : " + amount);
            playerDataManager.playerCoins -= amount; // Parayı azalt
            playerDataManager.SavePlayerData();   // Kaydet
            UpdateUI();                           // UI'yı güncelle
        }
    }
    public void IncreaseGems(int amount)
    {
        playerDataManager.playerGems += amount; // Parayı artır
        playerDataManager.SavePlayerData();   // Kaydet
        UpdateUI();                           // UI'yı güncelle
    }
    public void IncreaseXP(int amount)
    {
        playerDataManager.currentXP += amount; // XP artır
        playerDataManager.requiredXP = CalculateRequiredXP(playerDataManager.userLevel);
        Debug.Log("requiredXP: " + requiredXP);

        if (playerDataManager.currentXP >= playerDataManager.requiredXP)
        {
            LevelUp();
        }
        playerDataManager.SavePlayerData();   // Kaydet
        UpdateUI();                           // UI'yı güncelle
    }
    public void LevelUp()
    {
        playerDataManager.userLevel++; // Seviyeyi artır
        playerDataManager.currentXP = 0; // Mevcut XP'yi sıfırla
        playerDataManager.requiredXP = CalculateRequiredXP(playerDataManager.userLevel); // Yeni seviyeye göre gerekli XP'yi hesapla

        playerDataManager.SavePlayerData(); // Kaydet
        UpdateUI(); // UI'yı güncelle
    }
    private float CalculateRequiredXP(int level)
    {
        playerDataManager.baseXP = 10f;
        playerDataManager.xpMultiplier = 1.5f;

        Debug.Log("playerDataManager.baseXP: " + playerDataManager.baseXP);
        Debug.Log("playerDataManager.xpMultiplier: " + playerDataManager.xpMultiplier);
        Debug.Log("level: " + level);

        return playerDataManager.baseXP * Mathf.Pow(level, playerDataManager.xpMultiplier); // Yeni seviye için XP hesaplama
    }

    public void DecreaseGems(int amount)
    {
        if (playerDataManager.playerGems > 0)
        {
            playerDataManager.playerGems -= amount; // Parayı azalt
            playerDataManager.SavePlayerData();   // Kaydet
            UpdateUI();                           // UI'yı güncelle
        }
    }

    public void UpdatePlayerName()
    {
        playerDataManager.playerName = playerNameText.text; // İsmi güncelle
        playerDataManager.SavePlayerData();                  // Kaydet
    }

    public void UpdateCompletedLevel()
    {
        //playerDataManager.completedLevel = completedLevelDropdown.value; // Level'i güncelle
        playerDataManager.SavePlayerData();                              // Kaydet
    }

    /// <summary>
    /// Test Purpose
    /// </summary>
    /*public void ResetButton()
    {
        Debug.Log("All Player datas reset");
        playerDataManager.ResetPlayerData();
        UpdateUI();
    }*/

}

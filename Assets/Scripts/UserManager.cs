using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    [Tooltip("Coin text objects to update")]

    [Header("Coin Element")]
    public List<TextMeshProUGUI> coinsTexts;

    [Header("Gem Elements")]
    public TextMeshProUGUI gemsText;

    [Header("User Level Element")]
    public List<TextMeshProUGUI> userLevelTexts;

    [Header("XP Elements")]
    public List<TextMeshProUGUI> currentXPTexts; // Kullanıcı seviyesi
    public List<TextMeshProUGUI> requiredXPTexts; // Kullanıcı seviyesi

    [Header("User Name Element")]
    public TextMeshProUGUI playerNameText;

    public List<Image> userIconList; // Kullanıcı ikonu   
    public List<Image> userBadgeList; // Kullanıcı ikonu   

    public List<Sprite> userIconImageList; // Kullanılabilir kullanıcı ikonları
    public List<Sprite> userBadgeImageList; // Kullanılabilir kullanıcı ikonları

    public PlayerDataManager playerDataManager;
    public int userLevel = 1; // Kullanıcı başlangıç seviyesi
    public float currentXP = 0f; // Şu anki XP
    public float requiredXP = 10f; // Başlangıçta gerekli XP
    public float baseXP = 10f; // İlk seviye için baz XP
    public float xpMultiplier = 1.5f; // XP artış çarpanı

    [Header("UI Elements")]
    public List<Slider> userLevelSlider; // Seviye ilerleme çubuğu


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
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseCoins(100); // 15 XP ekler
            IncreaseGems(10);
            IncreaseXP(10);
        }*/

    }

    public void InitializeUser()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        IncreaseCoins(1000);
        // Mevcut değerleri UI'ya yansıt
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateCoinText(); // Update coin text
        UpdateUserLevelText(); // Update user level text
        UpdateXPTexts(); // Update XP text
        AssignUserBadge(); // Assign user badge
        UpdateUserBadges(); // Update user badges
        UpdateUserIcon(); // Update user icon

        gemsText.text = playerDataManager.playerGems.ToString();


        playerNameText.text = playerDataManager.playerName;

        UpdateLevelSlider();

        //userLevelSlider.maxValue = playerDataManager.requiredXP;
        //userLevelSlider.value = playerDataManager.currentXP;

        PlayerDataManager.Instance.SavePlayerData();

    }
    private void UpdateLevelSlider()
    {
        for (int i = 0; i < userLevelSlider.Count; i++)
        {
            userLevelSlider[i].maxValue = playerDataManager.requiredXP;
            userLevelSlider[i].value = playerDataManager.currentXP;
        }
    }

    private void UpdateCoinText()
    {
        for (int i = 0; i < coinsTexts.Count; i++)
        {
            coinsTexts[i].text = playerDataManager.playerCoins.ToString();
        }
    }
    private void UpdateUserLevelText()
    {
        for (int i = 0; i < userLevelTexts.Count; i++)
        {
            userLevelTexts[i].text = playerDataManager.userLevel.ToString();
        }

        for (int i = 0; i < currentXPTexts.Count; i++)
        {
            currentXPTexts[i].text = playerDataManager.currentXP.ToString();
        }
        for (int i = 0; i < requiredXPTexts.Count; i++)
        {
            requiredXPTexts[i].text = playerDataManager.requiredXP.ToString();
        }
    }
    private void UpdateXPTexts()
    {
        for (int i = 0; i < currentXPTexts.Count; i++)
        {
            currentXPTexts[i].text = playerDataManager.currentXP.ToString();
        }
        for (int i = 0; i < requiredXPTexts.Count; i++)
        {
            requiredXPTexts[i].text = playerDataManager.requiredXP.ToString();
        }
    }
    private void UpdateUserBadges()
    {
        for (int i = 0; i < userBadgeList.Count; i++)
        {
            userBadgeList[i].sprite = userBadgeImageList[playerDataManager.userBadgeImageIndex];
        }
    }
    private void UpdateUserIcon()
    {
        for (int i = 0; i < userIconList.Count; i++)
        {
            userIconList[i].sprite = userIconImageList[playerDataManager.userIconImageIndex];
        }
    }
    private void UpdateUserIcon(int index)
    {
        for (int i = 0; i < userIconList.Count; i++)
        {
            userIconList[i].sprite = userIconImageList[index];
        }
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
    public void ResetButton()
    {
        Debug.Log("All Player datas reset");
        playerDataManager.ResetPlayerData();
        UpdateUI();
    }
    public void AssignUserBadge()
    {
        // Kontrol: `userBadgeImageList` boşsa hata önle
        if (userBadgeImageList == null || userBadgeImageList.Count == 0)
        {
            Debug.LogWarning("Rozet listesi atanmadı!");
            return;
        }
        Debug.Log("UserLevel: " + playerDataManager.userLevel);

        // En yüksek seviyeden başlayarak kontrol et
        if (playerDataManager.userLevel > 100)
        {
            playerDataManager.userBadgeImageIndex = 6;
        }
        else if (playerDataManager.userLevel > 80)
        {
            playerDataManager.userBadgeImageIndex = 5;
        }
        else if (playerDataManager.userLevel > 60)
        {
            playerDataManager.userBadgeImageIndex = 4;
        }
        else if (playerDataManager.userLevel > 40)
        {
            playerDataManager.userBadgeImageIndex = 3;
        }
        else if (playerDataManager.userLevel > 30)
        {
            playerDataManager.userBadgeImageIndex = 2;
        }
        else if (playerDataManager.userLevel > 20)
        {
            playerDataManager.userBadgeImageIndex = 1;
        }
        else if (playerDataManager.userLevel > 10)
        {
            playerDataManager.userBadgeImageIndex = 0;
        }
        else
        {
            playerDataManager.userBadgeImageIndex = 0;
        }

        // Rozeti güncelle UI'da
        UpdateUserBadges();

        // Verileri kaydet
        playerDataManager.SavePlayerData();
    }

}

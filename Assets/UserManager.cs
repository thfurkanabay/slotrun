using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;

    public TextMeshProUGUI playerNameText;
    //public TMP_Dropdown completedLevelDropdown;

    public PlayerDataManager playerDataManager;

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

        Debug.Log("playerNameText:" + playerNameText);
        playerNameText.text = playerDataManager.playerName;
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
            playerDataManager.playerCoins -= amount; // Parayı azalt
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
}

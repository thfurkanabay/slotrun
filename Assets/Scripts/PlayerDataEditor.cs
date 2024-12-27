using TMPro;
using UnityEngine;

public class PlayerDataEditor : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI playerNameText;
    public TMP_Dropdown completedLevelDropdown;

    public PlayerDataManager playerDataManager;

    void Start()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();

        // Mevcut değerleri UI'ya yansıt
        UpdateUI();
    }

    public void UpdateUI()
    {
        coinsText.text = playerDataManager.playerCoins.ToString();
        playerNameText.text = playerDataManager.playerName;

        // Dropdown'u güncelle
        completedLevelDropdown.value = playerDataManager.completedLevel;
    }

    public void IncreaseCoins()
    {
        playerDataManager.playerCoins += 100; // Parayı artır
        playerDataManager.SavePlayerData();   // Kaydet
        UpdateUI();                           // UI'yı güncelle
    }

    public void DecreaseCoins()
    {
        if (playerDataManager.playerCoins > 0)
        {
            playerDataManager.playerCoins -= 100; // Parayı azalt
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
        playerDataManager.completedLevel = completedLevelDropdown.value; // Level'i güncelle
        playerDataManager.SavePlayerData();                              // Kaydet
    }
}
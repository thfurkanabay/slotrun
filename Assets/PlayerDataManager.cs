using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [Header("Player Data")]
    public string playerName;
    public int playerCoins;
    public int playerGems;

    public int completedLevel;

    public UserManager userManager;

    private string[] randomNames =
    {
        "ShadowHunter",
        "StarKnight",
        "BlazeWizard",
        "IronFury",
        "LoneWolf",
        "MysticShadow",
        "CrystalBlaze",
        "PhantomRider",
        "ThunderClaw",
        "SilverArrow"
    };

    void Start()
    {
        GenerateRandomPlayerName(); // Eğer bir isim yoksa rastgele oluştur
        userManager.InitializeUser();
        SavePlayerData();           // İlk veri kaydını yap

    }

    public void GenerateRandomPlayerName()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = randomNames[Random.Range(0, randomNames.Length)];
        }
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        PlayerPrefs.SetInt("PlayerGems", playerGems);
        PlayerPrefs.SetInt("CompletedLevel", completedLevel);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        playerGems = PlayerPrefs.GetInt("PlayerGems", 0);
        completedLevel = PlayerPrefs.GetInt("CompletedLevel", 0);
    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll();
        playerName = null; // İsmi sıfırla
        GenerateRandomPlayerName(); // Yeni rastgele isim oluştur
    }
}
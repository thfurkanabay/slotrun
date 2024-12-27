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
    public static PlayerDataManager Instance;


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
    private void Awake()
    {
        if (Instance != null && Instance != gameObject)
        {
            Destroy(this);
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
        LoadPlayerData(); // Önce kaydedilmiş verileri yükle
        GenerateRandomPlayerName(); // Eğer isim yoksa rastgele oluştur
        userManager.InitializeUser(); // UI'yı güncelle
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
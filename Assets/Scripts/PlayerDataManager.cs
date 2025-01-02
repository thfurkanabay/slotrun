using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [Header("Player Data")]
    public string playerName;
    public int playerCoins;
    public int playerGems;

    public int userLevel; // Kullanıcı seviyesi
    public float currentXP; // Şu anki XP
    public float requiredXP; // Şu anki XP
    public float baseXP = 10; // Şu anki XP
    public float xpMultiplier = 1.5f; // XP artış çarpanı
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
        PlayerPrefs.SetInt("UserLevel", userLevel);
        PlayerPrefs.SetFloat("CurrentXP", currentXP);
        PlayerPrefs.SetFloat("RequiredXP", requiredXP);
        PlayerPrefs.SetFloat("BaseXP", baseXP);
        PlayerPrefs.SetFloat("XpMultiplier", xpMultiplier);

        PlayerPrefs.SetInt("CompletedLevel", completedLevel);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        playerGems = PlayerPrefs.GetInt("PlayerGems", 0);
        userLevel = PlayerPrefs.GetInt("UserLevel", 1);
        currentXP = PlayerPrefs.GetFloat("CurrentXP", 0f);
        requiredXP = PlayerPrefs.GetFloat("RequiredXP", 0f);
        baseXP = PlayerPrefs.GetFloat("BaseXP", 0f);
        xpMultiplier = PlayerPrefs.GetFloat("XpMultiplier", 0f);

        completedLevel = PlayerPrefs.GetInt("CompletedLevel", 0);
    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll();
        playerName = null; // İsmi sıfırla
        userLevel = 1; // Seviyeyi sıfırla
        currentXP = 0f; // XP'yi sıfırla
        GenerateRandomPlayerName(); // Yeni rastgele isim oluştur
    }

}
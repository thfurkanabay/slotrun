using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour
{
    [Header("Player Data")]
    public string playerName;
    public int playerCoins;
    public int playerGems;
    public Image userIcon; // Kullanıcı seviyesi
    public Image userBadges; // Kullanıcı seviyesi
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


        int userIconIndex = userManager.userIcons.IndexOf(userIcon.sprite);
        PlayerPrefs.SetInt("UserIcon", userIconIndex);

        int userBadgesIndex = userManager.userBadges.IndexOf(userBadges.sprite);
        PlayerPrefs.SetInt("UserBadges", userBadgesIndex);

        PlayerPrefs.SetInt("CompletedLevel", completedLevel);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {

        // Kullanıcı ikonunu yükle
        int userIconIndex = PlayerPrefs.GetInt("UserIcon", 0);
        if (userIconIndex >= 0 && userIconIndex < userManager.userIcons.Count)
        {
            userIcon.sprite = userManager.userIcons[userIconIndex];
        }
        else
        {
            userIcon.sprite = userManager.userIcons[0];
        }

        // Kullanıcı rozetini yükle

        int userBadgesIndex = PlayerPrefs.GetInt("UserBadges", 0);
        if (userBadgesIndex >= 0 && userBadgesIndex < userManager.userBadges.Count)
        {
            userBadges.sprite = userManager.userBadges[userBadgesIndex];
        }
        else
        {
            userBadges.sprite = userManager.userBadges[0];
        }

        playerName = PlayerPrefs.GetString("PlayerName", "Guest");
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        playerGems = PlayerPrefs.GetInt("PlayerGems", 0);
        userLevel = PlayerPrefs.GetInt("UserLevel", 1);
        currentXP = PlayerPrefs.GetFloat("CurrentXP", 0f);
        requiredXP = PlayerPrefs.GetFloat("RequiredXP", 0f);
        baseXP = PlayerPrefs.GetFloat("BaseXP", 0f);
        xpMultiplier = PlayerPrefs.GetFloat("XpMultiplier", 0f);

        completedLevel = PlayerPrefs.GetInt("CompletedLevel", 0);

        AssignUserBadge();

    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll();
        playerName = null; // İsmi sıfırla
        userLevel = 1; // Seviyeyi sıfırla
        currentXP = 0f; // XP'yi sıfırla
        GenerateRandomPlayerName(); // Yeni rastgele isim oluştur
    }
    public void AssignUserBadge()
    {
        // Kontrol: `userManager` veya `userBadges` null ise hata önle
        if (userManager == null || userManager.userBadges == null || userManager.userBadges.Count == 0)
        {
            Debug.LogWarning("UserManager veya rozet listesi atanmadı!");
            return;
        }
        Debug.Log("UserLevel: " + userLevel);
        // En yüksek seviyeden başlayarak kontrol et
        if (userLevel > 100)
        {
            userBadges.sprite = userManager.userBadges[6]; // En yüksek rozet
        }
        else if (userLevel > 80)
        {
            userBadges.sprite = userManager.userBadges[5];
        }
        else if (userLevel > 60)
        {
            userBadges.sprite = userManager.userBadges[4];
        }
        else if (userLevel > 40)
        {
            userBadges.sprite = userManager.userBadges[3];
        }
        else if (userLevel > 30)
        {
            userBadges.sprite = userManager.userBadges[2];
        }
        else if (userLevel > 20)
        {
            userBadges.sprite = userManager.userBadges[1];
        }
        else if (userLevel > 10)
        {
            userBadges.sprite = userManager.userBadges[0];
        }
        else
        {
            userBadges.sprite = userManager.userBadges[0];
        }
    }

}
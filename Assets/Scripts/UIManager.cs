using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton Instance
    public static UIManager Instance { get; private set; }

    [System.Serializable]
    public class Panel
    {
        public string name;      // Panel adı
        public GameObject panel; // Panel GameObject
    }

    [SerializeField] private List<Panel> screens; // Tüm ana ekranların listesi
    [SerializeField] private List<Panel> popups;  // Tüm popup pencerelerin listesi

    private GameObject currentScreen; // Şu anda aktif olan ana ekran
    private Stack<GameObject> activePopups = new Stack<GameObject>(); // Aktif popup'ları takip eder

    private void Awake()
    {
        // Singleton Instance ayarı
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Daha önce bir instance varsa yok et
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne değişimlerinde bile kalıcı yap
    }
    public void OpenScreen(string screenName)
    {
        foreach (var screen in screens)
        {
            if (screen.name == screenName)
            {
                if (currentScreen != null)
                    currentScreen.SetActive(false); // Şu anki ekranı kapat

                screen.panel.SetActive(true); // Yeni ekranı aktif et
                currentScreen = screen.panel; // Güncel ekranı sakla
                CloseAllPopups(); // Ekran değiştiğinde tüm popup'ları kapat
                return;
            }
        }

        Debug.LogWarning($"Screen with name {screenName} not found.");
    }

    /// <summary>
    /// Belirtilen popup penceresini aktif eder.
    /// Ana ekranı kapatmadan popup'ı açar.
    /// </summary>
    /// <param name="popupName">Açılacak popup'ın adı.</param>
    public void OpenPopup(string popupName)
    {
        foreach (var popup in popups)
        {
            if (popup.name == popupName)
            {
                popup.panel.SetActive(true);
                activePopups.Push(popup.panel); // Popup'ı yığına ekle
                return;
            }
        }

        Debug.LogWarning($"Popup with name {popupName} not found.");
    }

    /// <summary>
    /// En üstteki popup'ı kapatır.
    /// </summary>
    public void CloseTopPopup()
    {
        if (activePopups.Count > 0)
        {
            var popupToClose = activePopups.Pop();
            popupToClose.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No popups to close.");
        }
    }

    /// <summary>
    /// Tüm popup pencerelerini kapatır.
    /// </summary>
    public void CloseAllPopups()
    {
        while (activePopups.Count > 0)
        {
            var popupToClose = activePopups.Pop();
            popupToClose.SetActive(false);
        }
    }
}
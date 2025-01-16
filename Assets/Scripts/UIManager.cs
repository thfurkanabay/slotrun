using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Popup> popups;  // Tüm popup pencerelerin listesi

    private GameObject currentScreen; // Şu anda aktif olan ana ekran
    private Stack<GameObject> activePopups = new Stack<GameObject>(); // Aktif popup'ları takip eder
    public Button coinCollectionButton;

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
        Debug.Log($"Opened Screen Name: {screenName}");

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
                popup.OpenPopup();
                popup.gameObject.SetActive(true);
                activePopups.Push(popup.gameObject); // Popup'ı yığına ekle
                return;
            }
        }

        Debug.LogWarning($"Popup with name {popupName} not found.");
    }

    /// <summary>
    /// En üstteki popup'ı kapatır.
    /// </summary>
    /// <summary>
    /// En üstteki popup'ı kapatır.
    /// </summary>
    public void CloseTopPopup()
    {
        if (activePopups.Count > 0)
        {
            var popupToClose = activePopups.Pop();
            var popupComponent = popupToClose.GetComponent<Popup>();
            if (popupComponent != null)
            {
                popupComponent.ClosePopup();
            }
            else
            {
                Debug.LogWarning("Popup component not found on the GameObject.");
            }
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
            var popupComponent = popupToClose.GetComponent<Popup>();
            if (popupComponent != null)
            {
                popupComponent.ClosePopup();
            }
            else
            {
                Debug.LogWarning("Popup component not found on the GameObject.");
            }
        }
    }
    public void LevelPopupCloseButton()
    {
        Debug.Log("LevelPopupCloseButton Clicked");

        if (GameManager.Instance.currentGameState == GameManager.GameState.GameLose)
        {
            Debug.Log("Game State: " + GameManager.Instance.currentGameState);
            CloseTopPopup();
            OpenPopup("Popup_Lose");
        }
        else
        {
            CloseTopPopup();
        }
    }
    /*public void CoinCollectionButton()
    {
        CoinCollection.Instance.CollectCoinsInSequence(coinCollectionButton.transform.position, 2);

    }*/
}
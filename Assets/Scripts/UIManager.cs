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
        public string name;
        public GameObject panel;
    }

    [SerializeField] private List<Panel> screens;
    [SerializeField] private List<Popup> popups;

    private GameObject currentScreen;
    private List<GameObject> activePopups = new List<GameObject>(); // Açık popuplar listesi
    public Button coinCollectionButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OpenScreen(string screenName)
    {
        Debug.Log($"Opened Screen Name: {screenName}");

        foreach (var screen in screens)
        {
            if (screen.name == screenName)
            {
                if (currentScreen != null)
                    currentScreen.SetActive(false);

                screen.panel.SetActive(true);
                currentScreen = screen.panel;
                CloseAllPopups();
                return;
            }
        }

        Debug.LogWarning($"Screen with name {screenName} not found.");
    }

    public void OpenPopup(string popupName)
    {
        foreach (var popup in popups)
        {
            if (popup.name == popupName)
            {
                popup.OpenPopup();
                popup.gameObject.SetActive(true);
                activePopups.Add(popup.gameObject); // Listeye ekle
                return;
            }
        }

        Debug.LogWarning($"Popup with name {popupName} not found.");
    }
    public void CloseTopPopup()
    {
        if (activePopups.Count > 0)
        {
            int lastIndex = activePopups.Count - 1;
            GameObject popupToClose = activePopups[lastIndex]; // Son popup'ı al
            activePopups.RemoveAt(lastIndex); // Stack yerine List olduğu için RemoveAt kullan

            if (popupToClose.activeSelf) // Eğer popup hala aktifse kapat
            {
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
                Debug.LogWarning("Trying to close an already inactive popup.");
            }
        }
        else
        {
            Debug.LogWarning("No popups to close.");
        }
    }

    public void CloseAllPopups()
    {
        while (activePopups.Count > 0)
        {
            var popupToClose = activePopups[activePopups.Count - 1];
            activePopups.RemoveAt(activePopups.Count - 1);
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
}

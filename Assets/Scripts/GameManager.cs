using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton Pattern

    public GroundMovement groundMovement;

    public GameObject tapAnimGameObject;
    public EntryFee entryFee;
    public GameObject screenTransition;

    public PerformanceManager performanceManager;

    public enum GameState
    {
        MainMenu,
        GameStart,
        GameWaiting,
        GamePlaying,
        GamePaused,
        GameWon,
        GameLose,
        GameOver,
        Shop

    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Önceki instance'ı yok et
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne geçişlerinde kalıcı yap
    }
    public AudioClip menuMusic;

    public GameState currentGameState; // Current state of the game
    public Level currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game is starting...");
        InitializeGame();
    }

    public void InitializeGame()
    {
        ChangeGameState(GameState.MainMenu);
        UIManager.Instance.OpenScreen("MainMenu"); // Open the Main Menu Panel

    }

    public void ChangeGameState(GameState newState)
    {
        Debug.Log("currentGameState: " + currentGameState);

        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.MainMenu:
                if (SoundManager.Instance != null)
                {
                    // Check if the current music is the menuMusic and if it is playing
                    if (!SoundManager.Instance.IsMusicPlaying())
                    {
                        // Play the menu music if it's not already playing or it's a different track
                        SoundManager.Instance.PlayMusic(menuMusic, true);
                    }
                }
                UIManager.Instance.OpenScreen("MainMenu");
                Time.timeScale = 1; // Normal game time
                break;

            case GameState.GameWaiting:
                Debug.Log("Game State: Waiting");
                UIManager.Instance.OpenScreen("Game");
                Time.timeScale = 1; // Resume normal gameplay
                break;
            case GameState.GamePlaying:
                Debug.Log("Game State: Playing");
                UIManager.Instance.OpenScreen("Game");
                Time.timeScale = 1; // Resume normal gameplay
                break;

            case GameState.GamePaused:
                Debug.Log("Game State: Paused");
                UIManager.Instance.OpenScreen("PauseMenu");
                Time.timeScale = 0; // Freeze game time
                break;
            case GameState.GameWon:
                Debug.Log("Game State: Game Won");
                UIManager.Instance.OpenPopup("Popup_Won");
                break;
            case GameState.GameLose:
                Debug.Log("Game State: Game Lose");
                UIManager.Instance.OpenPopup("Popup_Lose");
                break;
            case GameState.Shop:
                Debug.Log("Game State: Shop");
                UIManager.Instance.OpenScreen("Shop");
                break;
            default:
                Debug.LogWarning("Unhandled game state!");
                break;
        }
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.GamePaused);
    }
    public void BackToMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
        SoundManager.Instance.PlayMusic(menuMusic, true);
        //SoundManager.Instance.ResetMusic();
        groundMovement.StopMovement();
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.GamePlaying);
    }

    public void ShopScreen()
    {
        ChangeGameState(GameState.Shop);
    }
    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);

    }

    public void GameLose()
    {
        ChangeGameState(GameState.GameLose);

        ObstacleSpawner.Instance.LoseObstaceleSetting();
        CloudMovement.Instance.DestroyClouds();

        StartCoroutine(SoundManager.Instance.SlowAndStopMusic());

        PopupLose.Instance.StartCoroutine(PopupLose.Instance.ActivateButtons());

        if (PopupLose.Instance != null)
        {
            PopupLose.Instance.SetLosePopup();
        }
        else
        {
            Debug.Log("PopupLose instance is null!");
        }
        IncreaseGamePlayed();


        PlayerDataManager.Instance.SavePlayerData();

    }
    public void GameWon()
    {
        ChangeGameState(GameState.GameWon);

        // Send current rewards for won popup
        PopupWon.Instance.SetWonPopup();

        ObstacleSpawner.Instance.LoseObstaceleSetting();
        CloudMovement.Instance.DestroyClouds();
        SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.Win);
        IncreaseGamePlayed();
        IncreaseGameWon();
        PlayerDataManager.Instance.SavePlayerData();
        //StartCoroutine(SoundManager.Instance.SlowAndStopMusic());

    }

    public void StartPlaying()
    {

        PlayerController.Instance.SetPlayerGravityScale(1.0f);
        ObstacleSpawner.Instance.isGameOn = true;
        // Change the gamstate
        ChangeGameState(GameState.GamePlaying);
        PlayerController.Instance.currentState = GameState.GamePlaying;
        // Player isPlayerDead false
        PlayerController.Instance.isPlayerDead = false;
        // Spawn the obstscles
        ObstacleSpawner.Instance.StartCoroutine(ObstacleSpawner.Instance.SpawnObstacleGroups());
        //Start ground movemnet
        // Start cloud movement
        CloudMovement.Instance.StartCloudMovement();
    }
    public void StartGame()
    {
        StartCoroutine(StartGameProcess());
    }
    public IEnumerator StartGameProcess()
    {
        int entryFeeAmount = int.Parse(entryFee.entryFeeText.text); // Entry fee miktarını al
        int userCoins = PlayerDataManager.Instance.playerCoins; // Kullanıcının mevcut parasını al
        Debug.Log("entryFeeAmount: " + entryFeeAmount);

        if (userCoins < entryFeeAmount)
        {
            // Kullanıcının parası yetersizse bir popup göster
            UIManager.Instance.OpenPopup("Popup_AddCoin");
            SoundManager.Instance.PlaySFX(SoundManager.SoundEffect.NotEnoughCoins);
            Debug.Log("Not enough coins to start the game!");
            yield break; // Oyunu başlatma
        }
        screenTransition.SetActive(true);

        UIManager.Instance.OpenScreen("Game");
        ChangeGameState(GameState.GameWaiting);
        ChapterManager.Instance.LoadChapterByIndex(ChapterManager.Instance.currentChapterIndex);

        Animator screenAnimator = screenTransition.GetComponent<Animator>();
        screenTransition.SetActive(true);

        while (screenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f ||
              screenAnimator.IsInTransition(0))
        {
            yield return null; // Bir frame bekle
        }
        //SoundManager.Instance.PlayMusic();   

        tapAnimGameObject.SetActive(true);
        ObstacleSpawner.Instance.level.SetActive(true);
        // Change the gamstate

        UserManager.Instance.DecreaseCoins(entryFeeAmount);        // Player isPlayerDead false

        PlayerController.Instance.isPlayerDead = false;
        // Spawn the obstscles
        //Start ground movemnet
        groundMovement.StartMovement();
        // Start cloud movement
        //CloudMovement.Instance.StartCloudMovement();

        screenTransition.SetActive(false);
        ChapterManager.Instance.PlayChapterMX();

    }

    public void OnScreenTap()
    {
        Debug.Log("Tap the screen");
        if (currentGameState == GameState.GameWaiting)
        {
            tapAnimGameObject.SetActive(false);
            StartPlaying();  // Start the game logic
            PlayerController.Instance.Jump();
        }
    }
    public void IncreaseGamePlayed()
    {
        PlayerDataManager.Instance.totalGamePlayed++;
    }
    public void IncreaseGameWon()
    {
        PlayerDataManager.Instance.totalGameWon++;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton Pattern

    public GroundMovement groundMovement;

    public GameObject tapAnimGameObject;
    public EntryFee entryFee;


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

    private void Update()
    {
        Debug.Log("currentGameState: " + currentGameState);
    }
    public GameState currentGameState; // Current state of the game
    public Level currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        ChangeGameState(GameState.MainMenu);
        UIManager.Instance.OpenScreen("MainMenu"); // Open the Main Menu Panel

    }

    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;

        switch (currentGameState)
        {
            case GameState.MainMenu:
                Debug.Log("Game State: Main Menu");
                SoundManager.Instance.PlayMusic(menuMusic, true);
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
        SoundManager.Instance.ResetMusic();
        groundMovement.StopMovement();
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.GamePlaying);
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

        if (PopupLose.Instance != null)
        {
            PopupLose.Instance.SetLosePopup();
        }
        else
        {
            Debug.Log("PopupLose instance is null!");
        }
    }
    public void GameWon()
    {
        ChangeGameState(GameState.GameWon);
        ObstacleSpawner.Instance.LoseObstaceleSetting();
        CloudMovement.Instance.DestroyClouds();

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
        tapAnimGameObject.SetActive(true);
        ObstacleSpawner.Instance.level.SetActive(true);
        ChapterManager.Instance.PlayChapterMX();
        // Change the gamstate
        ChangeGameState(GameState.GameWaiting);
        UIManager.Instance.OpenScreen("Game");
        ChapterManager.Instance.LoadChapterByIndex(ChapterManager.Instance.currentChapterIndex);

        int entryFeeAmount = int.Parse(entryFee.entryFeeText.text); // entryFeeText'in doğru bir string olduğundan emin olun.
        UserManager.Instance.DecreaseCoins(entryFeeAmount);        // Player isPlayerDead false

        PlayerController.Instance.isPlayerDead = false;
        // Spawn the obstscles
        //Start ground movemnet
        groundMovement.StartMovement();
        // Start cloud movement

        //CloudMovement.Instance.StartCloudMovement();
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

}

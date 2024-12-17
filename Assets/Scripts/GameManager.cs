using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton Pattern

    public GroundMovement groundMovement;

    public enum GameState
    {
        MainMenu,
        GameScreen,
        GameStart,
        Paused,
        GameOver,

    }
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
                UIManager.Instance.OpenScreen("MainMenu");
                Time.timeScale = 1; // Normal game time
                break;

            case GameState.GameScreen:
                Debug.Log("Game State: Playing");
                UIManager.Instance.OpenScreen("Game");
                Time.timeScale = 1; // Resume normal gameplay
                break;

            case GameState.Paused:
                Debug.Log("Game State: Paused");
                UIManager.Instance.OpenScreen("PauseMenu");
                Time.timeScale = 0; // Freeze game time
                break;

            case GameState.GameOver:
                Debug.Log("Game State: Game Over");
                UIManager.Instance.OpenScreen("GameOver");
                Time.timeScale = 0; // Freeze game time
                break;
            case GameState.GameStart:
                Debug.Log("Game State: Game Start");
                //sUIManager.Instance.OpenScreen("GameOver");
                Time.timeScale = 1; // Freeze game time
                break;

            default:
                Debug.LogWarning("Unhandled game state!");
                break;
        }
    }


    public void PauseGame()
    {
        ChangeGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        ChangeGameState(GameState.GameScreen);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void StartGame()
    {
        ChangeGameState(GameState.GameStart);
        UIManager.Instance.OpenScreen("Game");

        if (ChapterManager.Instance != null)
        {
            ChapterManager.Instance.LoadChapterByIndex(ChapterManager.Instance.currentChapterIndex);
        }
        else
        {
            Debug.LogError("ChapterManager instance is null!");
        }
        // Player isPlayerDead false
        PlayerController.Instance.isPlayerDead = false;
        // Spawn the obstscles
        ObstacleSpawner.Instance.StartCoroutine(ObstacleSpawner.Instance.SpawnObstacleGroups());
        //Start ground movemnet
        groundMovement.StartMovement();
        // Start cloud movement
        CloudMovement.Instance.StartCloudMovement();

    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameManager.GameState currentState = GameManager.GameState.GameScreen;

    public float jumpForce = 5f;
    private Rigidbody2D rb;
    public bool isPlayerDead;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // Oyuncunun yeniden doğacağı nokta

    public static PlayerController Instance;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Singleton Instance ayarı
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Daha önce bir instance varsa yok et
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne değişimlerinde bile kalıcı yap
    }

    private void Update()
    {
        if (currentState == GameManager.GameState.GameScreen && Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Collide wih Obstacle");
            //currentState = GameManager.GameState.GameOver;
            HandleDeath(); // Ölüm sonrası işlemleri çağır
        }
    }

    public void SetGameState(GameManager.GameState newState)
    {
        currentState = newState;
    }

    private void HandleDeath()
    {
        isPlayerDead = true;
        //GameManager.Instance.StartGame();
    }
}
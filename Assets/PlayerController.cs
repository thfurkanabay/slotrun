using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager.GameState currentState = GameManager.GameState.GameScreen;

    public float jumpForce = 5f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (other.CompareTag("Goal"))
        {
            //FindObjectOfType<LevelController>().CollectGoal();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            currentState = GameManager.GameState.GameOver;
            //FindObjectOfType<LevelController>().GameOver();
        }
    }

    public void SetGameState(GameManager.GameState newState)
    {
        currentState = newState;
    }
}
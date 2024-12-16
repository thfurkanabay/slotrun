using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Zemin hareket hızını ayarlayın
    public GameObject ground2; // İkinci zemin objesi
    private float groundWidth; // Zemin objesinin genişliği

    void Start()
    {
        // Zemin objesinin genişliğini alıyoruz
        groundWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Zemini sola doğru hareket ettiriyoruz
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Eğer birinci zemin objesi ekranın dışına çıktıysa, onu ikinci zemin objesinin hemen sağında konumlandırıyoruz
        if (transform.position.x <= -groundWidth)
        {
            transform.position = new Vector3(ground2.transform.position.x + groundWidth, transform.position.y, transform.position.z);
        }

        // Eğer ikinci zemin objesi ekranın dışına çıktıysa, onu birinci zemin objesinin hemen sağında konumlandırıyoruz
        if (ground2.transform.position.x <= -groundWidth)
        {
            ground2.transform.position = new Vector3(transform.position.x + groundWidth, ground2.transform.position.y, ground2.transform.position.z);
        }
    }
}
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Zemin hareket hızı
    public GameObject ground1; // Birinci zemin objesi
    public GameObject ground2; // İkinci zemin objesi

    private float groundWidth; // Zemin genişliği
    private bool isMoving = false; // Hareket kontrolü

    private void Start()
    {
        // Zemin genişliğini alıyoruz
        groundWidth = ground1.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        if (!isMoving)
            return;

        // Birinci zemini sola doğru hareket ettir
        ground1.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // İkinci zemini sola doğru hareket ettir
        ground2.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Eğer birinci zemin ekranın solundan çıkarsa, onu ikinci zeminin hemen sağına yerleştir
        if (ground1.transform.position.x <= -groundWidth)
        {
            ground1.transform.position = new Vector3(ground2.transform.position.x + groundWidth, ground1.transform.position.y, ground1.transform.position.z);
        }

        // Eğer ikinci zemin ekranın solundan çıkarsa, onu birinci zeminin hemen sağına yerleştir
        if (ground2.transform.position.x <= -groundWidth)
        {
            ground2.transform.position = new Vector3(ground1.transform.position.x + groundWidth, ground2.transform.position.y, ground2.transform.position.z);
        }
    }

    public void StartMovement()
    {
        isMoving = true; // Hareketi başlat
    }

    public void StopMovement()
    {
        isMoving = false; // Hareketi durdur
    }
}
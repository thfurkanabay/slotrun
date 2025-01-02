using UnityEngine;

public class ParticleAroundImage : MonoBehaviour
{
    public Collider2D targetCollider; // Parçacığın hareket edeceği 2D Collider
    public GameObject particlePrefab; // Parçacık prefabı
    public float speed = 2f; // Parçacığın hareket hızı

    private Transform particle; // Parçacık Transform referansı
    private Vector2[] points; // Collider'ın sınır noktaları
    private int currentPointIndex = 0; // Şu anki hedef noktanın indeksi
    private Vector2 currentTarget; // Şu anki hedef pozisyon

    private void Start()
    {
        // Yeni bir parçacık oluştur
        GameObject particleObject = Instantiate(particlePrefab, transform);
        particle = particleObject.transform;

        // Collider türünü kontrol et ve sınır noktalarını al
        if (targetCollider is PolygonCollider2D polygonCollider)
        {
            points = polygonCollider.points; // Köşe noktalarını al
        }
        else if (targetCollider is EdgeCollider2D edgeCollider)
        {
            points = edgeCollider.points; // Kenar noktalarını al
        }
        else
        {
            Debug.LogError("Bu script sadece PolygonCollider2D veya EdgeCollider2D ile çalışır.");
            return;
        }

        // Collider'ın dünya pozisyonuna dönüştür
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = targetCollider.transform.TransformPoint(points[i]);
        }

        // İlk hedefi ayarla
        currentTarget = points[currentPointIndex];
    }

    private void Update()
    {
        if (points == null || points.Length == 0) return;

        // Parçacığı hedefe doğru hareket ettir
        particle.position = Vector2.MoveTowards(particle.position, currentTarget, speed * Time.deltaTime);

        // Parçacık hedefe ulaştıysa bir sonraki noktaya geç
        if (Vector2.Distance(particle.position, currentTarget) < 0.01f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length; // Döngüsel hareket
            currentTarget = points[currentPointIndex];
        }
    }
}
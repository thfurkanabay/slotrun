using System.Collections;
using UnityEngine;

public class TimeIconAnimation : MonoBehaviour
{
    public float rotationDuration = 1f; // 180° dönme süresi
    public float shakeDuration = 1f; // Sallanma süresi
    public float shakeAmount = 10f; // Sallanma mesafesi (derece cinsinden)
    public float intervalBetweenAnimations = 1f; // Her hareket arasında bekleme süresi

    private bool rotateToRight = true; // İlk dönüş yönü sağa doğru

    private void Start()
    {
        StartCoroutine(AnimateIcon());
    }

    private IEnumerator AnimateIcon()
    {
        while (true)
        {
            // 1. 180° Döndür
            yield return RotateIcon();

            // 2. Sallama hareketi yap
            yield return ShakeIcon();

            // 3. Bekleme süresi
            yield return new WaitForSeconds(intervalBetweenAnimations);
        }
    }

    private IEnumerator RotateIcon()
    {
        float elapsedTime = 0f;
        float startAngle = transform.eulerAngles.z; // Şu anki z açısını al
        float endAngle = rotateToRight ? startAngle + 180f : startAngle - 180f; // Yöne göre hedef açı belirle

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / rotationDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentAngle);
            yield return null;
        }

        // Son pozisyonu kesinleştir
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, endAngle);

        // Dönüş yönünü değiştir
        rotateToRight = !rotateToRight;
    }

    private IEnumerator ShakeIcon()
    {
        float elapsedTime = 0f;
        float startAngle = transform.eulerAngles.z;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Sin dalgası kullanarak sağa sola salınım
            float shakeOffset = Mathf.Sin(elapsedTime * Mathf.PI * 4) * shakeAmount; // Hız için 4 kat hızlı sinüs
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startAngle + shakeOffset);

            yield return null;
        }

        // Sallanma bittikten sonra pozisyonu sıfırla
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startAngle);
    }
}
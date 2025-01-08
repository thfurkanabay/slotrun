using System.Collections;
using UnityEngine;

public class ShineController : MonoBehaviour
{
    public GameObject targetObject;  // Hedef obje
    public GameObject shinePrefab;   // Shine prefab'ı
    public float moveDuration = 1f;  // Hareket süresi
    public float fadeDuration = 1f;  // Opasite azaltma süresi

    // Trigger fonksiyonu, animasyonu başlatmak için çağrılır
    public void TriggerShine()
    {
        // Target objenin RectTransform'ını alıyoruz
        RectTransform targetRectTransform = targetObject.GetComponent<RectTransform>();

        // Target objenin sol üst ve sağ üst pozisyonlarını hesaplıyoruz
        Vector3 topLeftPosition = targetRectTransform.position + new Vector3(-targetRectTransform.rect.width / 2, targetRectTransform.rect.height / 2, 0f);  // Sol üst köşe
        Vector3 topRightPosition = targetRectTransform.position + new Vector3(targetRectTransform.rect.width / 2, targetRectTransform.rect.height / 2, 0f); // Sağ üst köşe

        // Shine prefab'ını target objenin sol üst köşesinde oluşturuyoruz
        GameObject shineInstance = Instantiate(shinePrefab, topLeftPosition, Quaternion.identity);

        // Shine prefab'ını target objenin altına yerleştir
        shineInstance.transform.SetParent(targetObject.transform);

        // Prefab'ı en üstteki child yapmak için sibling sırasını ayarlıyoruz
        shineInstance.transform.SetSiblingIndex(targetObject.transform.childCount - 1);

        // Animasyonu başlat
        StartCoroutine(AnimateShine(shineInstance, topLeftPosition, topRightPosition));
    }

    // Animasyonu başlatan fonksiyon
    private IEnumerator AnimateShine(GameObject shineInstance, Vector3 startPosition, Vector3 endPosition)
    {
        // Shine prefab'ı üzerinde CanvasGroup bileşeni olup olmadığını kontrol ediyoruz
        CanvasGroup canvasGroup = shineInstance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = shineInstance.AddComponent<CanvasGroup>(); // Eğer yoksa, CanvasGroup ekliyoruz
        }

        float elapsedTime = 0f;

        // Hareket animasyonu (pozisyonu değiştiriyoruz)
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            shineInstance.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }
        // Animasyon tamamlandıktan sonra objeyi yok et
        Destroy(shineInstance);
    }
}
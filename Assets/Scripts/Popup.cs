using System.Collections;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Animation popupAnimation; // Popup'ın animasyon bileşeni
    public string openPopupAnimation; // Açılma animasyonu adı
    public string closePopupAnimation; // Kapanma animasyonu adı

    private bool isAnimating = false;

    public void OpenPopup()
    {
        if (!isAnimating && popupAnimation != null)
        {
            gameObject.SetActive(true); // Popup'ı aktif hale getir
            isAnimating = true;
            popupAnimation.Play(openPopupAnimation);
            StartCoroutine(WaitForAnimation());
        }
    }

    // Popup'ı kapatma fonksiyonu
    public void ClosePopup()
    {
        Debug.Log("Popup Close");

        if (!isAnimating && popupAnimation != null)
        {
            isAnimating = true;
            popupAnimation.Play(closePopupAnimation);
            StartCoroutine(DisableAfterAnimation());
        }
    }

    // Animasyonun bitmesini bekleyen bir coroutine
    private IEnumerator WaitForAnimation()
    {
        while (popupAnimation.isPlaying)
        {
            yield return null;
        }
        isAnimating = false; // Animasyon bitti
    }

    // Kapanma animasyonu bittiğinde popup'ı kapat
    private IEnumerator DisableAfterAnimation()
    {
        while (popupAnimation.isPlaying)
        {
            yield return null;
        }
        UIManager.Instance.CloseTopPopup();
        gameObject.SetActive(false); // Popup'ı kapat
        isAnimating = false; // Animasyon bitti
    }
}
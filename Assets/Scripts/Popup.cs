using System;
using System.Collections;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Animation popupAnimation; // Popup'ın animasyon bileşeni
    public string openPopupAnimation; // Açılma animasyonu adı
    public string closePopupAnimation; // Kapanma animasyonu adı

    private bool isAnimating = false;
    public event Action OnPopupClosed;


    private IEnumerator DisableAfterAnimation(float animationTime)
    {
        yield return new WaitForSeconds(animationTime); // Animasyon süresince bekle
        gameObject.SetActive(false); // Sonrasında kapat
    }
    public void OpenPopup()
    {
        if (!isAnimating && popupAnimation != null)
        {
            gameObject.SetActive(true); // Popup'ı aktif hale getir
            isAnimating = true;
            popupAnimation.Play(openPopupAnimation);
            StartCoroutine(WaitForAnimation());
        }
        Debug.Log($"{gameObject.name} Open:");

    }

    // Popup'ı kapatma fonksiyonu
    public void ClosePopup()
    {
        Debug.Log(name + " : Close");

        // Animation bileşeni var mı?
        Animation animation = GetComponent<Animation>();

        if (animation != null && animation.GetClip("popup_close") != null)
        {
            animation.Play("popup_close"); // Kapatma animasyonunu oynat
            StartCoroutine(DisableAfterAnimation(animation.GetClip("popup_close").length)); // Animasyon süresi kadar bekle
        }
        else
        {
            Debug.LogWarning("Close animation not found, closing instantly.");
            gameObject.SetActive(false); // Eğer animasyon yoksa anında kapat
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


}
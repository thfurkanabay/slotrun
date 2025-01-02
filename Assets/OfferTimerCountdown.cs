using UnityEngine;
using TMPro;

public class OfferTimerCountdown : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUI referansı
    public int startingHours = 12;    // Başlangıç saati
    public int startingMinutes = 45;  // Başlangıç dakikası
    public float countdownDuration;   // Zamanın toplam süresi (saniye cinsinden)

    private float timeRemaining;      // Kalan zaman (saniye cinsinden)

    private void Start()
    {
        // Başlangıç zamanını saniyeye çevir
        timeRemaining = (startingHours * 3600) + (startingMinutes * 60);
        UpdateTimerText();
    }

    private void Update()
    {
        // Kalan zamanı azalt
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            // Zaman bittiğinde istediğiniz bir işlem yapabilirsiniz
            timeRemaining = 0;
            UpdateTimerText();
            Debug.Log("Time's up!");
        }
    }

    // Zamanı "12h 45m" formatında güncelle
    private void UpdateTimerText()
    {
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);

        // Saat ve dakika formatını ayarlayın
        timerText.text = string.Format("{0:D2}h {1:D2}m", hours, minutes);
    }
}
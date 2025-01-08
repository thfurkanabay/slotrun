using UnityEngine;
using TMPro;

public class OfferTimerCountdown : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUI referansı

    [Header("Time Display Options")]
    public bool showHours;   // Saatleri göster
    public bool showMinutes; // Dakikaları göster
    public bool showSeconds; // Saniyeleri göster

    [Header("Starting Time")]
    public int startingHours = 12;    // Başlangıç saati
    public int startingMinutes = 45;  // Başlangıç dakikası
    public float startingSeconds = 30f; // Başlangıç saniyesi

    public float timeRemaining;      // Kalan zaman (saniye cinsinden)
    public bool isBonusRewardTimerFinished;

    private void Start()
    {
        StartTimer(); // Zamanlayıcıyı başlat
    }

    private void Update()
    {
        // Kalan zamanı azalt
        if (timeRemaining > 0)
        {
            isBonusRewardTimerFinished = false;
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            // Zaman bittiğinde işlem yapabilirsiniz
            timeRemaining = 0;
            isBonusRewardTimerFinished = true;
            Debug.Log("Time's up!");
        }
    }

    // Zamanı boolean ayarlarına göre günceller
    private void UpdateTimerText()
    {
        int hours = Mathf.FloorToInt(timeRemaining / 3600);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Formatı boolean değerlerine göre oluştur
        string timerString = "";

        if (showHours)
        {
            timerString += $"{hours:D2}h "; // Saatleri ekle
        }

        if (showMinutes)
        {
            timerString += $"{minutes:D2}m "; // Dakikaları ekle
        }

        if (showSeconds)
        {
            timerString += $"{seconds:D2}s"; // Saniyeleri ekle
        }

        timerText.text = timerString.Trim(); // Fazladan boşlukları temizle
    }

    // Zamanlayıcıyı başlat
    public void StartTimer()
    {
        timeRemaining = (startingHours * 3600) + (startingMinutes * 60) + startingSeconds;
        isBonusRewardTimerFinished = false;
        UpdateTimerText();
    }

    // Zamanlayıcıyı sıfırla ve başlat
    public void ResetTimer()
    {
        Debug.Log("Timer Reset!");
        StartTimer();
    }
}
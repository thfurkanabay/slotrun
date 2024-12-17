using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoalSlider : MonoBehaviour
{
    public Slider goalSlider; // Hedef slider
    //public Text progressText; // Yüzdeyi göstermek için
    //public Text completionText; // Tamamlandığında gösterilecek mesaj

    private int currentGoalCount = 0; // Şu ana kadar toplanan GoalObject sayısı
    private int totalGoalAmount = 0; // Toplam hedef sayısı
    private bool isUpdating = false; // Güncelleme işlemi devam ediyor mu?

    public static GoalSlider Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void InitializeGoal(int goalAmount)
    {
        // Slider ve değerleri başlat
        totalGoalAmount = goalAmount;
        currentGoalCount = 0;
        goalSlider.maxValue = totalGoalAmount;
        goalSlider.value = currentGoalCount;

        // Metinleri güncelle
        //UpdateProgressText();
        //completionText.gameObject.SetActive(false); // Tamamlama metnini gizle
    }
    public void IncrementGoalProgress()
    {
        // Eğer zaten güncelleme yapılıyorsa, yeni bir güncelleme yapma
        if (isUpdating || currentGoalCount >= totalGoalAmount) return;

        // Güncellemeyi başlat
        isUpdating = true;

        // İlerlemeyi artır
        currentGoalCount++;

        // Yavaşça slider'ı artır
        StartCoroutine(UpdateSliderValue(goalSlider.value, currentGoalCount));

        // İlerleme yüzdesini güncelle
        //UpdateProgressText();

        // Hedefe ulaşıldığında bitir
        if (currentGoalCount >= totalGoalAmount)
        {
            CompleteGoal();
        }
    }

    private IEnumerator UpdateSliderValue(float startValue, float endValue)
    {
        float duration = 0.5f; // Slider'ın değerini 0.5 saniye içinde artır
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            goalSlider.value = Mathf.Lerp(startValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Son değeri tam olarak ayarla
        goalSlider.value = endValue;

        // Güncelleme işlemini bitir
        isUpdating = false;
    }
    private void UpdateProgressText()
    {
        float percentage = ((float)currentGoalCount / totalGoalAmount) * 100;
        //progressText.text = $"Progress: {Mathf.FloorToInt(percentage)}%";
    }

    private void CompleteGoal()
    {
        // Tamamlama metnini göster
        Debug.Log("Goal Completed");
        //completionText.text = "Completed!";
        //completionText.gameObject.SetActive(true);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    [SerializeField] private Queue<bool> gameScores = new Queue<bool>(); // Kuyruk: true = WIN, false = LOSE
    [SerializeField] private int performanceScore = 0;
    private int maxGames = 10;
    [SerializeField] private int totalWins = 0; // Toplam WIN sayısı

    [SerializeField] public float goalObjectProbability = 10f; // Hedef nesne olasılığı

    private void Start()
    {
        // Başlangıçta örnek bir dizi ekleyelim
        AddGameResult(true); // Win
        AddGameResult(false); // Lose
        AddGameResult(true); // Win
        AddGameResult(true); // Win
        AddGameResult(false); // Lose
        AddGameResult(false); // Lose
        AddGameResult(true); // Win
        AddGameResult(false); // Lose
        AddGameResult(false); // Lose
        AddGameResult(true); // Win

        // Performansı hesapla ve hedef olasılığı güncelle
        UpdatePerformance();
        UpdateGoalProbability();
    }
    private void Update()
    {
        // Kullanıcıdan girişleri kontrol et
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddGameResult(true); // WIN ekle
            Debug.Log("Added WIN");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            AddGameResult(false); // LOSE ekle
            Debug.Log("Added LOSE");
        }
    }

    // Yeni bir oyun sonucu ekle (true = WIN, false = LOSE)
    public void AddGameResult(bool isWin)
    {
        if (gameScores.Count >= maxGames)
        {
            // Kuyruk doluysa ilk elemanı çıkar
            bool removed = gameScores.Dequeue();
            performanceScore -= removed ? 10 : 0; // Çıkarılan elemanın puanını azalt
            if (removed) totalWins--; // Çıkarılan sonuç WIN ise toplam WIN sayısını azalt
        }

        // Yeni sonucu ekle
        gameScores.Enqueue(isWin);
        performanceScore += isWin ? 10 : 0; // Kazandıysa 10 puan ekle

        // WIN sayısını artır
        if (isWin) totalWins++;

        // Performansı ve olasılığı güncelle
        UpdateGameState();

        // Toplam WIN sayısını logla
        Debug.Log($"Total Wins: {totalWins}");
    }


    // Performansı ve hedef nesne olasılığını güncellemek için genel yöntem
    private void UpdateGameState()
    {
        UpdatePerformance();
        UpdateGoalProbability();
    }

    // Performans seviyesini hesapla
    public string GetPerformanceLevel()
    {
        if (performanceScore < 30)

            return "Low Performance";
        else if (performanceScore < 60)
            return "Medium Performance";
        else
            return "High Performance";
    }

    // Hedef olasılığını performansa göre güncelle
    private void UpdateGoalProbability()
    {
        string performanceLevel = GetPerformanceLevel();
        Debug.Log($"Performance Level: {performanceLevel}");
        switch (performanceLevel)
        {
            case "Low Performance":
                goalObjectProbability = 15f; // Düşük performansta olasılığı artır
                break;
            case "Medium Performance":
                goalObjectProbability = 10f;
                break;
            case "High Performance":
                goalObjectProbability = 5f; // Yüksek performansta olasılığı azalt
                break;

        }

        // Olasılığı sınırlı tut (0-100 arasında)
        goalObjectProbability = Mathf.Clamp(goalObjectProbability, 0f, 100f);

        Debug.Log($"Performance: {performanceScore}, Goal Object Probability: {goalObjectProbability}%");
    }

    private void UpdatePerformance()
    {
        Debug.Log($"Performance Score: {performanceScore}, Performance Level: {GetPerformanceLevel()}");
    }

    // Örnek kullanım: Hedef nesne oluşturma olasılığına göre
    public bool ShouldSpawnGoalObject()
    {
        float randomValue = Random.Range(0f, 100f);
        return randomValue < goalObjectProbability;
    }
}
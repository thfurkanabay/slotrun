using UnityEngine;
using System.IO;

public class DebugLogToFile : MonoBehaviour
{
    private string logFilePath;

    private void Awake()
    {
        // Dinamik dosya adı oluştur (Tarih ve saat kullanılarak)
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"DebugLog_{timestamp}.txt";

        // Dosya yolunu belirle
        logFilePath = Path.Combine(Application.persistentDataPath, fileName);

        // Log olayını dinlemek için abone ol
        Application.logMessageReceived += LogToFile;

        // Dosya yolunu konsola yazdır (dosyanın nerede olduğunu görmek için)
        Debug.Log($"Log file created at: {logFilePath}");
    }

    private void LogToFile(string logString, string stackTrace, LogType type)
    {
        // Log mesajını dosyaya yaz
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"[{System.DateTime.Now}] [{type}] {logString}");
            if (type == LogType.Exception)
            {
                writer.WriteLine(stackTrace);
            }
        }
    }

    private void OnDestroy()
    {
        // Olay aboneliğini kaldır
        Application.logMessageReceived -= LogToFile;
    }
}
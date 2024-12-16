using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ChapterManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI chapterNameText;
    //public Text chapterDescriptionText;
    //public GameObject chapterBackgroundImage;
    public SpriteRenderer chapterBackgroundImage;
    public SpriteRenderer chapterGroundImage;
    public Transform characterSpawnPoint;

    [Header("Chapters")]
    public List<Chapter> chapters;
    public int currentChapterIndex = 0;

    private GameObject spawnedCharacter;
    public static ChapterManager Instance;
    private void Awake()
    {
        // Singleton Instance ayarı
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Daha önce bir instance varsa yok et
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Sahne değişimlerinde bile kalıcı yap
    }
    private void Start()
    {
    }

    public void LoadChapterByIndex(int chapterIndex)
    {
        if (chapterIndex >= 0 && chapterIndex < chapters.Count)
        {
            Chapter chapterToLoad = chapters[chapterIndex];
            LoadChapter(chapterToLoad);
        }
        else
        {
            Debug.LogError("Invalid chapter index.");
        }
    }

    public void LoadChapter(Chapter chapter)
    {
        // Set chapter name and description
        if (chapterNameText != null)
            chapterNameText.text = chapter.chapterName;

        //if (chapterDescriptionText != null)
        //    chapterDescriptionText.text = chapter.chapterDescription;

        // Set chapter background and ground images
        if (chapterBackgroundImage.sprite != null && chapter.chapterBackGroundSprite != null)
            chapterBackgroundImage.sprite = chapter.chapterBackGroundSprite;

        if (chapterGroundImage.sprite != null && chapter.chapterGroundSprite != null)
            chapterGroundImage.sprite = chapter.chapterGroundSprite;

        // Spawn chapter character
        if (chapter.chapterCharacter != null && characterSpawnPoint != null)
        {
            if (spawnedCharacter != null)
                Destroy(spawnedCharacter);

            spawnedCharacter = Instantiate(chapter.chapterCharacter, characterSpawnPoint.position, Quaternion.identity);
        }
    }

    public void NextChapter()
    {
        if (currentChapterIndex < chapters.Count - 1)
        {
            currentChapterIndex++;
            LoadChapterByIndex(currentChapterIndex);
        }
        else
        {
            Debug.Log("You are at the last chapter.");
        }
    }

    public void PreviousChapter()
    {
        if (currentChapterIndex > 0)
        {
            currentChapterIndex--;
            LoadChapterByIndex(currentChapterIndex);
        }
        else
        {
            Debug.Log("You are at the first chapter.");
        }
    }
}
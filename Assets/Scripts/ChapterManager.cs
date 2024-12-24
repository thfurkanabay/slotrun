using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ChapterManager : MonoBehaviour
{
    [Header("Chapter Elements")]
    public TextMeshProUGUI chapterNameText;
    public TextMeshProUGUI chapterDescriptionText;
    //public GameObject chapterBackgroundImage;
    public SpriteRenderer chapterBackgroundImage;
    //public SpriteRenderer chapterGroundImage;
    public Image chapterGoalObjectImage;
    public Transform characterSpawnPoint;
    public AudioClip chapterMXClip;
    public SpriteRenderer chapterIcon;

    public Slider goalSlider;

    [Header("Chapters")]
    public List<Chapter> chapters;
    public int currentChapterIndex = 0;
    public int currentLevelIndex = 0;
    public GameObject spawnedCharacter;

    public Image currentGoalObjectImage;
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

        if (chapterDescriptionText != null)
            chapterDescriptionText.text = chapter.chapterDescription;

        // Set chapter background and ground images
        if (chapterBackgroundImage.sprite != null && chapter.chapterBackGroundSprite != null)
            chapterBackgroundImage.sprite = chapter.chapterBackGroundSprite;

        //        if (chapterGroundImage.sprite != null && chapter.chapterGroundSprite != null)
        //           chapterGroundImage.sprite = chapter.chapterGroundSprite;

        if (chapter.chapterCharacter != null && characterSpawnPoint != null)
        {
            if (spawnedCharacter != null)
                Destroy(spawnedCharacter);

            Vector3 spawnPosition = characterSpawnPoint.position;
            spawnPosition.z = -1; // Set the z position to -1

            spawnedCharacter = Instantiate(chapter.chapterCharacter, spawnPosition, Quaternion.identity);
        }
        if (chapterGoalObjectImage.sprite != null && chapter.goalObjectImagelist != null)
        {
            int randomIndex = Random.Range(0, chapter.goalObjectImagelist.Count);
            chapterGoalObjectImage.sprite = chapter.goalObjectImagelist[randomIndex];
            currentGoalObjectImage = chapterGoalObjectImage;
        }
        if (chapterMXClip != null && chapter.chapterMX != null)
        {
            chapterMXClip = chapter.chapterMX;
        }
        if (chapterIcon != null && chapter.chapterIcon != null)
        {
            chapterIcon.sprite = chapter.chapterIcon;
        }

        // Set the goal amount for slider
        GoalSlider.Instance.InitializeGoal(chapter.levels[currentLevelIndex].goalObjectAmount);
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
    public void PlayChapterMX()
    {
        SoundManager.Instance.PlayMusic(chapters[currentChapterIndex].chapterMX, true);
    }
}
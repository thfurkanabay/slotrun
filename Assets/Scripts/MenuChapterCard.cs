using UnityEngine;
using UnityEngine.UI;

public class MenuChapterCard : MonoBehaviour
{
    [Header("Chapter Elements")]
    public int chapterNo;
    public Image chapterIcon;

    public PopupInfo popupInfo;
    public PopupLevel popupLevel;


    void Start()
    {
        LoadChapterInformaitons();

    }

    public void LoadChapterInformaitons()
    {
        chapterIcon.sprite = ChapterManager.Instance.chapters[chapterNo].chapterIcon;
    }

    public void InformationButtonClick()
    {
        if (popupInfo != null)
        {
            ChapterManager.Instance.currentChapterIndex = chapterNo;

            // Şu anki kartın bilgilerini Popup'a gönder
            popupInfo.SetChapterCard(this);
            UIManager.Instance.OpenPopup("PopupDescription");
        }
        else
        {
            Debug.LogError("PopupInfo bulunamadı!");
        }
    }
    public void LevelButtonClick()
    {
        if (popupLevel != null)
        {
            // Şu anki kartın bilgilerini Popup'a gönder
            popupLevel.SetLevelCard(this);

            UIManager.Instance.OpenPopup("PopupLevel");
        }
        else
        {
            Debug.LogError("PopupLevel bulunamadı!");
        }
    }
}
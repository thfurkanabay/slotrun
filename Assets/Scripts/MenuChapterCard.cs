using UnityEngine;
using UnityEngine.UI;

public class MenuChapterCard : MonoBehaviour
{
    [Header("Chapter Elements")]
    public int chapterNo;
    public Image chapterIcon;

    public PopupInfo popupInfo;
    public PopupLevel popupLevel;
    public Image screenTransitionChapterIcon;

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
            //popupInfo.OpenPopup();

            UIManager.Instance.OpenPopup("Popup_Chapter_Information");

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
            screenTransitionChapterIcon.sprite = chapterIcon.sprite;

            UIManager.Instance.OpenPopup("Popup_Level");
        }
        else
        {
            Debug.LogError("There is no PopupLevel!");
        }
    }
}
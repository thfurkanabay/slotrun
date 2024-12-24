using UnityEngine;
using UnityEngine.UI;

public class MenuChapterCard : MonoBehaviour
{
    [Header("Chapter Elements")]
    public int chapterNo;
    public Image chapterIcon;

    public PopupInfo popupInfo;

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
            // Şu anki kartın bilgilerini Popup'a gönder
            popupInfo.SetChapterCard(this);
            UIManager.Instance.OpenPopup("PopupDescription");
        }
        else
        {
            Debug.LogError("PopupInfo bulunamadı!");
        }
    }
}
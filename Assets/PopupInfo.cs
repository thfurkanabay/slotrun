using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfo : Popup
{
    public TextMeshProUGUI chapterInfo;
    public TextMeshProUGUI chapterName;

    public Image chapterIcon;

    private MenuChapterCard menuChapterCard; // Gönderilen kart referansı

    // Bilgi set etmek için bir yöntem
    public void SetChapterCard(MenuChapterCard card)
    {
        menuChapterCard = card;
        UpdatePopupInfo();
    }

    private void UpdatePopupInfo()
    {
        if (menuChapterCard != null)
        {
            int chapterNo = menuChapterCard.chapterNo;
            chapterIcon.sprite = ChapterManager.Instance.chapters[chapterNo].chapterIcon;
            chapterInfo.text = ChapterManager.Instance.chapters[chapterNo].chapterDescription;
            chapterName.text = ChapterManager.Instance.chapters[chapterNo].chapterName;
        }
        else
        {
            Debug.LogError("MenuChapterCard referansı atanmamış!");
        }
    }
}
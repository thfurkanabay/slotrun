using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLevel : Popup
{
    public MenuChapterCard menuChapterCard; // Gönderilen kart referansı
    public TextMeshProUGUI chapterName;
    public Image chapterImage;

    public void SetLevelCard(MenuChapterCard card)
    {
        menuChapterCard = card;
        UpdatePopupLevel();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void UpdatePopupLevel()
    {
        if (menuChapterCard != null)
        {
            int chapterNo = menuChapterCard.chapterNo;
            ChapterManager.Instance.currentChapterIndex = menuChapterCard.chapterNo;
            chapterName.text = ChapterManager.Instance.chapters[chapterNo].chapterName;
            chapterImage.sprite = ChapterManager.Instance.chapters[chapterNo].chapterIcon;

        }
        else
        {
            Debug.LogError("MenuChapterCard referansı atanmamış!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupLevel : Popup
{
    public MenuChapterCard menuChapterCard; // Gönderilen kart referansı
    public TextMeshProUGUI chapterName;
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
        }
        else
        {
            Debug.LogError("MenuChapterCard referansı atanmamış!");
        }
    }
}

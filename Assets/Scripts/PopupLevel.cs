using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLevel : Popup
{
    public MenuChapterCard menuChapterCard; // Gönderilen kart referansı

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
            ChapterManager.Instance.currentChapterIndex = menuChapterCard.chapterNo;
        }
        else
        {
            Debug.LogError("MenuChapterCard referansı atanmamış!");
        }
    }
}

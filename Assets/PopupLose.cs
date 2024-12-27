using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : Popup
{
    public Image characterLosePose;

    public static PopupLose Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLosePopup()
    {
        UpdatePopupLose();
    }
    public void UpdatePopupLose()
    {
        if (ChapterManager.Instance != null)
        {
            characterLosePose.sprite = ChapterManager.Instance.chapters[ChapterManager.Instance.currentChapterIndex].chapterCharacterLosePose;
        }
        else
        {
            Debug.LogError("MenuChapterCard reference not assigned!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : Popup
{
    public Image characterLosePose;
    public int chapterNo;

    public static PopupLose Instance;
    public List<Button> buttons;

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

    public void SetLosePopup()
    {
        DeactivateButton();

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
    public void PlayAgainButton()
    {
        Debug.Log("Play Again Button Clicked");

        UIManager.Instance.OpenPopup("Popup_Level");

    }
    public IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(2.1f);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void DeactivateButton()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
    }
}




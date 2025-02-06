using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupAfterWon : Popup
{
    public List<Button> buttons;

    public static PopupAfterWon Instance;
    private void Awake()
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

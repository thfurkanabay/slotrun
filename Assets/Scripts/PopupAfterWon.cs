using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAfterWon : Popup
{

    public void PlayAgainButton()
    {
        Debug.Log("Play Again Button Clicked");

        UIManager.Instance.OpenPopup("Popup_Level");

    }
}

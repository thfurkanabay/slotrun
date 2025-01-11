using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup_User : Popup
{
    public Image userIcon;
    public TextMeshProUGUI userName;
    public TextMeshProUGUI userLevel;
    public TextMeshProUGUI userCoin;
    public TextMeshProUGUI userGem;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUserPopup()
    {
        UpdateUserPopup();
    }

    private void UpdateUserPopup()
    {
        if (PlayerDataManager.Instance == null)
        {
            Debug.LogError("PlayerDataManager.Instance is null. Ensure PlayerDataManager is initialized.");
            return;
        }

        if (UserManager.Instance.userIconImageList[PlayerDataManager.Instance.userIconImageIndex] == null)
        {
            Debug.LogError("userIcon is null in PlayerDataManager. Assign it properly.");
            return;
        }

        if (UserManager.Instance.userIconImageList[PlayerDataManager.Instance.userIconImageIndex] == null)
        {
            Debug.LogWarning("userIcon.sprite is null. Assign a default sprite.");
        }
        else
        {
            userIcon.sprite = UserManager.Instance.userIconImageList[PlayerDataManager.Instance.userIconImageIndex];
        }

        userName.text = PlayerDataManager.Instance.playerName ?? "Unknown Player";
        userLevel.text = PlayerDataManager.Instance.userLevel.ToString();
        userCoin.text = PlayerDataManager.Instance.playerCoins.ToString();
        userGem.text = PlayerDataManager.Instance.playerGems.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWon : Popup
{
    [Header("Current Game Won Reward")]
    public TextMeshProUGUI coinCurrentGameRewardText;
    public TextMeshProUGUI xpCurrentGameRewardText;
    public TextMeshProUGUI gemCurrentGameRewardText;
    public static PopupWon Instance;

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
    public void SetWonPopup()
    {
        SetCurrentGameReward();
    }

    public void SetCurrentGameReward()
    {
        Debug.Log("RewardsController.Instance.coinRewardText.text" + RewardsController.Instance.coinRewardText.text);
        Debug.Log("RewardsController.Instance.gemRewardText.text" + RewardsController.Instance.gemRewardText.text);

        coinCurrentGameRewardText.text = RewardsController.Instance.coinRewardText.text;
        gemCurrentGameRewardText.text = RewardsController.Instance.gemRewardText.text;
        xpCurrentGameRewardText.text = RewardsController.Instance.xpRewardText.text;
    }
    public void OnClickClaimButton()
    {
        ClaimRewards.Instance.ClaimReward();
        //Debug.Log("coinReward" + RewardsController.Instance.coinReward);
        //Debug.Log("gemReward" + RewardsController.Instance.gemReward);
        //Debug.Log("xpReward" + RewardsController.Instance.xpReward);



    }
    public void OnClickClaimWithAdsButton()
    {
        // Will ad watch ads and get reward
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    [Header("Menu Level Card Rewards")]
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI gemRewardText;
    public TextMeshProUGUI xpRewardText;




    [Header("Reward Settings")]
    public int coinReward;
    public int gemReward;
    public int xpReward;

    public float coinMultiplier = 2.0f; // Default multiplier for coins
    public float gemMultiplier = 0.2f; // Default multiplier for gems (entry fee / 1000)
    public float xpMultiplier = 0.1f; // Default multiplier for gems (entry fee / 1000)

    public static RewardsController Instance;

    [Header("Entry Fee")]
    public EntryFee entryFee;

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
    /// <summary>
    /// Calculates rewards based on the entry fee and multipliers.
    /// </summary>
    public void LoadRewards()
    {
        if (entryFee != null)
        {
            coinReward = Mathf.RoundToInt(entryFee.currentFee * coinMultiplier);
            gemReward = Mathf.RoundToInt(entryFee.currentFee * gemMultiplier);
            xpReward = Mathf.RoundToInt(entryFee.currentFee * xpMultiplier);

        }
        else
        {
            Debug.LogError("EntryFee is not assigned!");
        }
    }
    /// <summary>
    /// Displays calculated rewards on the UI.
    /// </summary>
    public void UpdateRewardTexts()
    {
        coinRewardText.text = coinReward.ToString() + " COIN";
        gemRewardText.text = gemReward.ToString() + " GEM";
        xpRewardText.text = xpReward.ToString() + " XP";
    }

}
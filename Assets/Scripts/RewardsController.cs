using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI coinRewardText;
    public TextMeshProUGUI gemRewardText;

    [Header("Reward Settings")]
    public int coinReward;
    public int gemReward;
    public float coinMultiplier = 2.0f; // Default multiplier for coins
    public float gemMultiplier = 0.2f; // Default multiplier for gems (entry fee / 1000)

    [Header("Entry Fee")]
    public EntryFee entryFee;

    void Start()
    {
        //UpdateRewardTexts();
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
        coinRewardText.text = coinReward.ToString();
        gemRewardText.text = gemReward.ToString();
    }
}
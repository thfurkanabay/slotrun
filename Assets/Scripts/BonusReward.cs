using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusReward : MonoBehaviour
{
    public int bonusReward;
    public bool isTimeFinished = false;
    public OfferTimerCountdown offerTimerCountdown;
    public GameObject BonusRewardPanel;
    public GameObject TimerPanel;

    public static BonusReward Instance;

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

    void Update()
    {
        if (offerTimerCountdown.timeRemaining <= 0)
        {
            isTimeFinished = true;
            ActivateBonusRewardPanel();
        }
        else
        {
            isTimeFinished = false;
            ActivateTimerPanel();
        }
    }

    public void ActivateBonusRewardPanel()
    {
        BonusRewardPanel.SetActive(true);
        TimerPanel.SetActive(false);
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void ActivateTimerPanel()
    {
        TimerPanel.SetActive(true);
        BonusRewardPanel.SetActive(false);
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void OnClickClainBonusButton()
    {
        ClaimRewards.Instance.ClaimBonusReward();

        // Bonus ödülü aldıktan sonra süreyi sıfırla ve yeniden başlat
        RestartTimer();

        BonusRewardPanel.SetActive(false);
    }

    private void RestartTimer()
    {
        // Zamanlayıcıyı sıfırla ve yeniden başlat
        offerTimerCountdown.ResetTimer();
    }
}
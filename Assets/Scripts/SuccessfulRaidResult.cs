using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessfulRaidResult : MonoBehaviour
{
    [SerializeField] Text shipNameText;
    [SerializeField] Text crewCap;
    [SerializeField] Text healthText;
    [SerializeField] Text earnedGoldAmount;
    [SerializeField] Button nextButton;
    [SerializeField] ShipCaptureOrLoss shipCaptureOrLoss;

    public void ShowRaidOutcome(Player.RaidOutcome raidOutcome)
    {
        Ship raider = raidOutcome.raid.raider;
        shipNameText.text = raider.name;
        crewCap.text = (raider.currCrewCapacity - raidOutcome.crewLost).ToString() + "(-" + raidOutcome.crewLost + ")";
        healthText.text = (raider.currHealth - raidOutcome.damageTaken).ToString() + "(-" + raidOutcome.damageTaken + ")";
        earnedGoldAmount.text = "+" + raidOutcome.goldAmount;
        nextButton.onClick.AddListener(() => GoToNext(raidOutcome));
        gameObject.SetActive(true);
    }

    public void GoToNext(Player.RaidOutcome raidOutcome)
    {
        Ship target = raidOutcome.raid.target;
        if (raidOutcome.isCaptured)
        {
            shipCaptureOrLoss.ShowCapturedShip(target);
        } else
        {
            GameManager.instance.player.ShowNextRaidOutcome();
        }
        nextButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}

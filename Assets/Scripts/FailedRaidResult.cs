using UnityEngine;
using UnityEngine.UI;

public class FailedRaidResult : MonoBehaviour
{
    [SerializeField] Text shipNameText;
    [SerializeField] Text crewCap;
    [SerializeField] Text healthText;
    [SerializeField] Button nextButton;
    [SerializeField] ShipCaptureOrLoss shipCaptureOrLoss;


    public void ShowRaidOutcome(Player.RaidOutcome raidOutcome)
    {
        Ship raider = raidOutcome.raid.raider;
        shipNameText.text = raider.name;
        crewCap.text = raider.currCrewCapacity.ToString() + "(-" + raidOutcome.crewLost + ")";
        healthText.text = raider.currHealth.ToString() + "(-" + raidOutcome.damageTaken + ")";
        gameObject.SetActive(true);

        nextButton.onClick.AddListener(() => GoToNext(raidOutcome));
    }

    public void GoToNext(Player.RaidOutcome raidOutcome)
    {
        Ship target = raidOutcome.raid.target;
        if (raidOutcome.isLost)
        {
            shipCaptureOrLoss.ShowCapturedShip(target);
        }
        else
        {
            GameManager.instance.player.ShowNextRaidOutcome();
        }
        gameObject.SetActive(false);
    }

}

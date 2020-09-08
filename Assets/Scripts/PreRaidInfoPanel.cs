using UnityEngine;
using UnityEngine.UI;

public class PreRaidInfoPanel : MonoBehaviour
{
    [SerializeField]
    Text shipName;

    [SerializeField]
    Text shipClassName;

    [SerializeField]
    Text duration;

    [SerializeField]
    Text goldAmount;

    [SerializeField]
    Text successRate;

    [SerializeField]
    Text captureRate;

    [SerializeField]
    Text numCrew;

    [SerializeField]
    Text healthText;

    [SerializeField]
    Button raid;

    Ship shipToRaid;
    RaidSpawner.RaidInfo raidInfo;

    public void Start()
    {
        raid.onClick.AddListener(() => Raid());
    }

    public void SetShipInfo(Ship s)
    {
        shipToRaid = s;
        shipName.text = s.name;
        shipClassName.text = s.shipClass.shipClassName;
        numCrew.text = s.currCrewCapacity.ToString() + "/" + s.shipClass.defaultCrewCapacity.ToString();
        healthText.text = s.currHealth.ToString() + "/" + s.shipClass.defaultMaxHealth.ToString();
    }

    public void SetRaidInfo(RaidSpawner.RaidInfo raidInfo)
    {
        duration.text = "Raid Length: " + raidInfo.duration;
        goldAmount.text = "Potential Gold Return: " + raidInfo.goldAmount + "G";
        successRate.text = "Success Rate: " + raidInfo.successRate + "%";
        captureRate.text = "Capture Rate: " + raidInfo.captureChance + "%";
        this.raidInfo = raidInfo;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Raid()
    {
        GameManager.instance.player.StartRaid(shipToRaid, raidInfo);
        Hide();
    }
}

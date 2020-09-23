using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInfoList : MonoBehaviour
{
    [SerializeField] Text shipNameLabel;
    [SerializeField] Text shipClassLabel;
    [SerializeField] Text numCrew;
    [SerializeField] Text currHealth;
    [SerializeField] Button hireCrewButton;
    [SerializeField] Button upgradeButton;
    [SerializeField] Button nextShipButton;
    [SerializeField] Image shipImage;

    int currShipIndex = 0;

    public void ShowShipInfo()
    {
        FillShipInfo();
        ConfigureButtons();
        gameObject.SetActive(true);
    }

    Ship GetCurrentShip()
    {
        List<Ship> ships = GameManager.instance.player.GetCurrShips();
        Ship currShip = ships[currShipIndex];
        return currShip;
    }

    void ConfigureButtons()
    {
        Ship currShip = GetCurrentShip();
        hireCrewButton.GetComponentInChildren<Text>().text = "Hire Crew (" + currShip.shipClass.crewHireCost + ")";
        if (currShip.shipClass.shipClassName == "Man O War")
        {
            upgradeButton.gameObject.SetActive(false);
        }
        else
        {
            upgradeButton.GetComponentInChildren<Text>().text = "Upgrade (" + currShip.shipClass.upgradeCost + ")";
        }

        // Remove old listeners
        hireCrewButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        nextShipButton.onClick.RemoveAllListeners();

        // Attach listeners
        hireCrewButton.onClick.AddListener(() => HireCrew());
        upgradeButton.onClick.AddListener(() => Upgrade());
        nextShipButton.onClick.AddListener(() => ShowNextShip());
    }

    void FillShipInfo()
    {
        Ship currShip = GetCurrentShip();
        shipImage.sprite = currShip.shipClass.sprite;
        shipNameLabel.text = currShip.name;
        shipClassLabel.text = currShip.shipClass.shipClassName;
        numCrew.text = currShip.currCrewCapacity + "/" + currShip.shipClass.defaultCrewCapacity;
        currHealth.text = currShip.currHealth + "/" + currShip.shipClass.defaultMaxHealth;
    }

    public void Upgrade()
    {
        Ship ship = GetCurrentShip();
        bool didUpgrade = GameManager.instance.player.UpgradeShip(ship);
        if (didUpgrade)
        {
            ConfigureButtons();
            FillShipInfo();
        }
    }

    public void HireCrew()
    {
        Ship currShip = GetCurrentShip();
        bool result = GameManager.instance.player.HireCrew(currShip);
        if (result)
        {
            numCrew.text = currShip.currCrewCapacity + "/" + currShip.shipClass.defaultCrewCapacity;
        }
    }

    public void ShowNextShip()
    {
        currShipIndex++;
        List<Ship> ships = GameManager.instance.player.GetCurrShips();
        currShipIndex %= ships.Count;
        ShowShipInfo();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

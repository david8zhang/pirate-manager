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

    int currShipIndex = 0;

    public void ShowShipInfo()
    {
        List<Ship> ships = GameManager.instance.player.GetCurrShips();
        Ship currShip = ships[currShipIndex];
        shipNameLabel.text = currShip.name;
        shipClassLabel.text = currShip.shipClass.shipClassName;
        numCrew.text = currShip.currCrewCapacity + "/" + currShip.shipClass.defaultCrewCapacity;
        currHealth.text = currShip.currHealth + "/" + currShip.shipClass.defaultMaxHealth;

        // Remove old listeners
        hireCrewButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();
        nextShipButton.onClick.RemoveAllListeners();

        // Attach listeners
        hireCrewButton.GetComponentInChildren<Text>().text = "Hire Crew (" + currShip.shipClass.crewHireCost + ")";
        hireCrewButton.onClick.AddListener(() => HireCrew());
        if (currShip.shipClass.shipClassName == "Man O War")
        {
            upgradeButton.gameObject.SetActive(false);
        }else
        {
            upgradeButton.GetComponentInChildren<Text>().text = "Upgrade (" + currShip.shipClass.upgradeCost + ")";
        }
        nextShipButton.onClick.AddListener(() => ShowNextShip());


        gameObject.SetActive(true);
    }

    public void HireCrew()
    {
        List<Ship> ships = GameManager.instance.player.GetCurrShips();
        Ship currShip = ships[currShipIndex];
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

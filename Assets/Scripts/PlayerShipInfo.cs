using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipInfo : MonoBehaviour
{

    [SerializeField] Image image;
    [SerializeField] Text shipNameText;
    [SerializeField] Text shipClassNameText;
    [SerializeField] Text numCrew;
    [SerializeField] Text healthText;
    [SerializeField] Button raidButton;
    [SerializeField] Button repairButton;
    [SerializeField] Text repairingText;

    Ship selectedShip;

    public void ShowInfo(Ship ship)
    {
        selectedShip = ship;

        // Populate the UI with ship's information
        image.sprite = ship.shipClass.sprite;
        shipNameText.text = ship.name;
        shipClassNameText.text = ship.shipClass.shipClassName;
        numCrew.text = ship.currCrewCapacity.ToString() + "/" + ship.shipClass.defaultCrewCapacity.ToString();
        healthText.text = ship.currHealth.ToString() + "/" + ship.shipClass.defaultMaxHealth.ToString();
        gameObject.SetActive(true);

        // Configure & Setup the repair button
        if (ship.isRepairing)
        {
            repairButton.gameObject.SetActive(false);
            raidButton.gameObject.SetActive(false);
            repairingText.gameObject.SetActive(true);
        } else
        {
            repairingText.gameObject.SetActive(false);
            int repairDuration = GameManager.instance.player.GetRepairDuration(ship);
            repairButton.GetComponentInChildren<Text>().text = "Repair (" + repairDuration + " turns)";
            repairButton.onClick.AddListener(() => RepairShip());
            raidButton.onClick.AddListener(() => GameManager.instance.player.SelectRaidTarget());
        }

    }

    public void FinishedRepairing()
    {
        int repairDuration = GameManager.instance.player.GetRepairDuration(selectedShip);
        repairButton.gameObject.SetActive(true);
        raidButton.gameObject.SetActive(true);
        repairButton.GetComponentInChildren<Text>().text = "Repair (" + repairDuration + " turns)";
        repairingText.gameObject.SetActive(false);
    }

    public void RepairShip()
    {
        repairButton.gameObject.SetActive(false);
        raidButton.gameObject.SetActive(false);
        repairingText.gameObject.SetActive(true);
        repairingText.text = "Repairing...";
        GameManager.instance.player.RepairShip(selectedShip);
    }

    public void UpdateHealth()
    {
        healthText.text = selectedShip.currHealth.ToString() + "/" + selectedShip.shipClass.defaultMaxHealth.ToString();
    }

    public Ship GetSelectedShip()
    {
        return selectedShip;
    }

    public void BackToOverworld()
    {
        Hide();
        GameManager.instance.player.BackToOverworld();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

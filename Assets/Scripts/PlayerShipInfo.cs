using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipInfo : MonoBehaviour
{

    [SerializeField]
    Image image;

    [SerializeField]
    Text shipNameText;

    [SerializeField]
    Text shipClassNameText;

    [SerializeField]
    Text numCrew;

    [SerializeField]
    Text healthText;

    [SerializeField]
    Button raidButton;

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

    public void Start()
    {
        raidButton.onClick.AddListener(() => GameManager.instance.player.SelectRaidTarget());
    }
}

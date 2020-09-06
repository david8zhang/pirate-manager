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

    public void ShowInfo(Ship ship)
    {
        image.sprite = ship.shipClass.sprite;
        shipNameText.text = ship.name;
        shipClassNameText.text = ship.shipClass.shipClassName;
        numCrew.text = ship.currCrewCapacity.ToString() + "/" + ship.shipClass.defaultCrewCapacity.ToString();
        healthText.text = ship.currHealth.ToString() + "/" + ship.shipClass.defaultMaxHealth.ToString();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipCaptureOrLoss : MonoBehaviour
{
    [SerializeField] Text shipName;
    [SerializeField] Text shipClass;
    [SerializeField] Text titleLabel;
    [SerializeField] Button nextButton;

    public void ShowCapturedShip(Ship ship)
    {
        shipName.text = ship.name;
        shipClass.text = ship.shipClass.shipClassName;
        titleLabel.text = "You captured a ship!";
        gameObject.SetActive(true);
    }

    public void ShowLostShip(Ship ship)
    {
        shipName.text = ship.name;
        shipClass.text = ship.shipClass.shipClassName;
        titleLabel.text = "You lost a ship...";
        gameObject.SetActive(true);
    }

    public void Start()
    {
        nextButton.onClick.AddListener(() => OnNextClick());
    }

    public void OnNextClick()
    {
        gameObject.SetActive(false);
        GameManager.instance.player.ShowNextRaidOutcome();
    }
}

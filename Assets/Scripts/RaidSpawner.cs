﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject shipPrefab;

    [SerializeField]
    PreRaidInfoPanel preRaidInfoPanel;

    List<Ship> raidableShips = new List<Ship>();
    int numRaidable = 3;
    readonly Dictionary<string, int> numberToShipClassMap = new Dictionary<string, int>
        {
            { "Sloop", 1 },
            { "Caravel", 2 },
            { "Galleon", 3 },
            { "ManOWar", 4 }
        };

    readonly Dictionary<string, int> nameToGoldMapping = new Dictionary<string, int>();

    public struct RaidInfo
    {
        public int goldAmount;
        public int duration;
        public float successRate;
        public float captureChance;
    }

    public void Start()
    {
        SpawnRaidableShips();
    }

    void SpawnRaidableShips()
    {
        for (int i = 0; i < numRaidable; i++)
        {
            Ship ship = CreateNewShip("Ship " + i);
            raidableShips.Add(ship);
            nameToGoldMapping[ship.name] = GetGoldAmount(ship);
        }
        PlaceShips();
    }

    public void LoseRaidableShip(Ship s)
    {
        int indexToRemove = -1;
        for (int i = 0; i < raidableShips.Count; i++)
        {
            if (raidableShips[i].name == s.name)
            {
                indexToRemove = i;
            }
        }
        nameToGoldMapping.Remove(s.name);
        raidableShips.RemoveAt(indexToRemove);
    }

    public void Tick()
    {
        ReplaceLostShips();
    }

    public void ReplaceLostShips()
    {
        int diff = numRaidable - raidableShips.Count;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                Ship ship = CreateNewShip("Ship " + Random.Range(0, 1000));
                raidableShips.Add(ship);
                nameToGoldMapping[ship.name] = GetGoldAmount(ship);
                ship.MoveToRandPos();
            }
        }
    }

    Ship CreateNewShip(string name)
    {
        Ship starter = Instantiate(shipPrefab, transform.position, Quaternion.identity).GetComponent<Ship>();
        starter.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        starter.Initialize(name, ShipCatalog.instance.GetShipClassForName("Sloop"));
        starter.SetClickHandler(SelectShip);
        return starter;
    }

    void SelectShip(Ship s)
    {
        if (GameManager.instance.player.isRaiding && !GameManager.instance.player.IsShipInRaid(s.name))
        {
            Ship playerShip = GameManager.instance.player.shipToRaidWith;
            int goldAmount = nameToGoldMapping[s.name];
            float totalSuccessPercentage = CalculateTotalSuccessPercentage(playerShip, s);
            int duration = Map.GetDistanceBetweenPoints(playerShip.gridPos, s.gridPos);
            float capturePercentage = CalculateCapturePercentage(playerShip, s);

            RaidInfo raidInfo = new RaidInfo
            {
                goldAmount = goldAmount,
                duration = duration + 1,
                successRate = totalSuccessPercentage,
                captureChance = capturePercentage
            };
            preRaidInfoPanel.Show();
            preRaidInfoPanel.SetShipInfo(s);
            preRaidInfoPanel.SetRaidInfo(raidInfo);
        }
    }

    int GetGoldAmount(Ship target)
    {
        switch (target.shipClass.shipClassName)
        {
            case "Sloop":
                {
                    return Random.Range(100, 500);
                }
            case "Caravel":
                {
                    return Random.Range(600, 1200);
                }
            case "Galleon":
                {
                    return Random.Range(1300, 2000);
                }
            case "ManOWar":
                {
                    return Random.Range(2100, 3000);
                }
            default:
                return 1000;
        }
    }

    float CalculateCapturePercentage(Ship raider, Ship target)
    {
        int crewDiff = target.currCrewCapacity - raider.currCrewCapacity;
        int classDiff = numberToShipClassMap[target.shipClass.shipClassName] - numberToShipClassMap[raider.shipClass.shipClassName];
        float crewPercentage = 25 - (4.1667f * crewDiff);
        float classPercentage = 25 - (8.333f * classDiff);
        return crewPercentage + classPercentage;
    }

    int CalculateTotalSuccessPercentage(Ship raider, Ship target)
    {
        return CalculatePercentageBasedOnCrew(raider, target) + CalculatePercentageBasedOnShipClass(raider, target) + CalculatePercentageBasedOnHealth(raider, target);
    }

    int CalculatePercentageBasedOnShipClass(Ship raider, Ship target)
    {

        int raiderClass = numberToShipClassMap[raider.shipClass.shipClassName];
        int targetClass = numberToShipClassMap[target.shipClass.shipClassName];
        int classDiff = targetClass - raiderClass;
        return Mathf.RoundToInt(20 - (6.667f * classDiff));

    }

    int CalculatePercentageBasedOnHealth(Ship raider, Ship target)
    {
        int raiderHealth = raider.currHealth;
        int targetHealth = target.currHealth;
        int healthDiff = targetHealth - raiderHealth;
        return Mathf.RoundToInt(32.5f - (0.325f * healthDiff));
    }

    int CalculatePercentageBasedOnCrew(Ship raider, Ship target)
    {
        int crewDiff = target.currCrewCapacity - raider.currCrewCapacity;
        return Mathf.RoundToInt(5 - (0.833f * crewDiff));
    }

    void PlaceShips()
    {
        foreach (Ship s in raidableShips)
        {
            s.MoveToRandPos();
        }
    }
}

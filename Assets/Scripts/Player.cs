using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject shipPrefab;

    [SerializeField]
    PlayerShipInfo playerShipInfo;

    [SerializeField]
    RaidUI raidUI;

    List<Ship> ships = new List<Ship>();
    List<RaidingShip> raidingShips = new List<RaidingShip>();
    public Ship shipToRaidWith;
    public bool isRaiding = false;

    public struct RaidingShip
    {
        public Ship raider;
        public Ship target;
        public RaidSpawner.RaidInfo raidInfo;
    };

    public void Start()
    {
        GetOrInitShips();
        PlaceShips();
    }

    void GetOrInitShips()
    {
        ships.Add(CreateNewShip("Black Pearl"));
        ships.Add(CreateNewShip("Queen Anne's Revenge"));
    }

    Ship CreateNewShip(string name)
    {
        Ship starter = Instantiate(shipPrefab, transform.position, Quaternion.identity).GetComponent<Ship>();
        starter.Initialize(name, ShipCatalog.instance.GetShipClassForName("Sloop"));
        starter.SetClickHandler(SelectShip);
        return starter;
    }

    void PlaceShips()
    {
        foreach (Ship s in ships)
        {
            int[] randCoord = GameManager.instance.map.PlaceUnitAtRandomPos(s.gameObject);
            s.gridPos = randCoord;
        }
    }

    void SelectShip(Ship ship)
    {
        if (!isRaiding && !IsShipInRaid(ship.name))
        {
            playerShipInfo.ShowInfo(ship);
        }
    }

    public bool IsShipInRaid(string name)
    {
        foreach (RaidingShip s in raidingShips)
        {
            if (s.raider.name == name || s.target.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void SelectRaidTarget()
    {
        raidUI.Show();
        isRaiding = true;
        shipToRaidWith = playerShipInfo.GetSelectedShip();
        playerShipInfo.Hide();
    }

    public void StartRaid(Ship shipToRaid, RaidSpawner.RaidInfo raidInfo)
    {
        raidingShips.Add(new RaidingShip {
            raider = shipToRaidWith,
            target = shipToRaid,
            raidInfo = raidInfo
        });
        CloseRaidTargetSelect();
    }

    void MoveRaidingShip(RaidingShip s)
    {
        Ship raider = s.raider;
        Ship target = s.target;
        int[] newGridPos = raider.GetCoordinateTowardsDest(target.gridPos);
        GameManager.instance.map.MoveObject(raider.gameObject, newGridPos);
        raider.SetGridPos(newGridPos);
    }

    bool isRaidingShipAtDest(RaidingShip s)
    {
        Ship raider = s.raider;
        Ship target = s.target;
        return raider.gridPos[0] == target.gridPos[0] && raider.gridPos[1] == target.gridPos[1];
    }

    public void MoveRaidingShips()
    {
        foreach (RaidingShip s in raidingShips)
        {
            if (isRaidingShipAtDest(s))
            {
                ProcessRaidOutcome(s);
            } else
            {
                MoveRaidingShip(s);
            }
        }
    }

    void ProcessRaidOutcome(RaidingShip s)
    {
        Debug.Log("Processing raid outcome: " + s.raider.name);
    }

    public void CloseRaidTargetSelect()
    {
        raidUI.Hide();
        isRaiding = false;
        shipToRaidWith = null;
    }
}

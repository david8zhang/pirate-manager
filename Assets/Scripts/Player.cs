﻿using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject shipPrefab;
    [SerializeField] PlayerShipInfo playerShipInfo;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] SuccessfulRaidResult successfulRaidResult;
    [SerializeField] FailedRaidResult failedRaidResult;

    List<Ship> ships = new List<Ship>();
    List<Raid> raidingShips = new List<Raid>();
    Stack<RaidOutcome> raidOutcomes = new Stack<RaidOutcome>();

    public Ship shipToRaidWith;
    public bool isRaiding = false;

    int totalGold = 0;

    public struct Raid
    {
        public Ship raider;
        public Ship target;
        public RaidSpawner.RaidInfo raidInfo;
    };

    public enum RaidOutcomeStatus { Success, Failure };

    public struct RaidOutcome
    {
        public int goldAmount;
        public bool isCaptured;
        public bool isLost;
        public int damageTaken;
        public int crewLost;
        public RaidOutcomeStatus status;
        public Raid raid;
    }

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
            playerUI.Hide();
            playerShipInfo.ShowInfo(ship);
        }
    }

    public void BackToOverworld()
    {
        playerUI.Show();
    }

    public bool IsShipInRaid(string name)
    {
        foreach (Raid r in raidingShips)
        {
            if (r.raider.name == name || r.target.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void SelectRaidTarget()
    {
        playerUI.Show();
        playerUI.ShowCancelRaid();
        isRaiding = true;
        shipToRaidWith = playerShipInfo.GetSelectedShip();
        playerShipInfo.Hide();
    }

    public void StartRaid(Ship shipToRaid, RaidSpawner.RaidInfo raidInfo)
    {
        raidingShips.Add(new Raid {
            raider = shipToRaidWith,
            target = shipToRaid,
            raidInfo = raidInfo
        });
        CloseRaidTargetSelect();
    }

    void MoveRaidingShip(Raid r)
    {
        Ship raider = r.raider;
        Ship target = r.target;
        int[] newGridPos = raider.GetCoordinateTowardsDest(target.gridPos);
        GameManager.instance.map.MoveObject(raider.gameObject, newGridPos);
        raider.SetGridPos(newGridPos);
    }

    bool isRaidingShipAtDest(Raid r)
    {
        Ship raider = r.raider;
        Ship target = r.target;
        return raider.gridPos[0] == target.gridPos[0] && raider.gridPos[1] == target.gridPos[1];
    }

    public void Tick()
    {
        List<Raid> finishedRaids = MoveRaidingShips();
        ClearFinishedRaids(finishedRaids);

        if (raidOutcomes.Count > 0)
        {
            ProcessRaidOutcomes();
            ShowNextRaidOutcome();
            playerUI.SetTotalGoldAmount(totalGold);
        }

    }

    public void ClearFinishedRaids(List<Raid> finishedRaids)
    {
        foreach (Raid r in finishedRaids)
        {
            raidingShips.Remove(r);
        }
    }

    public void ProcessRaidOutcomes()
    {
        foreach (RaidOutcome raidOutcome in raidOutcomes)
        {
            Ship s = raidOutcome.raid.raider;
            Ship target = raidOutcome.raid.target;
            if (raidOutcome.status == RaidOutcomeStatus.Success)
            {
                totalGold += raidOutcome.goldAmount;
                if (raidOutcome.isCaptured)
                {
                    // Remove enemy ship
                    CaptureShip(target);
                } else
                {
                    SinkShip(target);
                }
            } else
            {
                if (raidOutcome.isLost)
                {
                    LoseShip(s);
                }
                else
                {
                    MoveShipToAdjacent(s);
                }
            }
            s.LoseCrew(raidOutcome.crewLost);
            s.TakeDamage(raidOutcome.damageTaken);
        }
    }

    public void SinkShip(Ship s)
    {
        GameManager gm = GameManager.instance;
        gm.map.RemoveObj(s.gameObject);
        gm.raidSpawner.LoseRaidableShip(s);
    }

    public void MoveShipToAdjacent(Ship s)
    {
        int[] adjPos = s.FindEmptyAdjacentPosition();
        s.MoveToPos(adjPos);
    }

    public void LoseShip(Ship lostShip)
    {
        int indexToRemove = 0;
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i].name == lostShip.name)
            {
                indexToRemove = i;
            }
        }
        ships.RemoveAt(indexToRemove);
        GameManager.instance.map.RemoveObj(lostShip.gameObject);
    }

    public void CaptureShip(Ship capturedShip)
    {
        GameManager.instance.raidSpawner.LoseRaidableShip(capturedShip);
        ships.Add(capturedShip);
        int[] adjPos = capturedShip.FindEmptyAdjacentPosition();
        if (adjPos != null)
        {
            capturedShip.MoveToPos(adjPos);
        } else
        {
            capturedShip.MoveToRandPos();
        }
        capturedShip.SetClickHandler(SelectShip);
        capturedShip.SetColor(new Color32(255, 255, 255, 255));
    }

    public List<Raid> MoveRaidingShips()
    {
        List<Raid> finishedRaids = new List<Raid>();
        for (int i = 0; i < raidingShips.Count; i++)
        {
            Raid r = raidingShips[i];
            if (isRaidingShipAtDest(r))
            {
                finishedRaids.Add(r);
                GenerateRaidOutcome(r);
            } else
            {
                MoveRaidingShip(r);
            }
        }
        return finishedRaids;
    }

    void GenerateRaidOutcome(Raid r)
    {
        RaidSpawner.RaidInfo raidInfo = r.raidInfo;
        float successRate = raidInfo.successRate;
        float successNum = Random.Range(1f, 100f);
        RaidOutcome raidOutcome = new RaidOutcome();
        if (successNum <= successRate)
        {
            raidOutcome.status = RaidOutcomeStatus.Success;
            raidOutcome.goldAmount = raidInfo.goldAmount;
            float capturedNum = Random.Range(1f, 100f);
            float captureRate = raidInfo.captureChance;
            if (capturedNum <= captureRate)
            {
                raidOutcome.isCaptured = true;
            }
        } else
        {
            raidOutcome.status = RaidOutcomeStatus.Failure;
            float lossRate = Random.Range(1f, 100f);
            if (lossRate <= 10)
            {
                raidOutcome.isLost = true;
            }
        }
        raidOutcome.damageTaken = CalculateDamageTaken(r, successNum <= successRate);
        raidOutcome.crewLost = CalculateCrewLost(r, successNum <= successRate);
        raidOutcome.raid = r;
        raidOutcomes.Push(raidOutcome);
    }

    public void ShowNextRaidOutcome()
    {
        if (GameManager.instance.IsTimeMoving())
        {
            GameManager.instance.Pause();
        }
        if (raidOutcomes.Count > 0)
        {
            RaidOutcome raidOutcome = raidOutcomes.Pop();
            if (raidOutcome.status == RaidOutcomeStatus.Success)
            {
                successfulRaidResult.ShowRaidOutcome(raidOutcome);
            }
            else
            {
                failedRaidResult.ShowRaidOutcome(raidOutcome);
            }
        } else
        {
            GameManager.instance.Play();
        }
    }

    int CalculateCrewLost(Raid r, bool isWin)
    {
        float percentCrewLost;
        if (isWin)
        {
            percentCrewLost = Random.Range(0f, 0.2f);
        }
        else
        {
            percentCrewLost = Random.Range(0.5f, 1f);
        }
        return Mathf.RoundToInt(r.raider.currCrewCapacity * percentCrewLost);
    }

    int CalculateDamageTaken(Raid r, bool isWin)
    {
        float percentHealth;
        if (isWin)
        {
            percentHealth = Random.Range(0f, 0.1f);
            
        } else
        {
            percentHealth = Random.Range(0.5f, 1f);
        }
        return Mathf.RoundToInt(r.raider.currHealth * percentHealth);
    }

    public void CloseRaidTargetSelect()
    {
        playerUI.HideCancelRaid();
        isRaiding = false;
        shipToRaidWith = null;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    public new string name;
    public int currHealth;
    public int currCrewCapacity;
    public ShipClass shipClass;
    public int[] gridPos;

    public SpriteRenderer spriteRenderer;

    public delegate void ClickDelegate(Ship ship);
    public ClickDelegate onClickFn;
    public bool isRepairing = false;

    readonly int baseRepairRate = 10;
    int[][] directions = new int[][]
    {
        new int[] { 0, 1 },
        new int[] { 0, -1 },
        new int[] { -1, 0 },
        new int[] { 1, 0 }
    };

    public void Initialize(string name, ShipClass shipClass)
    {
        this.name = name;
        this.shipClass = shipClass;
        currHealth = shipClass.defaultMaxHealth;
        currCrewCapacity = shipClass.defaultCrewCapacity;
        GetComponent<SpriteRenderer>().sprite = shipClass.sprite;
    }

    public void Upgrade()
    {
        shipClass = ShipCatalog.instance.GetShipClassForName(shipClass.nextShipClassName);
        GetComponent<SpriteRenderer>().sprite = shipClass.sprite;
    }

    public void SetColor(Color32 color)
    {
        spriteRenderer.color = color;
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        currHealth = Mathf.Max(0, currHealth);
    }

    public void LoseCrew(int numLostCrew)
    {
        currCrewCapacity -= numLostCrew;
        currCrewCapacity = Mathf.Max(0, currCrewCapacity);
    }

    public void SetClickHandler(ClickDelegate onClickFn)
    {
        this.onClickFn = onClickFn;
    }

    public void SetGridPos(int[] coord)
    {
        gridPos = coord;
    }

    public void MoveToPos(int[] coord)
    {
        gridPos = coord;
        GameManager.instance.map.MoveObject(gameObject, coord);
    }

    public void MoveToRandPos()
    {
        int[] randomPos = GameManager.instance.map.PlaceUnitAtRandomPos(gameObject);
        gridPos = randomPos;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        onClickFn(this);
    }

    public int[] FindEmptyAdjacentPosition()
    {
        int[][] diagonals = new int[][]
        {
            new int[] { -1, 1 },
            new int[] { -1, -1 },
            new int[] { 1, -1 },
            new int[] { 1, 1 }
        };
        List<int[]> allDirs = new List<int[]>();
        allDirs.AddRange(diagonals);
        allDirs.AddRange(directions);
        Map map = GameManager.instance.map;
        foreach (int[] dir in allDirs)
        {
            int newPosX = gridPos[0] + dir[0];
            int newPosY = gridPos[1] + dir[1];
            int[] newPos = new int[] { newPosX, newPosY };
            if (map.CheckWithinBounds(newPos) && !map.IsObjectAtPos(newPosX, newPosY))
            {
                return newPos;
            }
        }
        return null;
    }

    public int[] GetCoordinateTowardsDest(int[] dest)
    {
        int[] closestPoint = new int[0];
        int closestDistance = Int32.MaxValue;
        foreach (int[] dir in directions)
        {
            int newPosX = gridPos[0] + dir[0];
            int newPosY = gridPos[1] + dir[1];
            int[] newPos = new int[] { newPosX, newPosY };
            if (newPos[0] == dest[0] && newPos[1] == dest[1])
            {
                return newPos;
            }
            if (GameManager.instance.map.CheckWithinBounds(newPos) && !GameManager.instance.map.IsObjectAtPos(newPosX, newPosY))
            {
                int distance = Map.GetDistanceBetweenPoints(newPos, dest);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = newPos;
                }
            }
        }
        return closestPoint;
    }

    int GetRepairRate()
    {
        return baseRepairRate * (currCrewCapacity + 1);
    }

    public void Repair()
    {
        int repairRate = GetRepairRate();
        currHealth += repairRate;
        currHealth = Math.Min(currHealth, shipClass.defaultMaxHealth);
    }

    public int GetRepairDuration()
    {
        int repairRate = GetRepairRate();
        int healthDiff = shipClass.defaultMaxHealth - currHealth;
        return Mathf.CeilToInt(healthDiff / repairRate) + 1;
    }
}

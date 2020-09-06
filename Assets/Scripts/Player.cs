using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject shipPrefab;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    PlayerShipInfo playerShipInfo;

    List<Ship> ships = new List<Ship>();

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
            s.transform.SetParent(canvas.transform);
        }
    }

    void SelectShip(Ship ship)
    {
        playerShipInfo.ShowInfo(ship);
    }
}

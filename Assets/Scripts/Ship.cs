using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    public new string name;
    public int currHealth;
    public int currCrewCapacity;
    public ShipClass shipClass;
    public int[] gridPos;

    public delegate void ClickDelegate(Ship ship);
    public ClickDelegate onClickFn;

    public void Initialize(string name, ShipClass shipClass)
    {
        this.name = name;
        this.shipClass = shipClass;
        currHealth = shipClass.defaultMaxHealth;
        currCrewCapacity = shipClass.defaultCrewCapacity;
        GetComponent<SpriteRenderer>().sprite = shipClass.sprite;
    }

    public void SetClickHandler(ClickDelegate onClickFn)
    {
        this.onClickFn = onClickFn;
    }

    public void SetGridPos(int[] coord)
    {
        gridPos = coord;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        onClickFn(this);
    }
}

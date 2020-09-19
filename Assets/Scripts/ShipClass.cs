using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Class", menuName = "Ship Class")]
public class ShipClass : ScriptableObject
{
    public string shipClassName;
    public Sprite sprite;
    public int defaultMaxHealth;
    public int defaultCrewCapacity;
    public int crewHireCost;
    public int upgradeCost;
    public string nextShipClassName;
}

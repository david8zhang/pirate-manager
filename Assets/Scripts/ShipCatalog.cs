using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCatalog : MonoBehaviour
{
    public static ShipCatalog instance;

    public void Awake()
    {
        instance = this;
    }

    public ShipClass GetShipClassForName(string className)
    {
        return (ShipClass)Resources.Load("ShipClasses/" + className);
    }
}

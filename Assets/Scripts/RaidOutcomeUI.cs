using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidOutcomeUI : MonoBehaviour
{
    [SerializeField]
    GameObject successfulRaidResult;

    [SerializeField]
    GameObject unsuccessfulRaidResult;

    [SerializeField]
    GameObject shipCapturedOrLost;


    public void ShowRaidResult(Player.RaidOutcome raidOutcome)
    {
    }

}
